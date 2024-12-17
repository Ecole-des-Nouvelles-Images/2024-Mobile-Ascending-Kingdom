using System.Collections;
using Elias.Scripts.Camera;
using Elias.Scripts.Managers;
using Elias.Scripts.UIs;
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
    private CubeSpawner _cubeSpawner;
    [FormerlySerializedAs("shape")] public string Shape;
    public GameObject Vines;
    public GameObject Dust;
    public bool IsFrozen { get; set; } // Add this property

    private CameraShake _cameraShake;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        _meshRenderer = GetComponent<MeshRenderer>();
        _cubeSpawner = FindObjectOfType<CubeSpawner>();
        _cameraShake = FindObjectOfType<CameraShake>();
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
        //Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Rigidbody.isKinematic = true;
        SetShaderBool("_ISFREEZED");
        StartCoroutine("FreezeAnimation");
        Freeze = false;
        IsFrozen = true; // Set the IsFrozen property to true
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
        Bounds bounds = _meshRenderer.bounds;
        Vector3 lowestPoint = bounds.min;
        GameObject dustGo = Instantiate(Dust,new Vector3(transform.position.x,lowestPoint.y,transform.position.z), Quaternion.identity, null);
        _cameraShake.StartShake();
        StartCoroutine(DustDestroyer(dustGo));
        
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
    private IEnumerator FreezeAnimation()
    {
        float vineAmountStep = 0.1f;
        float vineAmount = 0.1f;
        float scaleStep = 0.1f;
        float scale = 0.1f;

        // Animation de _VineAmount
        for (int i = 0; i < 10; i++)
        {
            _meshRenderer.material.SetFloat("_VineAmount", vineAmount);
            vineAmount += vineAmountStep;
            yield return new WaitForSeconds(0.2f);
        }

        Vines.SetActive(true);

        // Animation de l'échelle des Vines
        for (int i = 0; i < 10; i++)
        {
            Vines.transform.localScale = new Vector3(scale, scale, scale);
            scale += scaleStep;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DustDestroyer(GameObject dustGameObject)
    {
        yield return new WaitForSeconds(2f);
        DestroyImmediate(dustGameObject);
    }
}
