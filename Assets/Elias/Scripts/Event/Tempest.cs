using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Tempest", menuName = "Events/Tempest")]
    public class Tempest : EventSO
    {
        private ParticleSystem ambientParticleSystem1;
        private ParticleSystem ambientParticleSystem2;

        public override void TriggerEvent()
        {
            Debug.Log("Tempest Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.BlocksChanged = 0; // Reset the counter

            ambientParticleSystem1 = GameManager.Instance.RainPS;
            ambientParticleSystem2 = GameManager.Instance.TempestPS; // Assuming you have a second particle system reference

            activeParticleSystem = GameManager.Instance.LightningPS;
            
            if (ambientParticleSystem1 != null)
            {
                ambientParticleSystem1.gameObject.SetActive(true);
                ambientParticleSystem1.Play();
            }

            if (ambientParticleSystem2 != null)
            {
                ambientParticleSystem2.gameObject.SetActive(true);
                ambientParticleSystem2.Play();
            }
            if (activeParticleSystem != null)
            {
                activeParticleSystem.gameObject.SetActive(true);
                activeParticleSystem.Play();
            }

            GameManager.Instance.StartCoroutine(ApplyTempestEffect());
        }

        private IEnumerator ApplyTempestEffect()
        {
            yield return new WaitUntil(() => GameManager.Instance.BlocksChanged > 10);

            if (ambientParticleSystem1 != null)
            {
                ambientParticleSystem1.Stop();
                ambientParticleSystem1.gameObject.SetActive(false);
            }

            if (ambientParticleSystem2 != null)
            {
                ambientParticleSystem2.Stop();
                ambientParticleSystem2.gameObject.SetActive(false);
            }

            if (activeParticleSystem != null)
            {
                activeParticleSystem.Stop();
                activeParticleSystem.gameObject.SetActive(false);
            }

            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }
}