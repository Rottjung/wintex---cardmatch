using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image Image => image; 
    private Image image;
    private Sprite front;
    private Sprite back;
    private Button button;
    private bool isFlipped = false;
    private AudioSource audioSource;

    private void Start()
    {
        image = GetComponent<Image>();
        back = image.sprite;
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
    }

    public void FlipCard()
    {
        if(!MatchManager.isEvaluating)
        {
            audioSource.Play();
            StartCoroutine(FlipAnimation());
        }
    }

    public bool IsFlipped()
    {
        return isFlipped;
    }

    public void SetCardSprite(Sprite sprite)
    {
        front = sprite;
    }

    IEnumerator FlipAnimation()
    {
        float flipDuration = 0.5f;
        float halfwayFlipTime = flipDuration / 2;

        for (float t = 0; t < halfwayFlipTime; t += Time.deltaTime)
        {
            float scale = Mathf.Lerp(1, 0, t / halfwayFlipTime);
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        isFlipped = !isFlipped;
        if (isFlipped)
            image.sprite = front; 
        else
            image.sprite = back; 

        for (float t = 0; t < halfwayFlipTime; t += Time.deltaTime)
        {
            float scale = Mathf.Lerp(0, 1, t / halfwayFlipTime); 
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }
        if(isFlipped)
            GameModeGame.Get<GameModeGame>().matchManager.CheckMatch(this);
    }

    internal void Disable()
    {
        image.color = new Color(1, 1, 1, 0);
        button.interactable = false;
    }
}
