using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Proto.Script
{
    public class Cube : MonoBehaviour
    {
        public Rigidbody rb;
        public float speed;
        public BoxCollider triggerCollider;
        

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
                triggerCollider.enabled = false;
                rb.AddForce(Vector3.down * speed, ForceMode.Impulse);
                this.transform.tag = "Solid";
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
                CubeSpawner spawner = FindObjectOfType<CubeSpawner>();
                spawner.CubeCount += 1;
                spawner.SpawnCube = true;
            }
        }
    }
}
