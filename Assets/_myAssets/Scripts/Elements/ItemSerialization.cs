using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemSerialization : MonoBehaviour
{

    public ChatArchive userArchive;

    private string jsonFileName = "ChatArchive.txt";

    private bool shouldUpdate = false;

    private void Update()
    {
        if(shouldUpdate)
        {
            WriteFile();
            shouldUpdate = false;
        }
    }

    public void AddChatEntry(string clientID, string message, ChatType ct)
    {
        if (!userArchive.userChatList.Any(x => x.ID.Equals(clientID)))
        {
            userArchive.userChatList.Add(new UserChat(clientID, new List<TextMessage>()));
        }
        userArchive.userChatList.First(x => x.ID.Equals(clientID)).textMessageList.Add(new TextMessage(DateTime.Now.ToString("hh:mm"), message, ct));
        shouldUpdate = true;
    }

    public List<TextMessage> GetChatListForUser(string clientID)
    {
        if (!userArchive.userChatList.Any(x => x.ID.Equals(clientID)))
            return new List<TextMessage>();

        return userArchive.userChatList.First(x => x.ID.Equals(clientID)).textMessageList;
    }

    private void Start()
    {
        ReadFile();
    }

    public void ReadFile()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, jsonFileName)))
        {
            string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, jsonFileName));
            userArchive = JsonUtility.FromJson<ChatArchive>(json);
        }
    }

    public void WriteFile()
    {
        string json = JsonUtility.ToJson(userArchive);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, jsonFileName), json);
    }
}

[System.Serializable]
public class ChatArchive
{
    public List<UserChat> userChatList = new List<UserChat>();
}

[System.Serializable]
public class UserChat
{
    public string ID;
    public List<TextMessage> textMessageList = new List<TextMessage>();

    public UserChat(string clientID, List<TextMessage> textList)
    {
        ID = clientID;
        textMessageList = textList;
    }
}

[System.Serializable]
public class TextMessage
{
    public string time;
    public string content;
    public ChatType type;
    public TextMessage(string t, string c, ChatType ct)
    {
        time = t;
        content = c;
        type = ct;
    }
}
