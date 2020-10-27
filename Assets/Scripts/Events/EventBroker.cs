using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBroker : MonoBehaviour
{
    Dictionary<string, UnityEvent> eventDictionary;

    static EventBroker eventBroker;

    void Init()
    {
        if (eventDictionary == null )
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static EventBroker instance
    {
        get
        {
            if (!eventBroker)
            {
                eventBroker = FindObjectOfType(typeof(EventBroker)) as EventBroker;

                if (!eventBroker)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventBroker.Init();
                }
            }

            return eventBroker;
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventBroker == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
