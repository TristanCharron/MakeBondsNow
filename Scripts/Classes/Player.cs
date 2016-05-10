using UnityEngine;
using UnityEngine.Networking;








public class Player : NetworkBehaviour
{

    [SyncVar]
    private string playerName = "", playerID = "", playerAnswer = "";

    [SyncVar]
    private bool isCompleted = false;
    public string _playerAnswer;
    public int _nbBonds = 0;

    [SyncVar]
    private int nbBonds = 0;

    [SerializeField]
    public SyncListString answerList = new SyncListString();





    // Get Set Accessors for player inner member variables
    public string getID() { return playerID.ToUpper(); }
    public int getNbBonds() { return nbBonds; }
    public void setNbBonds(int amount) { nbBonds += amount; }
    public string getName() { return playerName.ToUpper(); }
    public void setPlayerInfo(string name, string id) { playerName = name.ToUpper(); playerID = id.ToUpper(); }
    public string getPlayerAnswer() { return playerAnswer.ToUpper(); }
    public void setPlayerAnswer(string answer) { playerAnswer = answer == "" ? "" : answer.ToUpper(); }
    
    // Used to determine if players are done
    public bool isPlayerCompleted() { return isCompleted; }
    public void setPlayerCompleted(bool state) { isCompleted = state; }
    public void setAllPlayersCompleted(bool state)
    {
        foreach (GameObject player in lobbyManager.getPlayerList())
            player.GetComponent<Player>().setPlayerCompleted(state);
    }





    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            lobbyManager.round_Manager.setCurrentPlayer(this);
            playerName = PlayerPrefs.GetString("name");
            sendPlayerInfo( playerName);
        }
    }


    void sendPlayerInfo(string playerName)
    {
        if (roundManager.isPlayerHost())
            RpcSendPlayerInfo(playerName);
        else
            CmdSendPlayerInfo(playerName);

    }

    [ClientRpc]
    public void RpcSendPlayerInfo(string playerName)
    {
        setPlayerInfo(playerName);
    }
    [Command]
    public void CmdSendPlayerInfo(string playerName)
    {
        setPlayerInfo(playerName);
    }

    void setPlayerInfo(string playerName)
    {
        Player addedPlayerManager = lobbyManager.getPlayerList()[lobbyManager.getPlayerList().Length - 1].GetComponent<Player>();
        string id = playerName.Substring(0, 4) + Random.Range(1000, 10000).ToString();
        addedPlayerManager.gameObject.name = id;
        addedPlayerManager.setPlayerInfo(playerName, id);
        addedPlayerManager.setPlayerAnswer("");
        soundManager.playVoice(colorManager.defaultColor, playerName + " has joined the game ! ");
    }


    [Command]
    public void CmdOnSendVote(string votedAnswer, string _votingPlayer)
    {
        countVote(votedAnswer,_votingPlayer);
        RpcOnSendVote(votedAnswer, _votingPlayer);
    }

    [ClientRpc]
    public void RpcOnSendVote(string votedAnswer, string votingPlayerID)
    {
        if(!roundManager.isPlayerHost())
        countVote(votedAnswer, votingPlayerID);

    }

    void countVote(string votedAnswer, string votingPlayerID)
    {  
        Answer answer = answerManager.getAnswerByName(votedAnswer);
        Player votedPlayer = GameObject.Find(answer.getAuthor()).GetComponent<Player>();
        Player votingPlayer = GameObject.Find(votingPlayerID).GetComponent<Player>();
        Player targetedPlayer = GameObject.Find(turnManager.getPlayerIDTargeted()).GetComponent<Player>();

        int voteValue = answer.getValue() == targetedPlayer.getPlayerAnswer() ? 2 : 1;
        answer.setScore(voteValue);
        votedPlayer.setNbBonds(voteValue);

        statsManager.setBond(votingPlayer.getID(),votedPlayer.getID());
        answerManager.sortAnswerByScore();

    }



    [Command]
    public void CmdSetPlayerAnswer(string id)
    {
        GameObject.Find(id).GetComponent<Player>().setPlayerAnswer("");
    }


    [Command]
    public void CmdSendAnswer(string answer, string id)
    {
        Player player = GameObject.Find(id).GetComponent<Player>();
        player.setPlayerAnswer(answer);
        player.answerList.Add(answer);
    }

    [Command]
    public void CmdPlayerFinishedPhase(string playerID)
    {
        GameObject.Find(playerID).GetComponent<Player>().setPlayerCompleted(true);
        if (arePlayersCompleted())
            roundManager.curPhase.finishPhase();
    }


    [Command]
    public void CmdHostFinishedPhase()
    {
        roundManager.curPhase.onFinishHost();
    }

    public bool arePlayersCompleted()
    {
        foreach (GameObject p in lobbyManager.getPlayerList())
            if (!p.GetComponent<Player>().isPlayerCompleted())
                return false;

        return true;


    }

}





