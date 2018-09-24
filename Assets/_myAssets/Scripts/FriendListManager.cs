using Matrix.Xmpp.Roster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendListManager : MonoBehaviour {

    [SerializeField]
    private GameObject friendTemplate;
    [SerializeField]
    private GameObject userListObject;


    public void PopulateFriendList(RosterItem user)
    {
        GameObject temp = Instantiate(friendTemplate, userListObject.transform);
        temp.GetComponent<UserTemplate>().SetUserDetails(user);
    }
	
	
}
