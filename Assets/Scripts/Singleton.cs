using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static bool _isShuttingDown = false;
    static object _lock = new object();
    static T _instance;

    public static T Instance
    {
        get
        {
            if (_isShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                   "' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // Search for existing instance.
                    _instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (_instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }
            }

            return _instance;
        }
    }

    public void CreateInstance()
    {
        Debug.Log($"Instance of {typeof(T)} created.");
    }

    void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }


    void OnDestroy()
    {
        _isShuttingDown = true;
    }
}