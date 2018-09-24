using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Matrix.Xmpp.Roster;

public class UIManager : MonoBehaviour
{

    public static UIManager instance = null;

    public GameObject loginPanel;
    public GameObject friendsPanel;
    public GameObject chatPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void OpenChat(RosterItem user)
    {
        friendsPanel.SetActive(false);
        chatPanel.SetActive(true);
        chatPanel.GetComponent<ChatPanel>().InitializeChatPanel(user);
    }

    public void OpenFriendsList()
    {
        chatPanel.SetActive(false);
        loginPanel.SetActive(false);
        friendsPanel.SetActive(true);
    }

    public void OpenLoginPanel()
    {
        chatPanel.SetActive(false);
        loginPanel.SetActive(true);
        friendsPanel.SetActive(false);
    }
}
