using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    #region Fields

    /// <summary>
    /// The instance.
    /// </summary>
    private static T _instance;
    private static string _name;

    public bool isDontDestory;
    public static bool dontDestory;

    private static int _count;
    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null && dontDestory)
                {
                    Debug.LogError("No "+_name+" found, creating a new one!");
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
                if (_instance)
                {
                    if (Application.isPlaying && dontDestory)
                        DontDestroyOnLoad(_instance.gameObject);
                    _instance.name += "_" + _count++;
                }
            }
            return _instance;
        }
    }

    public static bool IsInstanceAvailable()
    {
        return _instance != null;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
        dontDestory = isDontDestory;
        _name = gameObject.name;
        if (_instance == null)
        {
            _instance = this as T;
            _instance.name += "_" + _count++;
            if(Application.isPlaying && isDontDestory)
                DontDestroyOnLoad(gameObject);
        }
        else if(_instance is T && _instance != this)
        {
#if !UNITY_EDITOR
            DestroyImmediate(gameObject);
#endif
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
    #endregion

}