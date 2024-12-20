using System.Collections.Generic;
using Elias.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;
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

        private bool tempbool;

        public bool AutoStart;
        
        public bool isPaused = false;
        
        private List<Color> dullColors = new List<Color>
        {
            new Color(1f, 0.6f, 0.6f), // Light Reddish Gray
            new Color(0.6f, 1f, 0.6f), // Light Greenish Gray
            new Color(0.6f, 0.6f, 1f), // Light Cyan Gray
            new Color(1f, 1f, 0.6f), // Light Orange Gray
            new Color(1f, 0.6f, 1f), // Light Purple Gray
            new Color(0.6f, 1f, 1f),
            new Color(1f, 1f, 1f), // Light Lime Gray
            new Color(0.6f, 0.6f, 0.6f)  // Light Spring Green Gray
        };


        // Liste des offsets en fonction du nom de la pièce et de sa rotation
        public Dictionary<string, Dictionary<float, Vector3>> offsets = new Dictionary<string, Dictionary<float, Vector3>>()
        {
            {
                "I", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0.5f) },
                    { 90f, new Vector3(0, 0, 0f) },
                    { 180f, new Vector3(0,0,-0.5f)},
                    { 270f, new Vector3(0,0,0)},
                    { 360f, new Vector3(0,0,0f)}
                }
            },
            {
                "Ialt", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0f) },
                    { 90f, new Vector3(0, 0, 0.5f) },
                    { 180f, new Vector3(0,0,0f)},
                    { 270f, new Vector3(0,0,-0.5f)},
                    { 360f, new Vector3(0,0,0f)}
                }
            },
            {
                "O", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0.5f) },
                    { 90f, new Vector3(0, 0, 0.5f) },
                    { 180f, new Vector3(0,0,-0.5f)},
                    { 270f, new Vector3(0,0,-0.5f)},
                    { 360f, new Vector3(0,0,0f)}
                }
            },
            {
                "Zalt", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0f) },
                    { 90f, new Vector3(0, 0, -0.5f) },
                    { 180f, new Vector3(0,0,0f)},
                    { 270f, new Vector3(0,0,0.5f)},
                    { 360f, new Vector3(0,0,0f)}
                }
            },
            {
                "Z", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, -0.5f) },
                    { 90f, new Vector3(0, 0, 0f) },
                    { 180f, new Vector3(0,0,0.5f)},
                    { 270f, new Vector3(0,0,0f)},
                    { 360f, new Vector3(0,0,-0.5f)}
                }
            },
            {
                "Lalt", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0f) },
                    { 90f, new Vector3(0, 0, 0.5f) },
                    { 180f, new Vector3(0,0,0f)},
                    { 270f, new Vector3(0,0,-0.5f)},
                    { 360f, new Vector3(0,0,0f)}
                }
            },
            {
                "L", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0.5f) },
                    { 90f, new Vector3(0, 0, 0f) },
                    { 180f, new Vector3(0,0,-0.5f)},
                    { 179f, new Vector3(0,0,-0.5f)},
                    { 269f, new Vector3(0,0,0f)},
                    { 270f, new Vector3(0,0,0f)},
                    { 359f, new Vector3(0,0,0.5f)},
                    { 360f, new Vector3(0,0,0.5f)}
                }
            },
            {
                "T", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0f) },
                    { 90f, new Vector3(0, 0, -0.5f) },
                    { 180f, new Vector3(0,0,0f)},
                    { 270f, new Vector3(0,0,0.5f)},
                    { 360f, new Vector3(0,0,0f)}
                }
            },
            {
                "Talt", new Dictionary<float, Vector3>()
                {
                    { 0f, new Vector3(0, 0, 0.5f) },
                    { 90f, new Vector3(0, 0, 0f) },
                    { 180f, new Vector3(0,0,-0.5f)},
                    { 270f, new Vector3(0,0,0f)},
                    { 360f, new Vector3(0,0,0.5f)}
                }
            },
        };

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
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
            if (!isPaused && SpawnCube)
            {
                if (GameManager.Instance.GetBlocCount() > 0 && GameManager.Instance.Blocs[^1].CompareTag("Solid"))
                {
                    Indicator.transform.position = new Vector3(0,0,0f);
                    GameObject newCube = Instantiate(cubeModels[Random.Range(0, cubeModels.Count)], transform.position, Quaternion.identity, this.transform);

                    lastCube = newCube.GetComponent<Bloc>();

                    // Set a random color from the brightColors list
                    newCube.GetComponent<MeshRenderer>().material.color = dullColors[Random.Range(0, dullColors.Count)];

                    GameManager.Instance.AddBloc(newCube.GetComponent<Bloc>());
                    newCube.GetComponent<MeshRenderer>().material.DisableKeyword("_ISFREEZED");
                    Bounds bounds = newCube.GetComponent<MeshRenderer>().bounds;
                    float lengthZ = bounds.size.z;

                    // Mettre à jour la taille de l'indicateur
                    UpdateIndicatorSize(lengthZ);

                    // Appliquer l'offset en fonction du nom de la pièce et de sa rotation
                    ApplyOffset(lastCube);

                    transform.position = new Vector3(transform.position.x,transform.position.y, transform.position.z);
                }
                SpawnCube = false;
            }
        }

        [ContextMenu("Spawn Cube")]
        public void SpawnCubeMethod()
        {
            Indicator.transform.position = new Vector3(0,0,0f);
            GameObject newCube = Instantiate(cubeModels[Random.Range(0, cubeModels.Count)], transform.position, Quaternion.identity, this.transform);
            lastCube = newCube.GetComponent<Bloc>();

            // Set a random color from the brightColors list
            newCube.GetComponent<MeshRenderer>().material.color = dullColors[Random.Range(0, dullColors.Count)];

            GameManager.Instance.AddBloc(newCube.GetComponent<Bloc>());
            newCube.GetComponent<MeshRenderer>().material.DisableKeyword("_ISFREEZED");
            Bounds bounds = newCube.GetComponent<MeshRenderer>().bounds;
            float lengthZ = bounds.size.z;

            // Mettre à jour la taille de l'indicateur
            UpdateIndicatorSize(lengthZ);

            // Appliquer l'offset en fonction du nom de la pièce et de sa rotation
            ApplyOffset(lastCube);

            transform.position = new Vector3(transform.position.x,transform.position.y, transform.position.z);
        }


        [ContextMenu("Freeze Cube")]
        public void FreezeMethod()
        {
            FreezeCubes(GameManager.Instance.Blocs);
        }

        public void FreezeCubes(List<Bloc> blocs)
        {
            SpawnCube = false;
            foreach (Bloc cube in blocs)
            {
                cube.Freeze = true;
            }
        }

        public void UpdateIndicatorSize(float lengthZ)
        {
            if (Indicator != null)
            {
                Vector3 newScale = Indicator.transform.localScale;
                newScale.x = lengthZ;
                Indicator.transform.localScale = newScale;
            }
        }

        public void ApplyOffset(Bloc newCube)
        {
            string cubeName = newCube.Shape;
            float rotationX = (int)QuaternionToEulerAngles(newCube.transform.rotation).x;
            Debug.Log((int)QuaternionToEulerAngles(newCube.transform.rotation).x);

            if (offsets.ContainsKey(cubeName) && offsets[cubeName].ContainsKey(rotationX))
            {
                Vector3 offset = offsets[cubeName][rotationX];
                Indicator.transform.localPosition = offset;
            }
        }
        
        Vector3 QuaternionToEulerAngles(Quaternion q)
        {
            // Calcul des angles d'Euler en utilisant les relations trigonométriques
            float rollX = Mathf.Atan2(2 * (q.w * q.x + q.y * q.z), 1 - 2 * (q.x * q.x + q.y * q.y)); // Rotation autour de X
            float pitchY = Mathf.Asin(2 * (q.w * q.y - q.z * q.x)); // Rotation autour de Y
            float yawZ = Mathf.Atan2(2 * (q.w * q.z + q.x * q.y), 1 - 2 * (q.y * q.y + q.z * q.z)); // Rotation autour de Z

            // Convertir les angles de radians en degrés
            float x = Mathf.Rad2Deg * rollX;
            float y = Mathf.Rad2Deg * pitchY;
            float z = Mathf.Rad2Deg * yawZ;

            // Normaliser les angles entre 0 et 360
            x = NormalizeAngle(x);
            y = NormalizeAngle(y);
            z = NormalizeAngle(z);

            return new Vector3(x, y, z);
        }

        // Fonction pour normaliser un angle entre 0 et 360°
        float NormalizeAngle(float angle)
        {
            angle = angle % 360; // Réduire l'angle dans la plage [-360, 360]
            if (angle < 0) angle += 360; // Si l'angle est négatif, le convertir en positif
            return angle;
        }
    }
    
    
}
