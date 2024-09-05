using TMPro;
using UnityEngine;

public class GameModeGame : GameModeBase
{
    public TMP_Text score;
    public MatchManager matchManager;
    public DB_Cards cardsDB;
    public GameObject cardPrefab;
    private int currentScore;

    public override void Init(bool OnReady = true)
    {
        base.Init(OnReady);
        matchManager = GetComponent<MatchManager>();
        matchManager.GenerateCardGrid(0);
        if (GameInstance.Instance.SaveFile.data.score <= 0)
            currentScore = GameInstance.Instance.SaveFile.data.score;
        SetScore();
    }

    internal void AddScore()
    {
        currentScore += 10;
        SetScore();
        GameInstance.Instance.SaveFile.data.score = currentScore;
        GameInstance.Instance.SaveGame();
    }

    private void SetScore()
    {
        score.text = currentScore.ToString();
    }
}
