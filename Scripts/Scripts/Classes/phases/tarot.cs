using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tarotPhase : Phase
{
    enum tarotChoice
    {
        first = 0,
        second,
        third,
    };

    tarotChoice tarotCardChoice;

    public tarotPhase(int id)
    {
        ID = id;
    }


    public override IEnumerator onStartJingle()
    {
        isActive = false;
        turnManager.setTime(0);
        soundManager.playJingle(soundManager.soundList["voteJingle"]);
        base.onInitialize();
        yield return new WaitForSeconds(2.25f);
        onStartPhase();
        yield break;
    }



    public override void onStartPhase()
    {

        if (!answerManager.isTieBreaker())
        {
            finishPhase();
            return;
        }
        else
        {
            if (roundManager.isTargeted())
            {
                uiManager.Instance.infoLabel.GetComponent<UIEffects>().txtFadeInOut(currentPlayer.getName() + dialogManager.dialogList["TIEBREAKERTARGETED"]);
                onInitialize();
            }
            else
                uiManager.Instance.infoLabel.GetComponent<UIEffects>().txtFadeInOut(currentPlayer.getName() + dialogManager.dialogList["TIEBREAKERPLAYER"]);
        }

        State = phaseState.on;
        soundManager.playVoice(colorManager.defaultColor, dialogManager.dialogList["TIEBREAKER"]);
        uiManager.Instance.infoLabel.GetComponent<UIEffects>().txtFadeInOut(uiManager.Instance.infoLabel.text);
    }


    public override void onInitialize()
    {
        uiManager.setPhaseCanvas(ref uiManager.Instance.tiebreakingCanvas, true);
       
    }

  


    public override void onFinishHost()
    {
        lobbyManager.Instance.RpcChangePhase((int)roundManager.currentPhase.showcase);
    }

    public override void onFinishPlayer()
    {

    }


    public override void onSubmit()
    {
        base.onSubmit();
        switch (tarotCardChoice)
        {
            case tarotChoice.first:
                currentPlayer.CmdOnSendVote(answerManager.getAnswerByPosition(true,1).getValue()
                    , currentPlayer.getID());
                break;
            case tarotChoice.second:
                currentPlayer.CmdOnSendVote(answerManager.getAnswerByPosition(true,2).getValue(),
                    currentPlayer.getID());
                break;
        }





    }




}
