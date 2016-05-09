using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class targetPhase : Phase
{


    public targetPhase(int id)
    {
        ID = id;
    }


    public override IEnumerator onStartJingle()
    {
        // pull new targeted player and question
        determineRoundInfo();
        
        //Reset answers and turn variables
        answerManager.Reset();
        turnManager.onNextTurn();

        // reset timer, UI for first phase and reveal selected player
        base.onInitialize();
        turnManager.setTime(turnManager.getLength());
        yield return new WaitForSeconds(0.1f);
        round_Manager.StartCoroutine(revealSelectedPlayer());
        yield break;
    }



    public override void onTransition()
    {
        uiManager.Instance.animator.Play(Animator.StringToHash("playerTarget"), -1, 0);
    }


    public override void onFinishHost()
    { 
        lobby_Manager.RpcChangePhase((int)roundManager.currentPhase.type);
    }
  


    Librairy.Question getQuestion()
    {
        // go through question list and pull a new question for players
        List<Librairy.Question> questionList = Librairy.getQuestionList();
        int qIndex = 0;
        do
            qIndex = Random.Range(0, questionList.Count);
        while (questionList[qIndex].isSelected() == true);

        Librairy.pushQuestion(questionList[qIndex].getValue());
        questionList[qIndex].setSelection(true);
        return questionList[qIndex];
    }

    Player getPlayer()
    {
        // return new targeted player .
        Player player = null;
        GameObject[] playerList = lobbyManager.getPlayerList();
        int pIndex = Random.Range(0, playerList.Length);

        do
          player = playerList[pIndex].GetComponent<Player>();
        while (turnManager.getPlayerIDTargeted() == player.getID());

        Debug.Assert(player != null, "DETERMINE PLAYER NOT WORKING, NOT ENOUGH PLAYERS");
        return player;
    }


    public void determineRoundInfo()
    {
        if (roundManager.isPlayerHost())
        {
            //determine player, insert his name in question (Regex) and question for next turn.
            Player roundPlayer = getPlayer();
            Librairy.Question roundQuestion = getQuestion();
            roundQuestion.setValue(roundQuestion.onRegexPlayerNameInQuestion(roundPlayer.getName()));
            roundQuestion.setValue(roundQuestion.getValue().ToUpper());

            //Synchronise answers with other clients.
            turnManager.setQuestionInfo(roundPlayer.getID(), roundPlayer.getName(), roundQuestion);
            uiManager.Instance.targetLabel.text = turnManager.getQuestionSelected().getValue();
            lobbyManager.Instance.RpcSendRoundInfo(roundPlayer.getID(), roundPlayer.getName(), roundQuestion.getValue());
        }

    }

    public IEnumerator revealSelectedPlayer()
    {
        //get targeted player name && start in-animation
        uiManager.Instance.infoLabel.text = turnManager.getPlayerNameTargeted();
        round_Manager.StartCoroutine(animIn(uiManager.Instance.infoLabel.text));
        uiManager.Instance.GetComponent<UIEffects>().beginShuffle(turnManager.getPlayerNameTargeted(), 3f);
        
        // execute text fadeout Animation
        yield return new WaitForSeconds(4f);
        uiManager.Instance.infoLabel.GetComponent<UIEffects>().txtFadeInOut(turnManager.getPlayerNameTargeted());
        soundManager.playSound(soundManager.soundList["lobbyIn"], false, 0.95f);
        yield return new WaitForSeconds(1f);

        //execute out-animation
        soundManager.playVoice(colorManager.defaultColor, turnManager.getPlayerNameTargeted());
        yield return new WaitForSeconds(3f);
        round_Manager.StartCoroutine(animOut());
        yield return new WaitForSeconds(1.5f);
        finishPhase();
    }




}
