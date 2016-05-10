using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tiebreakerPhase : Phase
{
    enum answerChoice
    {
        first = 0,
        second,
    };
    answerChoice ansChoice;

    public tiebreakerPhase(int id)
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
            // if player is targeted, he decides the winning answer
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
        round_Manager.StartCoroutine(onTieBreaker());
    }

    IEnumerator onTieBreaker()
    {
        // fade in-out answers with equal score
        yield return new WaitForSeconds(3f);
        uiManager.Instance.GetComponent<UIEffects>().txtFadeOut();
        yield return new WaitForSeconds(1f);
        uiManager.Instance.GetComponent<UIEffects>().txtFadeInOut(answerManager.getAnswerByPosition(true,1).getValue());
        yield return new WaitForSeconds(1f);
        uiManager.Instance.GetComponent<UIEffects>().txtFadeInOut(answerManager.getAnswerByPosition(true,2).getValue());

        // Listeners when selecting the winning answer
        uiManager.Instance.tiebreakerAns1.GetComponent<Button>().onClick.AddListener(() =>
        { ansChoice = answerChoice.first; onSubmit(); });
        uiManager.Instance.GetComponent<Button>().onClick.AddListener(() =>
        { ansChoice = answerChoice.second; onSubmit(); });
        yield break;

    }

  

    IEnumerator onFinishTieBreaker()
    {
        // fade answers animation based on selection
        switch (ansChoice)
        {
            case answerChoice.first:
                uiManager.Instance.tiebreakerAns2.GetComponent<UIEffects>().txtFadeOut();
                yield return new WaitForSeconds(1f);
                uiManager.Instance.tiebreakerAns2.text = "";
                uiManager.Instance.tiebreakerAns1.GetComponent<UIEffects>().txtFadeOut();
                yield return new WaitForSeconds(1f);
                uiManager.Instance.tiebreakerAns1.text = "";
                break;
            case answerChoice.second:
                uiManager.Instance.tiebreakerAns1.GetComponent<UIEffects>().txtFadeOut();
                yield return new WaitForSeconds(1f);
                uiManager.Instance.tiebreakerAns1.text = "";
                uiManager.Instance.tiebreakerAns2.GetComponent<UIEffects>().txtFadeOut();
                yield return new WaitForSeconds(1f);
                uiManager.Instance.tiebreakerAns2.text = "";
                break;
        }

        // send ending phase signal to host
        currentPlayer.CmdHostFinishedPhase();
        uiManager.Instance.tiebreakerAns1.GetComponent<Button>().onClick.RemoveAllListeners();
        uiManager.Instance.tiebreakerAns2.GetComponent<Button>().onClick.RemoveAllListeners();
        uiManager.setPhaseCanvas(ref uiManager.Instance.tiebreakingCanvas, false);
        uiManager.Instance.infoLabel.text = "";
        yield break;

    }


    public override void onFinishHost()
    {
            lobbyManager.Instance.RpcChangePhase((int)roundManager.currentPhase.showcase);
    }


    public override void onSubmit()
    {
        base.onSubmit();
        switch (ansChoice)
        {
            case answerChoice.first:
                currentPlayer.CmdOnSendVote(answerManager.getAnswerByPosition(true,1).getValue()
                    , currentPlayer.getID());
                break;
            case answerChoice.second:
                currentPlayer.CmdOnSendVote(answerManager.getAnswerByPosition(true,2).getValue(),
                    currentPlayer.getID());
                break;
        }
        round_Manager.StartCoroutine(onFinishTieBreaker());
    }






}
