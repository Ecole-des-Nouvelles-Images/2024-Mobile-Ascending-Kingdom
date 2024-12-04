using UnityEngine;

namespace Elias.Scripts.Event
{
    [CreateAssetMenu(fileName = "WindEvent", menuName = "Events/Wind Event")]
    public class Wind : EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Blizzard Event Triggered!");
        }
    }

    [CreateAssetMenu(fileName = "VolcanoEvent", menuName = "Events/Volcano Event")]
    public class Volcano: EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Volcano Event Triggered!");
        }
    }

    [CreateAssetMenu(fileName = "WaveEvent", menuName = "Events/Wave Event")]
    public class Blizzard : EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Wave Event Triggered!");
        }
    }
    
    public class Wave : EventSO
    {
        public override void TriggerEvent()
        {
            Debug.Log("Wave Event Triggered!");
        }
    }
}