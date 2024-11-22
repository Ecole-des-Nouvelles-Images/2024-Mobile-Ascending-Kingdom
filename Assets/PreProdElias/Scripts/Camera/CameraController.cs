using Cinemachine;
using UnityEngine;

namespace PreProdElias.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        public GameObject CubeSpawn;
        public CinemachineTargetGroup TargetGroup;
        public CameraHeightCheck CameraHeightCheck;

        private bool _up;
        private Transform _cubeSpawnTransform;

        private void Start()
        {
            _cubeSpawnTransform = CubeSpawn.GetComponent<Transform>();

            if (CameraHeightCheck == null)
            {
                Debug.LogError("CameraHeightCheck reference is not assigned!");
            }
        }

        private void Update()
        {
            if (!CameraHeightCheck.TowerLowerDetection)
            {
                MoveObjects(-0.2f);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_up && other.CompareTag("Solid"))
            {
                MoveObjects(2f);
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
            Vector3 cubeSpawnPosition = _cubeSpawnTransform.position;
            _cubeSpawnTransform.position = new Vector3(cubeSpawnPosition.x, cubeSpawnPosition.y + offsetY, cubeSpawnPosition.z);

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
