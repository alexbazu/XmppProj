using Matrix;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Matrix.Xmpp.Sasl;
using System.IO;
using UnityEngine.UI;
using Matrix.Xmpp.Roster;
using Matrix.Net;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    private InputField usernameText;
    [SerializeField]
    private InputField passwordText;
    private string licenseFilename = "license.txt";
    public List<RosterItem> buddyList = new List<RosterItem>();
    private FriendListManager friendListManager;

    public GameObject loginPanel;
    public GameObject friendsPanel;
    public ItemSerialization itemSerialization;

    private bool isLoggedIn = false;
    private bool shouldUpdate;

    public XmppClient mainUser;

    // Use this for initialization
    void Start()
    {
        byte[] licBytes = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, licenseFilename));
        string lic = System.Text.Encoding.ASCII.GetString(licBytes);
        Matrix.License.LicenseManager.SetLicense(lic);

        Debug.LogWarning("License error:" + Matrix.License.LicenseManager.LicenseError);
        Debug.LogWarning("License raw:" + Matrix.License.LicenseManager.RawLicense);

        UIManager.instance.OpenLoginPanel();
        friendListManager = GetComponent<FriendListManager>();
    }

    public void LogIn()
    {
        mainUser = new XmppClient();

        mainUser.SetUsername(usernameText.text);
        mainUser.SetXmppDomain("localhost");
        mainUser.Password = passwordText.text;
        mainUser.Status = "I am online!";
        mainUser.Show = Matrix.Xmpp.Show.Chat;
        try { mainUser.Open(); mainUser.OnLogin += delegate { isLoggedIn = true; }; }
        catch { Debug.Log("Wrong login data!"); }

        mainUser.AutoRoster = true;
        mainUser.OnRosterItem += new EventHandler<RosterEventArgs>(OnRosterItem);
        mainUser.OnMessage += MainUser_OnMessage;
    }

    private void MainUser_OnMessage(object sender, MessageEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Message.Body))
            itemSerialization.AddChatEntry(e.Message.From.ToString().Split('/')[0], e.Message.Body, ChatType.client);
    }

    private void Update()
    {
        if (isLoggedIn)
        {
            UIManager.instance.OpenFriendsList();
            isLoggedIn = false;
        }
        if (shouldUpdate)
        {
            foreach (RosterItem item in buddyList)
            {
                friendListManager.PopulateFriendList(item);
            }
            shouldUpdate = false;
        }
    }

    private void OnRosterItem(object sender, Matrix.Xmpp.Roster.RosterEventArgs e)
    {
        buddyList.Add(e.RosterItem);
        shouldUpdate = true;
    }
}
