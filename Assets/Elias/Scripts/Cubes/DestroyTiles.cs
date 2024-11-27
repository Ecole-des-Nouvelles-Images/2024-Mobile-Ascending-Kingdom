using Elias.Scripts.Managers;
using Proto.Script;
using UnityEngine;

namespace Elias.Scripts.Cubes
{
    public class DestroyTiles : MonoBehaviour
    {
        private CubeSpawner _cubeSpawner;

        private void Awake()
        {
            _cubeSpawner = FindObjectOfType<CubeSpawner>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Cube cube = other.GetComponent<Cube>();
            if (cube != null)
            {
                GameManager.Instance.RemoveBloc(cube);

                Destroy(other.gameObject);

                _cubeSpawner.SpawnCube = true;
            }
        }
    }
}