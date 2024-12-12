using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Volcano", menuName = "Events/Volcano")]
    public class Volcano : EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Volcano Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.BlocksDestroyed = 0; // Reset the counter

            ambiantParticleSystem = GameManager.Instance.VolcanoPS;
            if (ambiantParticleSystem != null)
            {
                ambiantParticleSystem.gameObject.SetActive(true);
                ambiantParticleSystem.Play();
            }
            else
            {
                Debug.LogWarning("No PS_Ashes ParticleSystem found!");
            }

            // Activate the MeteorSpawner
            MeterorSpawning meteorSpawner = GameManager.Instance.MeterorSpawningObj;
            if (meteorSpawner != null)
            {
                meteorSpawner.gameObject.SetActive(true);
                meteorSpawner.StartSpawning();
            }
            else
            {
                Debug.LogWarning("No MeteorSpawner found!");
            }

            GameManager.Instance.StartCoroutine(ApplyVolcanoEffect());
        }

        private IEnumerator ApplyVolcanoEffect()
        {
            yield return new WaitUntil(() => GameManager.Instance.BlocksDestroyed >= 5);

            if (ambiantParticleSystem != null)
            {
                ambiantParticleSystem.Stop();
                ambiantParticleSystem.gameObject.SetActive(false);
            }

            MeterorSpawning meteorSpawner = GameManager.Instance.MeterorSpawningObj;
            if (meteorSpawner != null)
            {
                meteorSpawner.StopSpawning();
                meteorSpawner.gameObject.SetActive(false);
            }

            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }
}
