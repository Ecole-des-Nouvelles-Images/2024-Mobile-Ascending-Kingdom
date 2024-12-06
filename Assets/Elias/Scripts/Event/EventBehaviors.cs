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