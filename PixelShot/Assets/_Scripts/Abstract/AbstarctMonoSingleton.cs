using UnityEngine;

public abstract class AbstarctMonoSingleton<T> : MonoBehaviour where T : Component
{
    
    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    protected void Awake()
    {
        if (_instance == null) _instance = this as T;
        else if (_instance != this) Destroy(this);
    }
    


}

