using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class lobbyManager : NetworkBehaviour
{
    //singleton
    public static lobbyManager Instance;
    public static roundManager round_Manager;
    public roundManager _round_Manager;

   


    public static GameObject[] getPlayerList()
    {
        return GameObject.FindGameObjectsWithTag("activePlayer");
    }


    private void Awake()
    {
        if (Instance == null && Instance != this)
            Instance = this;

        round_Manager = _round_Manager;
    }

    private void Start()
    {
        roundManager.onReset();
        uiManager.Instance.animator.Play(Animator.StringToHash("Lobby"), -1, 0);
        StartCoroutine(onInitialize());
 
    }

   

    
  

    public IEnumerator onInitialize()
    {
        colorManager.onInitialize();
        soundManager.playSound(soundManager.soundList["lobbyIn"], false, 1.20f);
        yield return new WaitForSeconds(0.25f);

        //show lobby and select System (character) for text-to-speech

        uiManager.setPhaseCanvas(ref uiManager.Instance.lobbyCanvas, true);
        InvokeRepeating("onUpdateLobby", 0.1f, 1f);
        uiManager.Instance.settings.setSystem(PlayerPrefs.GetString("system"));
        uiManager.Instance.roomCode.text = "10000000";
        

    }

  


    public void onBegin()
    {
        RpcChangeSettings(Settings.Parameters.getSystem(), Settings.Parameters.getLength(), Settings.Parameters.getTurns());
        if (getPlayerList().Length >= Settings.minPlayers)
            RpcBeginGame();

    }

    private void onBeginGame()
    {
        CancelInvoke("onUpdateLobby");
        uiManager.setPhaseCanvas(ref uiManager.Instance.lobbyCanvas, false);
        round_Manager.Initialize();

    }

    public void onCancelGame()
    {
        StartCoroutine(onCancel());
    }

    public IEnumerator onCancel()
    {
        CancelInvoke("onUpdateLobby");
        soundManager.playSound(soundManager.soundList["decline1"], false, 0.90f);

        yield return new WaitForSeconds(.25f);

        if (roundManager.isPlayerHost())
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();

        yield break;
    }


    private void onDisplayPlayer(int playerNb, string playerName)
    {
        GameObject.Find("playerUI" + playerNb).GetComponent<Text>().text = playerName;
    }

    private void onUpdateLobby()
    {
        GameObject[] playerList = getPlayerList();
        for (int i = 0; i < Settings.maxPlayers; i++)
        {
            if(i < playerList.Length)
            {
                Player player = playerList[i].GetComponent<Player>();
                onDisplayPlayer(i + 1, player.getName());
                playerList[i].name = player.getID();
            }
            else
                onDisplayPlayer(i + 1, "</UNKNOWN>"); 
        }
           
    }




    

    [ClientRpc]
    public void RpcBeginGame()
    {
        onBeginGame();
    }

  




    [ClientRpc]
    public void RpcChangeSettings(string system, float length, float turns)
    {
        turnManager.onInitialize(length,system, turns);
    }



    [ClientRpc]
    public void RpcSendRoundInfo(string ID, string name, string question)
    {
        //Store question, player Name and player ID on clients
        turnManager.setQuestionInfo(ID, name, new Librairy.Question(question, true));
        uiManager.Instance.targetLabel.text =  turnManager.getQuestionSelected().getValue();
    }

    [ClientRpc]
    public void RpcChangePhase(int phase)
    {
        if (roundManager.curPhase != null)
        {
            int newPhase;
            switch ((roundManager.currentPhase)phase)
            {
                default:
                    newPhase = phase - 1;
                    break;
                case roundManager.currentPhase.target:
                    newPhase = (int)roundManager.currentPhase.showcase;
                    break;
                case roundManager.currentPhase.tiebreaker:
                    newPhase = (int)roundManager.currentPhase.vote;
                    break;
                case roundManager.currentPhase.showcase:
                    newPhase = (int)roundManager.currentPhase.tiebreaker;
                    break;
            }
            // Set accurate phase and finish the previous one.
            roundManager.setPhaseByID((roundManager.currentPhase)(newPhase));
            roundManager.curPhase.onFinishPlayer();
        }
        round_Manager.setPhase((roundManager.currentPhase)phase);
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }


}