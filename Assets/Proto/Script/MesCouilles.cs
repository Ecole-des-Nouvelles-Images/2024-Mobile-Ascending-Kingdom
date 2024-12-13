using UnityEngine;

public class MesCouilles : MonoBehaviour
{
    void Update()
    {
        Debug.Log("transform : " + transform.eulerAngles);
        Debug.Log("rotation : " + transform.rotation.eulerAngles);
    }

    float NormalizeAngle(float angle)
    {
        return angle % 360;
    }
}