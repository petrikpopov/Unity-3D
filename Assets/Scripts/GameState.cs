using System;
using System.Collections.Generic;

public class GameState
{
    public static bool isDay { get; set; }
    public static bool isFpv { get; set; }
    public static float lookSensitivityX { get; set; } = 0.05f;
    public static float lookSensitivityY { get; set; } = -0.025f;
    public static float fpvRange { get; set; } = 1f;

    public static int room { get; set; } = 1; 
    public static Dictionary<String, object> collectedItems { get; private set; } = new();

    public static float cameraMinTilt { get; set; } = -30f; 
    public static float cameraMaxTilt { get; set; } = 60f;  

    #region effectsVolume
    private static float _effectsVolume = 1f;
    public static float effectsVolume
    {
        get => _effectsVolume;
        set
        {
            if (_effectsVolume != value)
            {
                _effectsVolume = value;
                NotifyListeners(nameof(effectsVolume));
            }
        }
    }
    #endregion

    #region ambientVolume
    private static float _ambientVolume = 1f;
    public static float ambientVolume
    {
        get => _ambientVolume;
        set
        {
            if (_ambientVolume != value)
            {
                _ambientVolume = value;
                NotifyListeners(nameof(ambientVolume));
            }
        }
    }
    #endregion

    #region isSoundsMuted (muteAll)
    private static bool _isSoundsMuted = false;
    public static bool isSoundsMuted
    {
        get => _isSoundsMuted;
        set
        {
            if (_isSoundsMuted != value)
            {
                _isSoundsMuted = value;
                NotifyListeners(nameof(isSoundsMuted));
            }
        }
    }
    #endregion

    #region Change Notifier
    private static Dictionary<String, List<Action<string>>> changeListeners = new();
    public static void AddChangeListener(Action<string> listener, params String[] names)
    {
        foreach (String name in names)
        {
            if (!changeListeners.ContainsKey(name))
            {
                changeListeners[name] = new List<Action<string>>();
            }
            changeListeners[name].Add(listener);
            listener(name);
        }
    }
    public static void RemoveChangeListener(Action<string> listener, params String[] names)
    {
        foreach (String name in names)
        {
            if (changeListeners.ContainsKey(name))
            {
                changeListeners[name].Remove(listener);
            }
        }
    }
    private static void NotifyListeners(String name)
    {
        if (changeListeners.ContainsKey(name))
        {
            foreach (var action in changeListeners[name])
            {
                action(name);
            }
        }
    }
    #endregion

    #region collectSubscribers
    private static List<Action<String>> collectSubscribers = new List<Action<String>>();
    public static void AddCollectListener(Action<String> subscriber)
    {
        collectSubscribers.Add(subscriber);
    }
    public static void RemoveCollectListener(Action<String> subscriber)
    {
        collectSubscribers.Remove(subscriber);
    }
    public static void Collect(String itemName)
    {
        collectSubscribers.ForEach(s => s(itemName));
    }
    #endregion

    #region eventSubscribers
    private const string broadcastKey = "Broadcast";
    private static Dictionary<String, List<Action<String, object>>> eventSubscribers = new();
    public static void AddEventListener(
        Action<String, object> subscriber,
        params string[] eventNames)
    {
        if (eventNames == null || eventNames.Length == 0)
        {
            eventNames = new string[1] { broadcastKey };
        }
        foreach (string eventName in eventNames)
        {
            if (eventSubscribers.ContainsKey(eventName))
            {
                eventSubscribers[eventName].Add(subscriber);
            }
            else
            {
                eventSubscribers[eventName] = new() { subscriber };
            }
        }
    }

    public static void RemoveEventListener(Action<String, object> subscriber, params string[] eventNames)
    {
        if (eventNames == null || eventNames.Length == 0)
        {
            eventNames = new string[1] { broadcastKey };
        }
        foreach (string eventName in eventNames)
        {
            if (eventSubscribers.ContainsKey(eventName))
            {
                eventSubscribers[eventName].Remove(subscriber);
            }
            else UnityEngine.Debug.LogError("RemoveEventListener: empty key - " + eventName);
        }
    }

    public static void TriggerEvent(String eventName, object data)
    {
        if (eventSubscribers.ContainsKey(eventName))
        {
            eventSubscribers[eventName].ForEach(s => s(eventName, data));
        }

        if (eventName != broadcastKey && eventSubscribers.ContainsKey(broadcastKey))
        {
            eventSubscribers[broadcastKey].ForEach(s => s(eventName, data));
        }
    }
    #endregion
}
