using Cinemachine;
using UnityEngine;

namespace Elias.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        public GameObject CubeSpawn;
        public CinemachineTargetGroup TargetGroup;
        public CameraHeightCheck CameraHeightCheck;
        public float moveSpeed = 2f; // Speed for smooth following

        private bool _up;
        private Transform _cubeSpawnTransform;
        private Vector3 _targetPosition;

        private void Start()
        {
            _cubeSpawnTransform = CubeSpawn.GetComponent<Transform>();
            _targetPosition = _cubeSpawnTransform.position;

            if (CameraHeightCheck == null)
            {
                Debug.LogError("CameraHeightCheck reference is not assigned!");
            }
        }

        private void Update()
        {
            if (!CameraHeightCheck.TowerLowerDetection)
            {
                MoveObjects(-1f);
            }

            // Smoothly follow the target position
            _cubeSpawnTransform.position = Vector3.Lerp(_cubeSpawnTransform.position, _targetPosition, Time.deltaTime * moveSpeed);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_up && other.CompareTag("Solid"))
            {
                MoveObjects(3f);
                _up = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Solid"))
            {
                _up = false;
            }
        }

        private void MoveObjects(float offsetY)
        {
            _targetPosition = new Vector3(_cubeSpawnTransform.position.x, _cubeSpawnTransform.position.y + offsetY, _cubeSpawnTransform.position.z);

            foreach (var target in TargetGroup.m_Targets)
            {
                if (target.target != null)
                {
                    Vector3 targetPosition = target.target.position;
                    target.target.position = new Vector3(targetPosition.x, targetPosition.y + offsetY, targetPosition.z);
                }
            }
        }
    }
}
