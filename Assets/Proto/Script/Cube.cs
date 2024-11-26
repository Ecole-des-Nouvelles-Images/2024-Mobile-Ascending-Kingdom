using System;
using System.Collections.Generic;
using Elias.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Proto.Script
{
    public class Cube : MonoBehaviour
    {
        public Rigidbody rb;
        public float speed;
        public BoxCollider triggerCollider;
        public bool Freeze;
        private Material mat;
        private Material tempMaterial;
        public MeshRenderer meshRenderer;
        public bool goDown;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            tempMaterial = GetComponent<Renderer>().material;
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            if (!transform.CompareTag("Solid"))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
            }
        }

        private void Update()
        {
            if (!transform.CompareTag("Solid"))
            {
                if (goDown)
                {
                    transform.Translate(Vector3.down * speed * 3 * Time.deltaTime, Space.World);
                }
            }

            if (Freeze)
            {
                FreezeObject();
            }
        }

        private void FreezeObject()
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.useGravity = false;
            if (transform.parent == null)
            {
                SetShaderBool("_ISFREEZED",true);
                Freeze = false;
            }
            Freeze = false;
        }
        
        public void SetShaderBool(string propertyName, bool value)
        {
           meshRenderer.material.EnableKeyword(propertyName);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Solid") && !this.gameObject.CompareTag("Solid"))
            {
                triggerCollider.enabled = false;
                triggerCollider.isTrigger = false;
                rb.AddForce(Vector3.down * speed/2 * Time.deltaTime, ForceMode.VelocityChange);
                rb.velocity = Vector3.zero;
                this.transform.tag = "Solid";
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX;
                transform.SetParent(null);
                
                GameManager.Instance.AddBloc(this);//Elias
                
                CubeSpawner spawner = FindObjectOfType<CubeSpawner>();
                spawner.CubeCount += 1;
                spawner.SpawnCubeMethod();
            }
        }
        
        
    }
}
