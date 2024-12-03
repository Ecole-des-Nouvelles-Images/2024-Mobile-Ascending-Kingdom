using System;
using System.Collections;
using System.Collections.Generic;
using Proto.Script;
using UnityEngine;
using UnityEngine.Serialization;

public class Bloc : MonoBehaviour
{
    [FormerlySerializedAs("_rigidbody")] public Rigidbody Rigidbody;
    [SerializeField] private float _speed;
    public bool Freeze;
    private MeshRenderer _meshRenderer;
    public bool GoDown;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        _meshRenderer = GetComponent<MeshRenderer>();
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
}
