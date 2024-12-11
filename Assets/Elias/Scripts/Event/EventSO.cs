using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "NewEvent", menuName = "Events/New Event")]
    public class EventSO : ScriptableObject
    {
        public string eventName;
        public Sprite eventBackground;
        public AudioClip eventSound;
        public Shader shader;
        public int eventBlockDuration;

        protected ParticleSystem ambiantParticleSystem;
        protected ParticleSystem activeParticleSystem;

        public virtual void TriggerEvent()
        {
            // event logic
        }
    }
}