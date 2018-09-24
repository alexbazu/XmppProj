using Matrix.Xmpp.Roster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Matrix.Xmpp.MessageArchiving;
using Matrix.Xmpp.Client;
using System;

public class ChatPanel : MonoBehaviour
{

    [SerializeField]
    private Text username;
    [SerializeField]
    private InputField message;
    [SerializeField]
    private AppManager appManager;
    [SerializeField]
    private ChatSpace chatSpace;

    private XmppClient mainClient;
    private RosterItem chatClient;
    private bool shouldUpdate;
    private string receivedMessage;

    private void Update()
    {
        if (shouldUpdate)
        {
            chatSpace.PopulateChat(receivedMessage, DateTime.Now.ToString("hh:mm"),ChatType.client);
            shouldUpdate = false;
        }
    }

    public void InitializeChatPanel(RosterItem user)
    {
        ClearMessages();
        username.text = user.Jid.User;
        chatClient = user;
        mainClient = appManager.mainUser;
        mainClient.OnMessage += MainClient_OnMessage;
        //GetOldChat();
        LoadArchivedMessages();
    }

    private void ClearMessages()
    {
        foreach (Transform item in chatSpace.transform)
        {
            Destroy(item.gameObject);
        }
    }

    private void LoadArchivedMessages()
    {
        foreach (TextMessage item in appManager.itemSerialization.GetChatListForUser(chatClient.Jid))
        {
            chatSpace.PopulateChat(item.content, item.time, item.type);
        } 
    }

    private void GetOldChat()
    {
        var jid = chatClient.Jid;
        var list = new Matrix.Xmpp.MessageArchiving.List
        {
            With = jid
        };

        var iq = new Matrix.Xmpp.Client.Iq
        {
            Type = Matrix.Xmpp.IqType.Get,
            Query = list
        };
        iq.GenerateId();
        mainClient.IqFilter.SendIq(iq, GetChatCallback);
    }

    private void GetChatCallback(object sender, IqEventArgs e)
    {
        //if (e.Iq.Type == Matrix.Xmpp.IqType.Error)
        //    Debug.LogError(e.Iq.Error.Text);
    }



    private void MainClient_OnMessage(object sender, MessageEventArgs e)
    {

        if (e.Message.From.User == chatClient.Jid.User)
        {
            if (!string.IsNullOrEmpty(e.Message.Body))
            {
                receivedMessage = e.Message.Body;
                shouldUpdate = true;
            }
        }
    }

    public void SendMessage()
    {
        var msg = new Message
        {
            Type = Matrix.Xmpp.MessageType.Chat,
            To = chatClient.Jid,
            Body = message.text
        };
        mainClient.Send(msg);
        chatSpace.PopulateChat(message.text, DateTime.Now.ToString("hh:mm"),ChatType.main);
        appManager.itemSerialization.AddChatEntry(chatClient.Jid, message.text, ChatType.main);
        message.text = "";
    }

}

