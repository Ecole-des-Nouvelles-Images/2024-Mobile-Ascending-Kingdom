using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "BlizzardEvent", menuName = "Events/Blizzard Event")]
    public class WindEventSO : EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Blizzard Event Triggered!");
        }
    }

    [CreateAssetMenu(fileName = "VolcanoEvent", menuName = "Events/Volcano Event")]
    public class VolcanoEventSO : EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Volcano Event Triggered!");
        }
    }

    [CreateAssetMenu(fileName = "WaveEvent", menuName = "Events/Wave Event")]
    public class WaveEventSO : EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Wave Event Triggered!");
        }
    }
}