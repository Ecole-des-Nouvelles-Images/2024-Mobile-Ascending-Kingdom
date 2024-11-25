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

    void Start()
    {
        // Initialiser lastZPosition à 0 au début
        lastZPosition = 0;
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
                    CheckTap(touch);
                    break;
            }
        }
    }

    void CheckSwipe()
    {
        if (currentTouchPosition.y < startTouchPosition.y - 300) // Threshold a ajuster
        {
            Debug.Log("Swipe Down Detected");
            SwipeDownMethod();
            isSwiping = false;
        }
        else
        {
            if (cubeSpawner.lastCube != null)
            {
                cubeSpawner.lastCube.goDown = false;
            }
        }
    }

    void CheckSlide()
    {
        float slideEndX = currentTouchPosition.x;
        float slideDistance = slideEndX - slideStartX;
        Debug.Log("Slide Distance: " + slideDistance);

        // Définissez la distance maximale de glissement
        float maxSlideDistance = 100.0f; // Vous pouvez ajuster cette valeur selon vos besoins

        // Appliquez le facteur de sensibilité à la distance de glissement
        float adjustedSlideDistance = slideDistance * sensitivityFactor;

        // Normalisez la distance de glissement ajustée
        float normalizedSlideValue = Mathf.Clamp(adjustedSlideDistance / maxSlideDistance, -1, 1);

        // Utilisez la valeur normalisée pour déplacer l'objet
        SlideMethod(normalizedSlideValue);
    }

    void CheckTap(Touch touch)
    {
        if (touch.tapCount == 1)
        {
            Debug.Log("Tap Detected");
            TapMethod();
        }
    }

    void SwipeDownMethod()
    {
        cubeSpawner.lastCube.goDown = true;
        Debug.Log("Swipe Down Method Executed");
    }

    void SlideMethod(float normalizedSlideValue)
    {
        // Calculer la nouvelle position Z en fonction de la valeur normalisée
        float newZPosition = lastZPosition + normalizedSlideValue * slideSpeed * Time.deltaTime;

        // Clamper le déplacement à des intervalles de 0.5
        newZPosition = Mathf.Round(newZPosition * 2) / 2;

        // Limiter la position Z entre -7 et 7
        newZPosition = Mathf.Clamp(newZPosition, -7, 7);

        // Mettre à jour la position de l'objet
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        // Stocker la dernière position Z
        lastZPosition = newZPosition;
    }

    void TapMethod()
    {
        // TAP METHOD
        Debug.Log("Tap Method Executed");
    }
}
