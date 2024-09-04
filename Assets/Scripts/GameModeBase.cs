using UnityEngine;
using System;

public class GameModeBase : MonoBehaviour
{
    public static T Get<T>() where T : GameModeBase => GameInstance.Instance.GetGameMode<T>();

    public static bool IsGameModeAvailable<T>() where T : GameModeBase
    {
        if (!GameInstance.IsInstanceAvailable())
            return false;

        return GameInstance.Instance.IsGameModeAvailable<T>();
    }

    #region GameModeReady

    protected bool isGameModeReady;
    public bool IsGameModeReady => isGameModeReady;
    
    public void ResetGameMode()
    {
        isGameModeReady = false;
    }

    private void Awake()
    {
        isGameModeReady = false;
    }
    
    public void OnGameModeReady(Action callback)
    {
        if (callback == null)
        {
            return;
        }

        if (isGameModeReady)
        {
            callback.Invoke();
            return;
        }

        onGameModeReady += callback;
    }

    protected event Action onGameModeReady;

    protected void InvokeGameModeReady()
    {
        isGameModeReady = true;
        onGameModeReady?.Invoke();
        onGameModeReady = null;
    }

    #endregion
    
    public virtual void Init(bool OnReady = true)
    {
        if(OnReady)
        {
            InvokeGameModeReady();
        }
    }

    protected virtual void OnDestroy()
    {
        isGameModeReady = false;
    }
}
