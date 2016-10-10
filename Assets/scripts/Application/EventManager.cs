using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// EventManager class from the official Unity web page. Minor modifications where made to be able to send data with a command in the form of an object.
/// </summary>
public class EventManager : MonoBehaviour {

    private Dictionary<string, GameEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance {
        get {
            if (!eventManager) {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager) 
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                else 
                    eventManager.Init();
            }
            return eventManager;
        }
    }

    void Start() {
        EventManager callInit = instance;
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Init() {
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<string, GameEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction<object> listener) {
        GameEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        }
        else {
            thisEvent = new GameEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<object> listener) {
        if (eventManager == null) return;
        GameEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName) {
        TriggerEvent(eventName, null);
    }

    public static void TriggerEvent(string eventName, object arguments) {
        GameEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke(arguments);
        }
    }
}
