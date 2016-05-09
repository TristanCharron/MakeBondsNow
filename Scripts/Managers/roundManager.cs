using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class roundManager : MonoBehaviour
{
    private static roundManager Instance;
    private static Player currentPlayer;
    public static List<Phase> Phases = new List<Phase>();
    public static Phase curPhase;
    public List<BondsManager> bondsList;
    private static bool isStarted = false, isHost = false;


    public enum currentPhase
    {
        off = 0,
        target = 1,
        type = 2,
        vote = 3,
        tiebreaker = 4,
        showcase = 5,
        ending = 6,

    };


    void Awake()
    {
        setHost();
        Librairy.createQuestionList();
    }

    public static void setHost() { isHost = PlayerPrefs.GetString("isHost") == true.ToString() ? true : false; }
    public static bool isPlayerHost() { return isHost; }

    public static Player getCurrentPlayer() { return currentPlayer; }

    public static Player returnPlayerByID(string id) {
        Player p = null;
        foreach (GameObject player  in lobbyManager.getPlayerList())
            if (player.GetComponent<Player>().getID() == id)
                return player.GetComponent<Player>();
        Debug.Assert(p == null, "returnPlayerByID did not work" + id);
        return p;
    }

 

    public void setCurrentPlayer(Player player) {
        currentPlayer = player;
    }

    public void setPhase(currentPhase phase)
    {  
        setPhaseByID(phase);
        currentPlayer.setAllPlayersCompleted(false);
        StartCoroutine(curPhase.onStartJingle());
    }


    public static bool isTargeted()
    {
        return currentPlayer.getID() == turnManager.getPlayerIDTargeted() ? true : false;
    }

    public void Initialize()
    {
        statsManager.Initialize();
        onReset();
        isStarted = true;
        InvokeRepeating("isPlayerListEmpty", 0, 5f);
        setPhase(currentPhase.target);
       

    }

    public static void onReset()
    {
        Phases.Clear();
        Phases.Add(new targetPhase((int)currentPhase.target));
        Phases.Add(new typePhase((int)currentPhase.type));
        Phases.Add(new votePhase((int)currentPhase.vote));
        Phases.Add(new tiebreakerPhase((int)currentPhase.tiebreaker));
        Phases.Add(new showcasePhase((int)currentPhase.showcase));
        Phases.Add(new endingPhase((int)currentPhase.ending));
        curPhase = null;
        currentPlayer.setPlayerCompleted(false);
    }
   

    private void isPlayerListEmpty()
    {
      
        if (lobbyManager.getPlayerList().Length < Settings.minPlayers)
        {
            if (isHost)
                NetworkManager.singleton.StopHost();
            else
                NetworkManager.singleton.StopClient();
        }


    }




    public static void setPhaseByID(currentPhase phase)
    {
        switch(phase)
        {
            case currentPhase.target:
                curPhase = Phases[0] as targetPhase;
                break;
            case currentPhase.type:
                curPhase = Phases[1] as typePhase;
                break;
            case currentPhase.vote:
                curPhase = Phases[2] as votePhase;
                break;
            case currentPhase.tiebreaker:
                curPhase = Phases[3] as tiebreakerPhase;
                break;
            case currentPhase.showcase:
                curPhase = Phases[4] as showcasePhase;
                break;
            case currentPhase.ending:
                curPhase = Phases[5] as endingPhase;
                break;
        }



    }



    // Update is called once per frame
    void Update()
    {

        if (curPhase != null && isStarted)
            curPhase.Update();
          
    }
}
