using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null) { instance = FindFirstObjectByType<T>(); }
            DontDestroyOnLoad(instance);

            return instance;
        }
    }

}
