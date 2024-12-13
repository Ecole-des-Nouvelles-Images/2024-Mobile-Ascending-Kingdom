using System;
using System.Collections;
using System.Collections.Generic;
using Elias.Scripts.Managers;
using Elias.Scripts.UIs;
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

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        _meshRenderer = GetComponent<MeshRenderer>();
        _cubeSpawner = FindObjectOfType<CubeSpawner>();
    }

    private void FixedUpdate()
    {
        if (!_cubeSpawner.isPaused && !transform.CompareTag("Solid"))
        {
            TranslateDown(1);
        }
    }

    private void Update()
    {
        if (!_cubeSpawner.isPaused && !transform.CompareTag("Solid") && GoDown)
        {
            TranslateDown(3);
        }
        if (Freeze)
        {
            FreezeObject();
        }
    }

    private void TranslateDown(float multiplier)
    {
        transform.Translate(Vector3.down * _speed * multiplier * Time.deltaTime, Space.World);
    }

    private void FreezeObject()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Rigidbody.useGravity = false;
        SetShaderBool("_ISFREEZED");
        Freeze = false;
    }

    private void SetShaderBool(string propertyName)
    {
        _meshRenderer.material.EnableKeyword(propertyName);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Solid") || gameObject.CompareTag("Solid")) return;
        
        AudioManager.Instance.PlaySound(AudioManager.Instance.blocPlaced);
        
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
        if (GameManager.Instance.EventActive && GameManager.Instance._currentEvent.eventName == "Volcano" && this.gameObject.CompareTag("Solid"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            GameManager.Instance.RemoveBloc(this);
            GameManager.Instance.BlocksDestroyed++;
        }
        if (GameManager.Instance.EventActive && GameManager.Instance._currentEvent.eventName == "Tempest" && this.gameObject.CompareTag("Solid"))
        {
            if (_cubeSpawner != null && _cubeSpawner.cubeModels.Count > 0)
            {
                Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), Random.Range(-1f, 1f)).normalized;
                Rigidbody.AddForce(forceDirection * 30f, ForceMode.Impulse);
                GameManager.Instance.BlocksChanged++;
            }
        }
    }
}
