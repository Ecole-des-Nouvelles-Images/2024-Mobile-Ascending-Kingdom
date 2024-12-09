using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Tempest", menuName = "Events/Tempest")]
    public class Tempest : EventSO
    {
        public GameObject tempestParticlePrefab; // Reference to the Tempest particle system prefab
        private GameObject tempestParticleSystemInstance; // The instantiated particle system object

        public override void TriggerEvent()
        {
            if (tempestParticlePrefab == null)
            {
                Debug.LogError("tempestParticlePrefab is not assigned in the GameManager!");
                return;
            }

            Debug.Log("Tempest Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.BlocksChanged = 0; // Reset the counter

            // Instantiate the Tempest particle system prefab
            tempestParticleSystemInstance = Instantiate(tempestParticlePrefab);
            tempestParticleSystemInstance.SetActive(true);

            // Ensure the Particle System is playing
            ParticleSystem ps = tempestParticleSystemInstance.GetComponent<ParticleSystem>();
            if (ps != null && !ps.isPlaying)
            {
                ps.Play();
            }

            GameManager.Instance.StartCoroutine(ApplyTempestEffect());
        }

        private IEnumerator ApplyTempestEffect()
        {
            // Wait until the BlocksChanged condition is met (when blocks are affected)
            yield return new WaitUntil(() => GameManager.Instance.BlocksChanged > 0);

            // Stop the tempest particle system when the event ends
            if (tempestParticleSystemInstance != null)
            {
                ParticleSystem ps = tempestParticleSystemInstance.GetComponent<ParticleSystem>();
                if (ps != null && ps.isPlaying)
                {
                    ps.Stop();
                }

                tempestParticleSystemInstance.SetActive(false);
                Destroy(tempestParticleSystemInstance); // Destroy the instantiated particle system
            }

            // Reset the event status
            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }
}
