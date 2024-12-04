using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "NewEvent", menuName = "Events/New Event")]
    public class EventSO : ScriptableObject
    {
        public AudioClip eventSound;
        //public GameObject eventEffectPrefab; SHADER ???
        public int eventBlockDuration;

        public virtual void TriggerEvent()
        {
            // event logic
        }
    }
}