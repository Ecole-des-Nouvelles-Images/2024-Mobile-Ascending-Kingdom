using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace PreProdElias.Scripts
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
                    transform.parent.position = new Vector3(transformPosition.x, transformPosition.y +1f, transformPosition.z);
                    Vector3 cubeSpawnPosition = cubeSpawnTransform.position;
                    cubeSpawnTransform.position = new Vector3(cubeSpawnPosition.x, cubeSpawnPosition.y +1f, cubeSpawnPosition.z);
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
