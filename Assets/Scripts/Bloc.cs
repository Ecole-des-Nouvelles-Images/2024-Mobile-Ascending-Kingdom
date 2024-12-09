using System;
using System.Collections;
using System.Collections.Generic;
using Elias.Scripts.Managers;
using Proto.Script;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Bloc : MonoBehaviour
{
    [FormerlySerializedAs("_rigidbody")] public Rigidbody Rigidbody;
    [SerializeField] private float _speed;
    public bool Freeze;
    private MeshRenderer _meshRenderer;
    public bool GoDown;
    private CubeSpawner _cubeSpawner;
    public string shape;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        _meshRenderer = GetComponent<MeshRenderer>();
        _cubeSpawner = FindObjectOfType<CubeSpawner>();
    }
    
    private void FixedUpdate() {
        if (!transform.CompareTag("Solid")) TranslateDown(1);
        
    }
    
    private void Update() {
        if (!transform.CompareTag("Solid")) if (GoDown)TranslateDown(3);
        if (Freeze) FreezeObject();
    }
    
    private void TranslateDown(float multiplier) {
        transform.Translate(Vector3.down * _speed * multiplier * Time.deltaTime, Space.World);
    }
    
    private void FreezeObject() {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Rigidbody.useGravity = false;
        SetShaderBool("_ISFREEZED");
        Freeze = false;
    }
    
    private void SetShaderBool(string propertyName) {
        _meshRenderer.material.EnableKeyword(propertyName);
    }
    
    private void OnCollisionEnter(Collision other) {
        if (!other.gameObject.CompareTag("Solid") || gameObject.CompareTag("Solid")) return;
//        Debug.Log(gameObject.name + " collided with " + other.gameObject.name);
        //_rigidbody.AddForce(Vector3.down * _speed/2 * Time.deltaTime, ForceMode.VelocityChange);
        Rigidbody.velocity = Vector3.zero;
        transform.tag = "Solid";
        Rigidbody.useGravity = true;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
                                 RigidbodyConstraints.FreezePositionX;
        transform.SetParent(null);
        CubeSpawner spawner = FindObjectOfType<CubeSpawner>();
        spawner.CubeCount += 1;
        spawner.SpawnCubeMethod();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("hihihi");
        if (GameManager.Instance.EventActive && GameManager.Instance._currentEvent.eventName == "Volcano")
        {
            Destroy(this.gameObject);
            GameManager.Instance.RemoveBloc(this);
            GameManager.Instance.BlocksDestroyed++;
        }
        if (GameManager.Instance.EventActive && GameManager.Instance._currentEvent.eventName == "Tempest")
        {
            if (_cubeSpawner != null && _cubeSpawner.cubeModels.Count > 0)
            {
                int randomIndex = Random.Range(0, _cubeSpawner.cubeModels.Count);
                GameObject newModel = _cubeSpawner.cubeModels[randomIndex];
                MeshFilter meshFilter = GetComponent<MeshFilter>();
                meshFilter.mesh = newModel.GetComponent<MeshFilter>().sharedMesh;
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.material = newModel.GetComponent<MeshRenderer>().sharedMaterial;
                GameManager.Instance.BlocksChanged++;
            }
        }
        
    }
}
