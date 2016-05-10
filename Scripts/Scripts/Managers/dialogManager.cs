using UnityEngine;
using System.Collections.Generic;

public class dialogManager : MonoBehaviour
{
    public static Dictionary<string, string> dialogList;
    public static string[] dialogListFromFile;
   
    void Awake()
    {
        // List of in-game text that will be use from .txt

        dialogList = new Dictionary<string, string>();
        dialogList.Add("MENUDEFAULT", "INSERT PLAYER NAME");
        dialogList.Add("WAIT", "PLEASE WAIT");
        dialogList.Add("HURRYUP", "10 SECONDS LEFT !");
        dialogList.Add("INPUTNAME", "INSERT YOUR FIRST NAME");
        dialogList.Add("INPUTROOMCODE", "INSERT ROOM CODE");
        dialogList.Add("SHOWCASEBEGIN", "DID YOU WIN ?");
        dialogList.Add("SHOWCASEOVER", "ARE YOU READY TO MAKE MORE BONDS ?");
        dialogList.Add("SHOWCASEPLAYER", " PRESS ON THE SCREEN TO CONTINUE !");
        dialogList.Add("TIEBREAKERTARGETED", ", SELECT THE WINNING ANSWER !");
        dialogList.Add("TIEBREAKERPLAYER", " IS PICKING AN ANSWER !");
        dialogList.Add("TIEBREAKER", " TIEBREAKER !");
        dialogList.Add("VOTETARGETED", " YOU CANNOT VOTE SINCE YOU ARE TARGETED ! ");

    }


}


