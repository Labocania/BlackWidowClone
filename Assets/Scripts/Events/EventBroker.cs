using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBroker : MonoBehaviour
{
    Dictionary<string, UnityEvent> eventDictionary;
    // Event with one integer argument dictionary.
    Dictionary<string, UnityEvent<int>> eventArgDictionary;

    static EventBroker eventBroker;

    void Init()
    {
        if (eventDictionary == null || eventArgDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
            eventArgDictionary = new Dictionary<string, UnityEvent<int>>();
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
                    Debug.LogError("There needs to be one active EventBroker script on a GameObject in your scene.");
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

    public static void StartListening(string eventName, UnityAction<int> listener)
    {
        UnityEvent<int> thisEvent = null;
        if (instance.eventArgDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<int>();
            thisEvent.AddListener(listener);
            instance.eventArgDictionary.Add(eventName, thisEvent);
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

    public static void StopListening(string eventName, UnityAction<int> listener)
    {
        if (eventBroker == null) return;
        UnityEvent<int> thisEvent = null;
        if (instance.eventArgDictionary.TryGetValue(eventName, out thisEvent))
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

    public static void TriggerEvent(string eventName, int argument)
    {
        UnityEvent<int> thisEvent = null;
        if (instance.eventArgDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(argument);
        }
    }
}


