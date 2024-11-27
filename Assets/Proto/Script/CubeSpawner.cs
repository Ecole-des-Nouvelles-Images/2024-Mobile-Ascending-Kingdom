using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Proto.Script
{
    public class CubeSpawner : MonoBehaviour
    {
        public bool SpawnCube;
        public GameObject CubePrefab;
        public List<Bloc> Cubes = new List<Bloc>();
        public int CubeCount;
        public int CubeLimit;
        public List<GameObject> cubeModels = new List<GameObject>();
        public List<float> rotations = new List<float>();
        public List<float> rY = new List<float>();
        public GameObject cameraObject;
        public Bloc lastCube;

        public bool AutoStart;

        private void Awake()
        {
            CubeCount = 0;
            CubeLimit = -1;
        }

        private void Start()
        {
            if (AutoStart)
            {
                SpawnCubeMethod();
            }
        }

        private void Update()
        {
            if (SpawnCube)
            {
                if (Cubes.Count > 0 && Cubes[^1].CompareTag("Solid"))
                {
                    GameObject newCube = Instantiate(cubeModels[Random.Range(0,cubeModels.Count)], transform.position, Quaternion.Euler(rotations[Random.Range(0,rotations.Count)],rY[Random.Range(0, rY.Count)],0), this.transform);
                    lastCube = newCube.GetComponent<Bloc>();
                    newCube.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    Cubes.Add(newCube.GetComponent<Bloc>());
                    newCube.GetComponent<MeshRenderer>().material.DisableKeyword("_ISFREEZED");
                    transform.position = new Vector3(transform.position.x,transform.position.y /*+ 2*/, transform.position.z);
                }
                SpawnCube = false;
            }

            

        }

        public void GoRight()
        {
            transform.position += new Vector3(0, 0, 1);
        }

        public void GoLeft()
        {
            transform.position -= new Vector3(0, 0, 1);
        }

        private void FixedUpdate()
        {
            if (CubeCount == CubeLimit)
            {
                FreezeCubes();
            }
        }

        [ContextMenu("Spawn Cube")]
        public void SpawnCubeMethod()
        {
            GameObject newCube = Instantiate(cubeModels[Random.Range(0,cubeModels.Count)], transform.position, Quaternion.Euler(rotations[Random.Range(0,rotations.Count)],0,0), this.transform);
            lastCube = newCube.GetComponent<Bloc>();
            newCube.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Cubes.Add(newCube.GetComponent<Bloc>());
            newCube.GetComponent<MeshRenderer>().material.DisableKeyword("_ISFREEZED");
            transform.position = new Vector3(transform.position.x,transform.position.y /*+ 2*/, transform.position.z);
        }

        private void FreezeCubes()
        {
            SpawnCube = false;
            if (CubeCount == CubeLimit && Cubes.Count != 1)
            {
                for (int i = Cubes.Count - 1; i >= 0; i--)
                {
                    Bloc cube = Cubes[i];
                    cube.Freeze = true;
                    Debug.Log(cube.name + " is freezed");
                    CubeCount -= 1;
                    Cubes.RemoveAt(i);
                }
            }
            
        }
    }
}
