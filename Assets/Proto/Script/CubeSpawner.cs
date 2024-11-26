using System.Collections.Generic;
using Elias.Scripts.Managers;
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
        public Cube lastCube;
        public float moveSpeed = 2f; // Speed for smooth following

        private Transform _transform;
        private Vector3 _targetPosition;

        private void Awake()
        {
            CubeCount = 0;
            CubeLimit = 5;
            _transform = transform;
            _targetPosition = _transform.position;
        }

        private void Update()
        {
            if (SpawnCube)
            {
                if (CubeCount > 0 && GameManager.Instance.GetBlocCount() > 0 &&
                    GameManager.Instance.GetBlocCount() % CubeLimit == 0)
                {
                    SpawnNewCube();
                }
                SpawnCube = false;
            }

            // Smoothly follow the target position
            _transform.position = Vector3.Lerp(_transform.position, _targetPosition, Time.deltaTime * moveSpeed);
        }

        public void GoRight()
        {
            _targetPosition += new Vector3(0, 0, 1);
        }

        public void GoLeft()
        {
            _targetPosition -= new Vector3(0, 0, 1);
        }

        [ContextMenu("Spawn Cube")]
        public void SpawnCubeMethod()
        {
            SpawnNewCube();
        }

        private void SpawnNewCube()
        {
            GameObject newCube = Instantiate(
                cubeModels[Random.Range(0, cubeModels.Count)],
                _transform.position,
                Quaternion.Euler(rotations[Random.Range(0, rotations.Count)], rY[Random.Range(0, rY.Count)], 0),
                this.transform);

            lastCube = newCube.GetComponent<Cube>();
            newCube.GetComponent<MeshRenderer>().material.color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f));

            // Register the new cube with the GameManager
            GameManager.Instance.AddBloc(lastCube);

            newCube.GetComponent<MeshRenderer>().material.DisableKeyword("_ISFREEZED");
            CubeCount++;
        }

        private void FixedUpdate()
        {
            if (CubeCount == CubeLimit)
            {
                FreezeCubes();
            }
        }

        private void FreezeCubes()
        {
            SpawnCube = false;
            if (CubeCount == CubeLimit)
            {
                var cubesToFreeze = GameManager.Instance.GetBlocCount();

                for (int i = cubesToFreeze - 1; i >= 0; i--)
                {
                    Cube cube = GameManager.Instance.Blocs[i];
                    cube.Freeze = true;
                    Debug.Log(cube.name + " is freezed");
                    CubeCount--;

                    // Remove the cube from the GameManager's list
                    GameManager.Instance.RemoveBloc(cube);
                }
            }
        }
    }
}
