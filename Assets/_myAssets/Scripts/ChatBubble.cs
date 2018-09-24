using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{

    [SerializeField]
    private Text textMessage;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Image textBoxBG;
    [SerializeField]
    private Color mainColor;
    [SerializeField]
    private Color clientColor;
    public void Initialize(string text, string time, ChatType type)
    {
        textMessage.text = text;
        timeText.text = time;

        if (type == ChatType.client)
        {
            timeText.GetComponent<RectTransform>().anchorMin = new Vector2(0, .5f);
            timeText.GetComponent<RectTransform>().anchorMax = new Vector2(0, .5f);
            timeText.GetComponent<RectTransform>().pivot = new Vector2(0, .5f);
            textBoxBG.color = clientColor;
        }
        else
        {
            timeText.GetComponent<RectTransform>().anchorMin = new Vector2(1, .5f);
            timeText.GetComponent<RectTransform>().anchorMax = new Vector2(1, .5f);
            timeText.GetComponent<RectTransform>().pivot = new Vector2(1, .5f);
            textBoxBG.color = mainColor;
        }
    }

}
    public enum ChatType
    {
        main,
        client
    }
