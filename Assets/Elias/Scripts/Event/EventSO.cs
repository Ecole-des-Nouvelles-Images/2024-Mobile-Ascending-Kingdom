using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "NewEvent", menuName = "Events/New Event")]
    public class EventSO : ScriptableObject
    {
        public string eventName;
        public AudioClip eventSound;
        public ParticleSystem particleSystem;
        public Shader shader;
        public int eventBlockDuration;
         // Add this line

        public virtual void TriggerEvent()
        {
            // event logic
        }
    }
}