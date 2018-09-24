using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSpace : MonoBehaviour {

    [SerializeField]
    private GameObject chatBubble;

    public void PopulateChat(string message, string time, ChatType type)
    {
        GameObject temp = Instantiate(chatBubble, transform);
        temp.GetComponent<ChatBubble>().Initialize(message,time, type);
    }
}
