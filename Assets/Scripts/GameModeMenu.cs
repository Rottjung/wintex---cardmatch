
using UnityEngine;

public class GameModeMenu : GameModeBase
{
    public override void Init(bool OnReady = true)
    {
        base.Init(OnReady);
        GameInstance.Instance.StartMainTheme();
    }

    public void NewGame()
    {
        GameInstance.Instance.CreateNewSavefile();
        Play();
    }

    private void Play()
    {
        LoadingManager.Instance.LoadByNameAsync("Game");
    }

    public void Continue()
    {
        if(GameInstance.Instance.LoadGame())
            Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
