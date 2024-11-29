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
            Bloc piece = other.GetComponent<Bloc>();
            if (piece != null)
            {
                GameManager.Instance.RemoveBloc(piece);

                Destroy(other.gameObject);

                _cubeSpawner.SpawnCube = true;
            }
        }
    }
}