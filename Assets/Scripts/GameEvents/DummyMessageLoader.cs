using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMessageLoader : MonoBehaviour {

    public MessageDialogController dialogsController;

    //public PlayableCharacter character;
    public string charName;
    public string quote;
    public string quote2;
    public string quote3;
    public string quote4;
    List<MessageDialog> msgList = new List<MessageDialog>();

    void CreateMessage()
    {
        MessageDialog msg1 = new MessageDialog();
        PlayableCharacter char1 = CharacterParty.charactersParty.Find(p => p.charStats.name.Equals(charName));
        msg1.avatarImage = char1.charStats.avatar;
        msg1.avatarName = char1.charStats.name;
        msg1.dialogText = quote;
        msgList.Add(msg1);

        MessageDialog msg2 = new MessageDialog();
        msg2.avatarImage = msg1.avatarImage;
        msg2.avatarName = msg1.avatarName;
        msg2.dialogText = quote2;
        msgList.Add(msg2);

        MessageDialog msg3 = new MessageDialog();
        msg3.dialogText = quote3;
        msgList.Add(msg3);

        MessageDialog msg4 = new MessageDialog();
        msg4.avatarImage = char1.charStats.avatar;
        msg4.avatarName = char1.charStats.name;
        msg4.dialogText = quote4;
        msgList.Add(msg4);


    }
	


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M) && !dialogsController.IsDialogActive)
        {
            CreateMessage();
            dialogsController.SetMessagesArray(msgList);
           dialogsController.ShowMessage();
        }
	}
}
