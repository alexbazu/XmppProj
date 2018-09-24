using Matrix.Xmpp.Roster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserTemplate : MonoBehaviour
{

    [SerializeField]
    private Text username;

    private RosterItem thisUser;

    public void SetUserDetails(RosterItem user)
    {
        username.text = user.Jid.User;
        thisUser = user;
    }

    public void StartChat()
    {
        UIManager.instance.OpenChat(thisUser);
    }
}
