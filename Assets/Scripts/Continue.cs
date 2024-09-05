using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour
{
    void Start()
    {
        if(GameInstance.Instance.SaveFile.data.score <= 0)
        {
            GetComponent<Button>().interactable = false;
            TMP_Text text = GetComponentInChildren<TMP_Text>();
            Color color = text.color;
            text.color = new Color(color.r, color.g, color.b, 0.5f);
        }
    }
}
