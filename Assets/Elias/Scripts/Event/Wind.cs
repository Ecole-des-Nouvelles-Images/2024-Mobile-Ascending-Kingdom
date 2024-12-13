using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Wind", menuName = "Events/Wind")]
    public class Wind : EventSO
    {
        public float duration = 5f; // Duration of the wind event
        public float horizontalForce = 0.05f; // Force applied horizontally
        public Vector3 forceDirection = Vector3.right; // Direction of the force

        public override void TriggerEvent()
        {
            Debug.Log("Wind Event Triggered!");
            GameManager.Instance.EventActive = true;
            ambiantParticleSystem = GameManager.Instance.WindPS;
            if (ambiantParticleSystem != null)
            {
                ambiantParticleSystem.gameObject.SetActive(true);
                ambiantParticleSystem.Play();
            }
            GameManager.Instance.StartCoroutine(ApplyWindEffect(duration));
        }

        private IEnumerator ApplyWindEffect(float duration)
        {
            float startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                foreach (Bloc bloc in GameManager.Instance.Blocs)
                {
                    Rigidbody rb = bloc.GetComponent<Rigidbody>();
                    rb.AddForce(forceDirection * horizontalForce, ForceMode.Acceleration);
                }
                yield return null;
            }

            if (ambiantParticleSystem != null)
            {
                ambiantParticleSystem.Stop();
                ambiantParticleSystem.gameObject.SetActive(false);
            }

            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold(); // Comment out this line to keep the threshold rising
        }
    }
}