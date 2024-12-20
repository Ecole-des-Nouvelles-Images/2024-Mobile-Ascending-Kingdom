using UnityEngine;
using UnityEngine.Serialization;

namespace Elias.Background
{
    public class BackgroundTiling : MonoBehaviour
    {
        public GameObject backgroundPrefab;
        public Camera mainCamera;
        public float offset = 0.1f;
        private float _segmentHeight;
        private float _nextSpawnY;
        
        [SerializeField] private Sprite[] _backgroundSprites;

        void Start()
        {
            if (backgroundPrefab == null || mainCamera == null)
            {
                Debug.LogError("Assurez-vous de lier le prefab et la caméra dans l'inspecteur !");
                return;
            }

            SpriteRenderer spriteRenderer = backgroundPrefab.GetComponent<SpriteRenderer>();
            _segmentHeight = spriteRenderer.bounds.size.y;

            
            _nextSpawnY = mainCamera.transform.position.y - mainCamera.orthographicSize;
        
            // Instancier initialement plusieurs segments pour couvrir l'écran
            if (_nextSpawnY < mainCamera.transform.position.y + mainCamera.orthographicSize)
            {
                SpawnSegment(_nextSpawnY);
                _nextSpawnY += _segmentHeight;
            }
        }

        void Update()
        {
            float cameraTopY = mainCamera.transform.position.y + mainCamera.orthographicSize;

            if (cameraTopY >= _nextSpawnY - offset)
            {
                SpawnSegment(_nextSpawnY);
                _nextSpawnY += _segmentHeight;
            }
        }

        void SpawnSegment(float spawnY)
        {
            Instantiate(backgroundPrefab, new Vector3(0, spawnY, 0), Quaternion.identity);
        }
    }
}