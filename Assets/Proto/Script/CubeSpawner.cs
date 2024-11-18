using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Proto.Script
{
    public class CubeSpawner : MonoBehaviour
    {
        public bool SpawnCube;
        public GameObject CubePrefab;
        public List<Cube> Cubes = new List<Cube>();
        public int CubeCount;
        public int CubeLimit;
        public List<GameObject> cubeModels = new List<GameObject>();
        public List<float> coordinates = new List<float>();
        public GameObject cameraObject;

        private void Awake()
        {
            CubeCount = 0;
            CubeLimit = 5;
        }

        private void Update()
        {
            if (SpawnCube)
            {
                if (Cubes[^1].CompareTag("Solid"))
                {
                    GameObject newCube = Instantiate(cubeModels[Random.Range(0,cubeModels.Count)], transform.position, transform.rotation);
                    Cubes.Add(newCube.GetComponent<Cube>());
                    transform.position = new Vector3(transform.position.x,transform.position.y + 2, transform.position.z);
                }
                SpawnCube = false;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1.5f, 0, 0);
                cameraObject.transform.position += new Vector3(-1.5f, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1.5f, 0, 0);
                cameraObject.transform.position += new Vector3(1.5f, 0, 0);
            }

            /*if (Cubes.Count == CubeLimit)
            {
                StartCoroutine(FreezeCubes());
                return;
            }*/
        }

        [ContextMenu("Spawn Cube")]
        public void SpawnCubeMethod()
        {
            GameObject newCube = Instantiate(cubeModels[Random.Range(0,cubeModels.Count)], transform.position, transform.rotation);
            Cubes.Add(newCube.GetComponent<Cube>());
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        }

        private IEnumerator FreezeCubes()
        {
            if (Cubes.Count <= 0)
            {
                yield break;
            }
            else
            {
                foreach (Cube cube in Cubes)
                {
                    cube.rb.useGravity = false;
                    Cubes.Remove(cube);
                    if (Cubes.Count <= 0)
                    {
                        CubeCount = 0;
                        CubeLimit += 5;
                        cube.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                        SpawnCubeMethod();
                    }
                }
                
            }
        }
    }
}
