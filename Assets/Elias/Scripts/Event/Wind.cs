using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{

    [CreateAssetMenu(fileName = "Wind", menuName = "Events/Wind")]
    public class Wind : EventSO
    {
        public float duration = 5f; // Duration of the wind event
        public float gravityScale = 2f;

        public override void TriggerEvent()
        {
            Debug.Log("Wind Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.StartCoroutine(ApplyWindEffect(duration));
        }

        private IEnumerator ApplyWindEffect(float duration)
        {
            float startTime = Time.time;
            float generalY = GetGeneralY();

            while (Time.time < startTime + duration)
            {
                foreach (Bloc bloc in GameManager.Instance.Blocs)
                {
                    if (bloc.transform.position.y > generalY)
                    {
                        Rigidbody rb = bloc.GetComponent<Rigidbody>();
                        rb.mass = 1f;
                        rb.drag = 0f;
                        Vector3 customGravity = Physics.gravity * gravityScale;
                        rb.AddForce(customGravity, ForceMode.Acceleration);
                    }
                }
                yield return null;
            }

            // Revert the values
            foreach (Bloc bloc in GameManager.Instance.Blocs)
            {
                Rigidbody rb = bloc.GetComponent<Rigidbody>();
                rb.mass = 1f; // Revert to original mass
                rb.drag = 0.1f; // Revert to original drag
            }

            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }

        private float GetGeneralY()
        {
            float maxHeight = 0;
            foreach (Bloc bloc in GameManager.Instance.Blocs)
            {
                if (bloc.transform.position.y > maxHeight)
                {
                    maxHeight = bloc.transform.position.y;
                }
            }
            return maxHeight;
        }
    }
}