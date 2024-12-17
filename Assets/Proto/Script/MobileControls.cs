using System;
using Proto.Script;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    private bool isSwiping = false;
    private bool isSliding = false;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private float slideStartX;
    private float slideEndX;
    public CubeSpawner cubeSpawner;
    public float sensitivityFactor = 2.0f;
    private float lastZPosition;
    public float slideSpeed = 5.0f; // Vitesse de glissement

    private float tapTimeWindow = 0.3f; // Fenêtre de temps pour détecter un double tap
    private float lastTapTime = 0f;
    private int tapCount = 0;

    public float minSlideDistance = -100.0f; // Distance minimale de glissement
    public float maxSlideDistance = 100.0f; // Distance maximale de glissement
    public float minSlideThreshold = 10.0f; // Seuil minimum de glissement

    private Vector3 targetPosition;

    void Start()
    {
        // Initialiser lastZPosition à 0 au début
        lastZPosition = 0;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    isSliding = true;
                    slideStartX = touch.position.x;
                    CheckDoubleTap(touch); // Vérifier le double tap lorsque la touche commence
                    break;

                case TouchPhase.Moved:
                    currentTouchPosition = touch.position;
                    if (isSwiping)
                    {
                        CheckSwipe();
                    }
                    if (isSliding)
                    {
                        CheckSlide();
                    }
                    break;

                case TouchPhase.Ended:
                    if (isSwiping)
                    {
                        isSwiping = false;
                    }
                    if (isSliding)
                    {
                        isSliding = false;
                    }
                    break;
            }
        }

        // Déplacer l'objet vers la position cible en conservant la position Y actuelle
        transform.position = new Vector3(
            Mathf.MoveTowards(transform.position.x, targetPosition.x, slideSpeed * Time.deltaTime),
            transform.position.y,
            Mathf.MoveTowards(transform.position.z, targetPosition.z, slideSpeed * Time.deltaTime)
        );
    }

    public static float Clamp0360(float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }
    
    void CheckSwipe()
    {
        if (currentTouchPosition.y < startTouchPosition.y - 350) // Threshold a ajuster
        {
//            Debug.Log("Swipe Down Detected");
            SwipeDownMethod();
            isSwiping = false;
            isSliding = false;
        }
        else
        {
            if (cubeSpawner.lastCube != null)
            {
                cubeSpawner.lastCube.GoDown = false;
            }
            isSliding = true;
        }
    }

    void CheckSlide()
    {
        float slideEndX = currentTouchPosition.x;
        float slideDistance = Mathf.Abs(slideEndX - slideStartX);

        if (slideDistance >= minSlideThreshold)
        {
            // Utilisez la position X du doigt pour déterminer la position Z de l'objet
            float newZPosition = Mathf.Round(slideEndX / Screen.width * (maxSlideDistance - minSlideDistance) + minSlideDistance);

            // Utilisez la valeur normalisée pour déplacer l'objet
            SlideMethod(newZPosition);
        }
    }

    void CheckDoubleTap(Touch touch)
    {
        float currentTime = Time.time;
        if (currentTime - lastTapTime < tapTimeWindow)
        {
            tapCount++;
            if (tapCount == 2)
            {
                Debug.Log("Double Tap Detected");
                TapMethod();
                tapCount = 0; // Réinitialiser le compteur de taps
            }
        }
        else
        {
            tapCount = 1; // Réinitialiser le compteur de taps si le temps écoulé est trop long
        }
        lastTapTime = currentTime;
    }

    void SwipeDownMethod()
    {
        cubeSpawner.lastCube.GoDown = true;
        //Debug.Log("Swipe Down Method Executed");
    }

    void SlideMethod(float newZPosition)
    {
        // Clamper le déplacement à des intervalles de 1
        newZPosition = Mathf.Round(newZPosition);

        // Limiter la position Z entre -7 et 7
        newZPosition = Mathf.Clamp(newZPosition, -7, 7);

        // Mettre à jour la position cible en conservant la position Y actuelle
        targetPosition = new Vector3(transform.position.x, transform.position.y, newZPosition);

        // Stocker la dernière position Z
        lastZPosition = newZPosition;
    }

    void TapMethod()
    {
        if (!cubeSpawner.lastCube.CompareTag("Solid"))
        {
            Transform cubeTransform = cubeSpawner.lastCube.transform;
            // Créer une rotation de 90 degrés autour de l'axe X
            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            cubeTransform.rotation *= rotation;
            Bounds bounds = cubeTransform.GetComponent<MeshRenderer>().bounds;
            float lengthZ = bounds.size.z;
            /*Vector3 currentRotation = cubeTransform.rotation.eulerAngles;
            if (currentRotation.x < 360) {
                currentRotation.x += 90;      
            }
            else
            {
                currentRotation.x = 0;
            }
            // Appliquer la rotation à l'objet
            cubeTransform.rotation = Quaternion.Euler(currentRotation);
            Bounds bounds = cubeTransform.GetComponent<MeshRenderer>().bounds;
            float lengthZ = bounds.size.z;*/
            //cubeSpawner.Indicator.transform.localScale = new Vector3(lengthZ, 200, lengthZ);
            //cubeSpawner.Indicator.transform.localPosition = new Vector3(cubeTransform.localPosition.x, 0, 0);
            cubeSpawner.UpdateIndicatorSize(lengthZ);
            cubeSpawner.ApplyOffset(cubeSpawner.lastCube.GetComponent<Bloc>());
        }
        Debug.Log("Tap Method Executed");
    }
}