using UnityEngine;

/// <summary>
///  A static instance is similar to singleton, but instead of destroying any new instances,
///  it overrides the current instance. This is handy for resetting the state and saves
///  manual implementation.
/// </summary>
/// <typeparam name="T"></typeparam>

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }

    
}

/// <summary>
/// This transforms the static instance into a basic singleton. This will destroy any new
/// instance, leaving the original instance intact.
/// </summary>
/// <typeparam name="T"></typeparam>
/// 
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        base.Awake();
    }
}

/// <summary>
/// Finally we have persitent version of singleton. This will survive through scene loads.
/// Perfect for having system classes which requires stateful, persistent data. Our audio sources
/// where music plays through loading screens, etc.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}