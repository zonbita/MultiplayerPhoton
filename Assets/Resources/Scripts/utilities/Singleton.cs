using Photon.Pun;
using UnityEngine;

public class Singleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                    instance.gameObject.name = instance.GetType().Name;
                }
            }
            return instance;
        }
    }

    public void Reset()
    {
        instance = null;
    }

    public static bool Exists()
    {
        return (instance != null);
    }
}