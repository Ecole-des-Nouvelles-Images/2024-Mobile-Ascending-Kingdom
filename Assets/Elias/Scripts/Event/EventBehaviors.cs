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
            EventActive = true;
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

            EventActive = false;
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
            EventActive = true;
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
            EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }
    }



    [CreateAssetMenu(fileName = "VolcanoEvent", menuName = "Events/Volcano")]
    public class Volcano : EventSO
    {
        public ParticleSystem volcanoParticleSystem;
        private int blocksDestroyed = 0;
        private const int maxBlocksToDestroy = 5;

        public override void TriggerEvent()
        {
            Debug.Log("Volcano Event Triggered!");
            EventActive = true;
            GameManager.Instance.StartCoroutine(ApplyVolcanoEffect());
        }

        private IEnumerator ApplyVolcanoEffect()
        {
            // Activate the particle system
            volcanoParticleSystem.gameObject.SetActive(true);

            // Enable collision detection
            ParticleSystem.CollisionModule collisionModule = volcanoParticleSystem.collision;
            collisionModule.enabled = true;
            collisionModule.sendCollisionMessages = true;

            // Wait until the event ends
            yield return new WaitUntil(() => blocksDestroyed >= maxBlocksToDestroy);

            // Deactivate the particle system
            volcanoParticleSystem.gameObject.SetActive(false);

            // End the event
            EventActive = false;
            GameManager.Instance.ResetEventThreshold();
        }

        private void OnParticleCollision(GameObject other)
        {
            Bloc bloc = other.GetComponent<Bloc>();
            if (bloc != null)
            {
                GameManager.Instance.RemoveBloc(bloc);
                Object.Destroy(bloc.gameObject);
                blocksDestroyed++;
                Debug.Log("Block destroyed by Volcano Event. Total destroyed: " + blocksDestroyed);
            }
        }
    }
    
    
    [CreateAssetMenu(fileName = "TempestEvent", menuName = "Events/Tempest")]
    public class Tempest: EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Tempest Event Triggered!");
        }
    }
}