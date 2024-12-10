using System.Collections;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "Blizzard", menuName = "Events/Blizzard")]
    public class Blizzard : EventSO
    {
        private int blizzardCount = 5;
        private float originalMass = 1f;
        private float originalDrag = 0.1f;

        public override void TriggerEvent()
        {
            Debug.Log("Blizzard Event Triggered!");
            GameManager.Instance.EventActive = true;
            GameManager.Instance.StartCoroutine(ApplyBlizzardEffect());
        }

        private IEnumerator ApplyBlizzardEffect()
        {
            while (blizzardCount > 0)
            {
                yield return new WaitUntil(() => GameManager.Instance.Blocs.Count > 0);

                Bloc lastBloc = GameManager.Instance.Blocs[^1];
                Rigidbody rb = lastBloc.GetComponent<Rigidbody>();
                rb.mass = 0.5f; // Lower mass
                rb.drag = 0.05f; // Lower drag

                blizzardCount--;
            }

            // Revert the values for future blocks
            GameManager.Instance.OnBlocListUpdated += RevertBlizzardEffect;
        }

        private void RevertBlizzardEffect()
        {
            if (GameManager.Instance.Blocs.Count > 0)
            {
                Bloc lastBloc = GameManager.Instance.Blocs[^1];
                Rigidbody rb = lastBloc.GetComponent<Rigidbody>();
                rb.mass = originalMass;
                rb.drag = originalDrag;
            }

            GameManager.Instance.OnBlocListUpdated -= RevertBlizzardEffect;
            GameManager.Instance.EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }
}