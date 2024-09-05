using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Collections;

public class MatchManager : MonoBehaviour
{
    public static bool isEvaluating;

    public AudioClip success, fail;
    private Card firstCard;
    private Card secondCard;
    private AudioSource audioSource;
    private int pairs;
    private int solved;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GenerateCardGrid(int level)
    {
        solved = 0;
        GameModeGame gmg = GameModeGame.Get<GameModeGame>();
        GridLayoutGroup grid = FindFirstObjectByType<GridLayoutGroup>();

        for(int i = grid.transform.childCount - 1; i >= 0; i--)
            Destroy(grid.transform.GetChild(i).gameObject);

        RectTransform gridRect = grid.GetComponent<RectTransform>();
        float gridWidth = gridRect.rect.width;
        float gridHeight = gridRect.rect.height;

        if (gridHeight > gridWidth)
            grid.startAxis = GridLayoutGroup.Axis.Vertical;
        else
            grid.startAxis= GridLayoutGroup.Axis.Horizontal;

        int columns = Mathf.FloorToInt(gridWidth / 300);
        int rows = Mathf.FloorToInt(gridHeight / 300);

        rows -= rows % 2 != 0 ? 1 : 0;

        int maxCards = Mathf.Min(columns * rows, 32);
        pairs = maxCards / 2;

        CardsDBData cardsData = gmg.cardsDB.dictionary[level];

        List<Sprite> selectedSprites = new List<Sprite>();
        int amount = cardsData.sprites.Length;

        for (int i = 0; i <pairs; i++)
        {
            selectedSprites.Add(cardsData.sprites[i % amount]);
            selectedSprites.Add(cardsData.sprites[i % amount]);
        }

        selectedSprites = selectedSprites.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < selectedSprites.Count; i++)
        {
            GameObject newCard = Instantiate(gmg.cardPrefab, grid.transform);
            newCard.GetComponent<Card>().SetCardSprite(selectedSprites[i]);
        }
    }

    internal void CheckMatch(Card card)
    {
        if(firstCard == null)
        {
            firstCard = card;
            return;
        }
        else if(secondCard == null) 
            secondCard = card;

        if(firstCard != null && secondCard != null)
        {
            isEvaluating = true;
            if(firstCard.Image.sprite ==  secondCard.Image.sprite) 
                StartCoroutine(SolveCards());
            else
                StartCoroutine(FlipCards());
        }
    }

    private IEnumerator SolveCards()
    {
        audioSource.clip = success;
        audioSource.Play();
        yield return new WaitForSeconds(.5f);
        isEvaluating = false;
        firstCard.Disable();
        secondCard.Disable();
        firstCard = null;
        secondCard = null;
        GameModeGame.Get<GameModeGame>().AddScore();
        solved++;
        if (solved == pairs)
            GameModeBase.Get<GameModeGame>().FinishLevel();
    }

    private IEnumerator FlipCards()
    {
        audioSource.clip = fail;
        audioSource.Play();
        yield return new WaitForSeconds(.5f);
        isEvaluating = false;
        firstCard.FlipCard();
        secondCard.FlipCard();
        firstCard = null;
        secondCard = null;
    }
}
