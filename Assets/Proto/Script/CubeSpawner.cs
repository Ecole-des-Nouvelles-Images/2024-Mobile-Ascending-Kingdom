using System;
using System.Collections;
using System.Collections.Generic;
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
                    GameObject newCube = Instantiate(CubePrefab, transform.position, Quaternion.Euler(0,0,Random.Range(0,360)));
                    Cubes.Add(newCube.GetComponent<Cube>());
                    transform.position = new Vector3(Random.Range(-1,1), transform.position.y + 2, transform.position.z);
                }
                SpawnCube = false;
            }

            if (CubeCount >= CubeLimit)
            {
                StartCoroutine(FreezeCubes());
            }
        }

        [ContextMenu("Spawn Cube")]
        public void SpawnCubeMethod()
        {
            GameObject newCube = Instantiate(CubePrefab, transform.position, Quaternion.identity);
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
                    cube.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                    Cubes.Remove(cube);
                    if (Cubes.Count <= 0)
                    {
                        CubeCount = 0;
                        CubeLimit += 5;
                        SpawnCubeMethod();
                    }
                }
                
            }
        }
    }
}
