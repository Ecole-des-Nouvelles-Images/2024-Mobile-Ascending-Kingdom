using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elias.Scripts.Event
{
    public class EventManager : MonoBehaviour
    {
        public List<EventSO> AvailableEvents;
        private List<EventSO> _currentEvents = new List<EventSO>();
        public UnityEngine.Camera Camera;

        private void Start()
        {
            // Initialize or load available events
        }

        public void TriggerRandomEvent()
        {
            if (AvailableEvents.Count == 0)
            {
                Debug.LogWarning("No events available to trigger.");
                return;
            }

            int randomIndex = Random.Range(0, AvailableEvents.Count);
            EventSO chosenEvent = AvailableEvents[randomIndex];
            TriggerEvent(chosenEvent);
        }

        public void TriggerEvent(EventSO eventSO)
        {
            if (eventSO == null)
            {
                Debug.LogWarning("EventSO is null.");
                return;
            }

            Debug.Log($"Triggering event: {eventSO.eventName}");
            eventSO.TriggerEvent();

            // Additional logic for handling the event, such as playing sounds or effects
            if (eventSO.eventSound != null)
            {
                AudioSource.PlayClipAtPoint(eventSO.eventSound, Camera.transform.position);
            }

            if (eventSO.eventEffectPrefab != null)
            {
                Instantiate(eventSO.eventEffectPrefab, Vector3.zero, Quaternion.identity);
            }

            // Block duration logic
            StartCoroutine(BlockDuration(eventSO.eventBlockDuration));
        }

        private IEnumerator BlockDuration(int duration)
        {
            Debug.Log($"Event block duration started for {duration} seconds.");
            yield return new WaitForSeconds(duration);
            Debug.Log("Event block duration ended.");
        }
    }
}