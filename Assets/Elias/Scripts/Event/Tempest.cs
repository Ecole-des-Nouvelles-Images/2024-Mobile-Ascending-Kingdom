using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Tempest", menuName = "Events/Tempest")]
    public class Tempest : EventSO
    {
        
        public ParticleSystem tempestParticleSystem;
        public override void TriggerEvent()
        {
            Debug.Log("Tempest Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.BlocksChanged = 0;
            GameManager.Instance.StartCoroutine(ApplyTempestEffect());
            tempestParticleSystem.gameObject.SetActive(true);
        }

        private IEnumerator ApplyTempestEffect()
        {
            yield return new WaitUntil(() => GameManager.Instance.BlocksChanged > 0);

            tempestParticleSystem.gameObject.SetActive(false);

            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }
}