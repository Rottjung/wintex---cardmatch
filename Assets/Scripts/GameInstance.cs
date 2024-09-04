

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
        if (SerializationManager<SaveFile>.Load(out m_Savefile))
        {
            m_HasLoadedSavefile = true;
            LoadMainMenu();
        }
        else
            CreateNewSavefile();
    }
   

    public void CreateNewSavefile()
    {
        m_Savefile = new SaveFile();
        m_Savefile.savefileName = GameName;

        if (!SerializationManager<SaveFile>.Save(m_Savefile))
            Debug.Log("Something went wrong Creating a new Savefile");
        else
            LoadMainMenu();
    }

    public void SaveGame()
    {
        SerializationManager<SaveFile>.Save(m_Savefile);
    }

    protected void Start()
    {
        if(LoadingManager.IsInstanceAvailable())
            LoadingManager.Instance.OnLoadingComplete += Init;
        else
            Init();
    }

    private void Init()
    {
        if(LoadingManager.IsInstanceAvailable())
            LoadingManager.Instance.OnLoadingComplete -= Init;
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
}
