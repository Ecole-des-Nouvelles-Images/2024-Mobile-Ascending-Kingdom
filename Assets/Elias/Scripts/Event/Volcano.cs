using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Volcano", menuName = "Events/Volcano")]
    public class Volcano : EventSO
    {
        public GameObject volcanoParticlePrefab; // Reference to the prefab

        private GameObject volcanoParticleSystemInstance; // Instance of the particle system

        public override void TriggerEvent()
        {
            if (volcanoParticlePrefab == null)
            {
                Debug.LogError("volcanoParticlePrefab is not assigned in the GameManager!");
                return;
            }

            Debug.Log("Volcano Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.BlocksDestroyed = 0; // Reset the counter

            volcanoParticleSystemInstance = Instantiate(volcanoParticlePrefab);
            volcanoParticleSystemInstance.SetActive(true);

            ParticleSystem ps = volcanoParticleSystemInstance.GetComponent<ParticleSystem>();
            if (ps != null && !ps.isPlaying)
            {
                ps.Play();
            }

            GameManager.Instance.StartCoroutine(ApplyVolcanoEffect());
        }

        private IEnumerator ApplyVolcanoEffect()
        {
            yield return new WaitUntil(() => GameManager.Instance.BlocksDestroyed >= 5);

            if (volcanoParticleSystemInstance != null)
            {
                ParticleSystem ps = volcanoParticleSystemInstance.GetComponent<ParticleSystem>();
                if (ps != null && ps.isPlaying)
                {
                    ps.Stop();
                }

                volcanoParticleSystemInstance.SetActive(false);
                Destroy(volcanoParticleSystemInstance); // Destroy the instantiated particle system
            }

            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }
}
