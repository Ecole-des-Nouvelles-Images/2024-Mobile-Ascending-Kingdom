using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "NewEvent", menuName = "Events/New Event")]
    public class EventSO : ScriptableObject
    {
        public string eventName;
        public Sprite eventIcon;
        public AudioClip eventSound;
        public GameObject eventEffectPrefab;
        public int eventBlockDuration;

        public virtual void TriggerEvent()
        {
            // event logic
        }
    }
}