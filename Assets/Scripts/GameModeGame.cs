using System;
using TMPro;
using UnityEngine;

public class GameModeGame : GameModeBase
{
    public TMP_Text score;
    public MatchManager matchManager;
    public DB_Cards cardsDB;
    public GameObject cardPrefab;
    public GameObject finishedLevelScreen;
    private int currentScore;
    private int currentLevel;

    public override void Init(bool OnReady = true)
    {
        base.Init(OnReady);
        matchManager = GetComponent<MatchManager>();
        currentScore = GameInstance.Instance.SaveFile.data.score;
        SetScore();
        currentLevel = GameInstance.Instance.SaveFile.data.level;
        matchManager.GenerateCardGrid(currentLevel);
        GameInstance.Instance.StopMainTheme();
    }

    internal void AddScore()
    {
        currentScore += 10;
        SetScore();
        GameInstance.Instance.SaveFile.data.score = currentScore;
        GameInstance.Instance.SaveGame();
    }

    internal void FinishLevel()
    {
        finishedLevelScreen.SetActive(true);
    }

    public void NextLevel()
    {
        if (currentLevel < cardsDB.dictionary.Count)
            currentLevel++;
        else
            currentLevel = 0; //reset for now so we can keep playing
        matchManager.GenerateCardGrid(currentLevel);
    }

    public void BackToMain()
    {
        LoadingManager.Instance.LoadByNameAsync("MainMenu");
    }

    private void SetScore()
    {
        score.text = currentScore.ToString();
    }
}
