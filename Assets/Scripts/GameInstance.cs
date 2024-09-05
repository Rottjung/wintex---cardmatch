

using System;
using UnityEngine;

public class GameInstance : Singleton<GameInstance>
{
    public string GameName;
    public SaveFile SaveFile => m_Savefile;
    private SaveFile m_Savefile;
    public bool HasLoadedSavefile => m_HasLoadedSavefile;
    private bool m_HasLoadedSavefile;
    public GameModeBase GameMode => m_GameMode;
    private GameModeBase m_GameMode;
    private AudioSource audioSource;

    public bool IsGameModeAvailable<T>() where T : GameModeBase
    {
        return (m_GameMode as T) != null;
    }

    private void LoadMainMenu()
    {
        LoadingManager.Instance.LoadSceneAsync(1); //Assuming 1 is MainMenu
    }

    public void LoadSavefile()
    {
        if (LoadGame())
        {
            m_HasLoadedSavefile = true;
            LoadMainMenu();
        }
        else
        {
            CreateNewSavefile();
            if (!SaveGame())
                Debug.Log("Something went wrong Creating a new Savefile");
            else
            {
                m_HasLoadedSavefile = true;
                LoadMainMenu();
            }
        }
    }

    public void CreateNewSavefile()
    {
        m_Savefile = new SaveFile();
        m_Savefile.savefileName = GameName;
        m_Savefile.data = new SaveData();
        m_Savefile.data.score = 0;
        m_Savefile.data.level = 0;
    }

    public bool SaveGame()
    {
        return SerializationManager<SaveFile>.Save(m_Savefile);
    }

    public bool LoadGame()
    {
        return SerializationManager<SaveFile>.Load(out m_Savefile);
    }

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (LoadingManager.IsInstanceAvailable())
            LoadingManager.Instance.OnLoadingComplete += Init;
        else
            Init();
    }

    private void Init()
    {
        if(m_Savefile == null)
            LoadSavefile();
        LoadGameMode();
        m_GameMode.Init();
    }

    public T GetGameMode<T>() where T : GameModeBase
    {
        if (!m_GameMode)
        {
            LoadGameMode();
            Debug.Assert(m_GameMode, "GameInstance: You should always provide the game mode for the gameInstance. " +
                "If you are getting this message it means that you might try to get the GameMode when exiting the scene.");
            return null;
        }
        return m_GameMode as T;
    }

    public void LoadGameMode()
    {
        m_GameMode = FindFirstObjectByType<GameModeBase>();
        if (!m_GameMode)
        {
            Debug.LogWarning("Scene doesn't has a GameMode, Spawning Default GameMode");
            GameObject go = new GameObject("GameModeBase");
            m_GameMode = go.AddComponent<GameModeBase>();
        }
    }
    internal void StartMainTheme()
    {
        audioSource.Play();
    }
    internal void StopMainTheme()
    {
        audioSource.Stop();
    }
}
