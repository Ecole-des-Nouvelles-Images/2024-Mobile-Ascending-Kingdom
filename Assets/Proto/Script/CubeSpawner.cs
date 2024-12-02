using System;
using System.Collections;
using System.Collections.Generic;
using Elias.Scripts.Managers;
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
        public int CubeCount;
        public int CubeLimit;
        public List<GameObject> cubeModels = new List<GameObject>();
        public List<float> rotations = new List<float>();
        public List<float> rY = new List<float>();
        public GameObject cameraObject;
        public Bloc lastCube;
        public GameObject Indicator;

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
                if (GameManager.Instance.GetBlocCount() > 0 && GameManager.Instance.Blocs[^1].CompareTag("Solid"))
                {
                    GameObject newCube = Instantiate(cubeModels[Random.Range(0, cubeModels.Count)], transform.position, Quaternion.identity);

                    lastCube = newCube.GetComponent<Bloc>();
                    newCube.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f),
                        Random.Range(0f, 1f), Random.Range(0f, 1f));

                    GameManager.Instance.AddBloc(newCube.GetComponent<Bloc>());
                    newCube.GetComponent<MeshRenderer>().material.DisableKeyword("_ISFREEZED");
                    Bounds bounds = newCube.GetComponent<MeshRenderer>().bounds;
                    float lengthZ = bounds.size.z;
                    Indicator.transform.localScale = new Vector3(lengthZ, 200, lengthZ);
                    Indicator.transform.localPosition = new Vector3(newCube.transform.localPosition.x, 0, 0);
                    transform.position = new Vector3(transform.position.x,transform.position.y /*+ 2*/, transform.position.z);
                }
                SpawnCube = false;
            }
        }

        public void GoRight()
        {
            if (lastCube != null)
            {
                lastCube.transform.position += new Vector3(0, 0, 1);
            }
        }

        public void GoLeft()
        {
            if (lastCube != null)
            {
                lastCube.transform.position -= new Vector3(0, 0, 1);
            }
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
            GameObject newCube = Instantiate(cubeModels[Random.Range(0, cubeModels.Count)], transform.position, Quaternion.identity);
            lastCube = newCube.GetComponent<Bloc>();
            newCube.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            GameManager.Instance.AddBloc(newCube.GetComponent<Bloc>());
            newCube.GetComponent<MeshRenderer>().material.DisableKeyword("_ISFREEZED");
            Bounds bounds = newCube.GetComponent<MeshRenderer>().bounds;
            float lengthZ = bounds.size.z;
            Indicator.transform.localScale = new Vector3(lengthZ, 200, lengthZ);
            Indicator.transform.localPosition = new Vector3(newCube.transform.localPosition.x, 0, 0);
            transform.position = new Vector3(transform.position.x,transform.position.y /*+ 2*/, transform.position.z);
        }

        private void FreezeCubes()
        {
            SpawnCube = false;
            if (CubeCount == CubeLimit && GameManager.Instance.GetBlocCount() != 1)
            {
                for (int i = GameManager.Instance.GetBlocCount() - 1; i >= 0; i--)
                {
                    Bloc cube = GameManager.Instance.Blocs[i];
                    cube.Freeze = true;
                    Debug.Log(cube.name + " is freezed");
                    CubeCount -= 1;
                    GameManager.Instance.RemoveBloc(cube);
                }
            }
        }

        /*public void tempTap()
        {
            /*if (!CubeSpawner.lastCube.CompareTag("Solid"))
            {*/
                //Transform cubeTransform = CubeSpawner.lastCube.transform;
                // Créer une rotation de 90 degrés autour de l'axe X
                /*Quaternion rotation = Quaternion.Euler(90, 0, 0);
                // Appliquer la rotation à l'objet
                //cubeTransform.rotation *= rotation;
            //}
            Debug.Log("Tap Method Executed");
            
        }*/
    }
}
