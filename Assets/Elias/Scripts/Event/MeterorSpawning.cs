using System.Collections;
using UnityEngine;

namespace Elias.Scripts.Event
{
    public class MeterorSpawning : MonoBehaviour
    {
        public GameObject meteorPrefab;
        public Transform spawnPoint;
        private bool isSpawning = false;
        
        private float _speed = 0.5f; // Speed of movement
        private float _minZ = -4.0f; // Minimum x position
        private float _maxZ = 4.0f; // Maximum x position
        private bool _movingRight = true; // Direction of movement

        void Update()
        {
            Vector3 currentPosition = transform.position;

            if (_movingRight)
            {
                currentPosition.z += _speed * Time.deltaTime;
                if (currentPosition.z >= _maxZ)
                {
                    currentPosition.z = _maxZ;
                    _movingRight = false;
                }
            }
            else
            {
                currentPosition.z -= _speed * Time.deltaTime;
                if (currentPosition.z <= _minZ)
                {
                    currentPosition.z = _minZ;
                    _movingRight = true;
                }
            }

            transform.position = currentPosition;
        }
        public void StartSpawning()
        {
            if (!isSpawning)
            {
                isSpawning = true;
                StartCoroutine(SpawnMeteors());
            }
        }

        public void StopSpawning()
        {
            isSpawning = false;
            StopAllCoroutines();
        }

        private IEnumerator SpawnMeteors()
        {
            while (isSpawning)
            {
                GameObject meteor = Instantiate(meteorPrefab, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSeconds(3f);
            }
        }
    }
}
