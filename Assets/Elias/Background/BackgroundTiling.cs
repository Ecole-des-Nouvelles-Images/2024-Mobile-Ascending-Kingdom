using System.Collections.Generic;
using UnityEngine;

namespace Elias.Background
{
    public class InfiniteBackground : MonoBehaviour
    {
        public GameObject backgroundPrefab;
        public Camera mainCamera;
        public int initialSegments = 5;

        public Sprite startTile; // Sprite pour le début
        public Sprite moonTile; // Sprite pour la lune
        public Sprite transitionTile; // Sprite pour la transition
        public Sprite skyTile; // Sprite répétable (ciel)
        public Sprite nightTile; // Sprite répétable (nuit)

        private float _segmentHeight = 24f;
        private Queue<GameObject> _activeSegments;
        private bool _moonAppeared = false;
        private bool _transitionAppeared = false;

        void Start()
        {
            if (backgroundPrefab == null || mainCamera == null || startTile == null || moonTile == null || 
                transitionTile == null || skyTile == null || nightTile == null)
            {
                Debug.LogError("Assurez-vous de lier le prefab, la caméra et tous les sprites dans l'inspecteur !");
                return;
            }

            _activeSegments = new Queue<GameObject>();

            float startY = mainCamera.transform.position.y - mainCamera.orthographicSize;

            for (int i = 0; i < initialSegments; i++)
            {
                SpawnSegment(startY, i == 0 ? startTile : skyTile);
                startY += _segmentHeight;
            }
        }

        void Update()
        {
            GameObject firstSegment = _activeSegments.Peek();
            float segmentBottomY = firstSegment.transform.position.y + _segmentHeight;

            if (segmentBottomY < mainCamera.transform.position.y - mainCamera.orthographicSize)
            {
                // Recycler le segment avec un nouveau sprite
                float newY = _activeSegments.ToArray()[_activeSegments.Count - 1].transform.position.y + _segmentHeight;
                Sprite nextSprite = GetNextSprite();
                RecycleSegment(firstSegment, newY, nextSprite);
            }
        }

        void SpawnSegment(float spawnY, Sprite sprite)
        {
            GameObject newSegment = Instantiate(backgroundPrefab, new Vector3(backgroundPrefab.transform.position.x, 
                spawnY, backgroundPrefab.transform.position.z), backgroundPrefab.transform.rotation);

            SpriteRenderer sRenderer = newSegment.GetComponent<SpriteRenderer>();
            sRenderer.sprite = sprite;

            _activeSegments.Enqueue(newSegment);
        }

        void RecycleSegment(GameObject segment, float newY, Sprite sprite)
        {
            segment.transform.position = new Vector3(backgroundPrefab.transform.position.x, 
                newY, backgroundPrefab.transform.position.z);

            SpriteRenderer sRenderer = segment.GetComponent<SpriteRenderer>();
            sRenderer.sprite = sprite;

            _activeSegments.Dequeue();
            _activeSegments.Enqueue(segment);
        }

        Sprite GetNextSprite()
        {
            if (!_moonAppeared && Random.value < 0.5f)
            {
                _moonAppeared = true;
                return moonTile;
            }

            if (!_transitionAppeared && _moonAppeared && Random.value < 0.25f)
            {
                _transitionAppeared = true;
                return transitionTile;
            }

            if (_transitionAppeared)
            {
                return nightTile;
            }

            return skyTile;
        }
    }
}
