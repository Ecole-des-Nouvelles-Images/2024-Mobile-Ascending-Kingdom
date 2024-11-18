using UnityEngine;
using UnityEngine.Serialization;

namespace Proto.Script
{
    public class Cube : MonoBehaviour
    {
        public Rigidbody rb;
        public float speed;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            if (!transform.CompareTag("Solid"))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Solid"))
            {
                this.transform.tag = "Solid";
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                  RigidbodyConstraints.FreezePositionZ;
                CubeSpawner spawner = FindObjectOfType<CubeSpawner>();
                spawner.CubeCount += 1;
                spawner.SpawnCube = true;
            }
        }
    }
}
