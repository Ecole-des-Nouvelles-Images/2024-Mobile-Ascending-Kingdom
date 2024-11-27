using UnityEngine;

namespace Elias.Scripts.Camera
{
    public class CameraHeightCheck : MonoBehaviour
    {
        public Collider LowerCollider;
        public bool TowerLowerDetection;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Solid"))
            {
                TowerLowerDetection = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Solid"))
            {
                TowerLowerDetection = false;
            }
        }
    }
}