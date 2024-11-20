using UnityEngine;

namespace PreProdElias.Scripts.Camera
{
    
    public class CameraController : MonoBehaviour
    {
        public GameObject cubeSpawn;
        public GameObject treshold;
        private bool Up;
        private Transform cubeSpawnTransform;
        private Transform tresholdTransform;

        private void Start()
        { 
            cubeSpawnTransform = cubeSpawn.GetComponent<Transform>();
            tresholdTransform = treshold.GetComponent<Transform>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!Up)
            {
                if (other.CompareTag("Solid"))
                {
                    Vector3 transformPosition = transform.parent.position;
                    transform.parent.position = new Vector3(transformPosition.x, transformPosition.y +2f, transformPosition.z);
                    Vector3 cubeSpawnPosition = cubeSpawnTransform.position;
                    cubeSpawnTransform.position = new Vector3(cubeSpawnPosition.x, cubeSpawnPosition.y +2f, cubeSpawnPosition.z);
                    Up = true;
                }
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Solid"))
            {
                Up = false;

            }
        }
    }
}
