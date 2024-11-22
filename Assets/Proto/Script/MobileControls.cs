using UnityEngine;

public class MobileControls : MonoBehaviour
{
    private bool isSwiping = false;
    private bool isSliding = false;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private float slideStartX;
    private float slideEndX;

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
        if (currentTouchPosition.y < startTouchPosition.y - 50) // Threshold a ajuster
        {
            Debug.Log("Swipe Down Detected");
            SwipeDownMethod();
            isSwiping = false;
        }
    }

    void CheckSlide()
    {
        slideEndX = currentTouchPosition.x;
        float slideDistance = slideEndX - slideStartX;
        Debug.Log("Slide Distance: " + slideDistance);
        SlideMethod(slideDistance);
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
        // SWIPE METHOD
        Debug.Log("Swipe Down Method Executed");
    }

    void SlideMethod(float distance)
    {
        // SLIDE METHOD
        Debug.Log("Slide Method Executed with distance: " + distance);
    }

    void TapMethod()
    {
        // TAP METHOD
        Debug.Log("Tap Method Executed");
    }
}
