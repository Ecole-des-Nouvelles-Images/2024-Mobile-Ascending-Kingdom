using System;
using Proto.Script;
using UnityEngine;

namespace PreProdElias.Scripts
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
            if (_cubeSpawner.CubeCount !=0)
            {
                _cubeSpawner.CubeCount -= 1;
            }
            _cubeSpawner.Cubes.Remove(other.GetComponent<Bloc>());
            Destroy(other.gameObject);
            _cubeSpawner.SpawnCube = true;
        }
    }
}
