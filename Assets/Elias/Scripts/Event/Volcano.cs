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
            activeParticleSystem = GameManager.Instance.MeteorPS;
            if (ambiantParticleSystem != null)
            {
                ambiantParticleSystem.gameObject.SetActive(true);
                activeParticleSystem.gameObject.SetActive(true);
                ambiantParticleSystem.Play();
                activeParticleSystem.Play();
                
            }

            else
            {
                Debug.LogWarning("No PS_Ashes ParticleSystem found!");
            }

            GameManager.Instance.StartCoroutine(ApplyVolcanoEffect());
        }

        private IEnumerator ApplyVolcanoEffect()
        {
            yield return new WaitUntil(() => GameManager.Instance.BlocksDestroyed >= 5);

            if (ambiantParticleSystem != null && activeParticleSystem != null)
            {
                ambiantParticleSystem.Stop();
                activeParticleSystem.Stop();
                ambiantParticleSystem.gameObject.SetActive(false);
                activeParticleSystem.gameObject.SetActive(false);
            }

            GameManager.Instance.EventActive = false;
            // GameManager.Instance.ResetEventThreshold(); // Comment out this line to keep the threshold rising
        }
    }
}