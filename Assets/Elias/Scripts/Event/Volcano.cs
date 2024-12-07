using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Volcano", menuName = "Events/Volcano")]
    public class Volcano : EventSO
    {
        public ParticleSystem volcanoParticleSystem;

        public override void TriggerEvent()
        {
            Debug.Log("Volcano Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.BlocksDestroyed = 0; // Reset the counter
            GameManager.Instance.StartCoroutine(ApplyVolcanoEffect());
            volcanoParticleSystem.gameObject.SetActive(true);
        }

        private IEnumerator ApplyVolcanoEffect()
        {
            yield return new WaitUntil(() => GameManager.Instance.BlocksDestroyed >= 5);

            volcanoParticleSystem.gameObject.SetActive(false);

            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }
}