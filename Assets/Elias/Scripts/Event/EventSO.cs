using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "NewEvent", menuName = "Events/New Event")]
    public class EventSO : ScriptableObject
    {
        public AudioClip eventSound;
        public ParticleSystem particleSystem;
        public Shader shader;
        public int eventBlockDuration;
        public bool EventActive = false; // Add this line

        public virtual void TriggerEvent()
        {
            // event logic
        }
    }
}