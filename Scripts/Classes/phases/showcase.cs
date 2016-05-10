using UnityEngine;
using System.Collections;

public class showcasePhase : Phase
{
    public showcasePhase(int id)
    {
        ID = id;
    }

    public override IEnumerator onStartJingle()
    {
        isActive = false;
        soundManager.playJingle(soundManager.soundList["showcaseJingle"]);
        onInitialize();
        yield return new WaitForSeconds(2.25f);
        uiManager.Instance.GetComponent<UIEffects>().txtFadeInOut("");
        round_Manager.StartCoroutine(onDisplayAnswer());
        yield break;


    }
    public override void onStartPhase()
    {
        uiManager.setPhaseCanvas(ref uiManager.Instance.showcasingCanvas, true);
        uiManager.Instance.completedRoundBtn.onClick.AddListener(() => { onSubmit(); });
        uiManager.Instance.infoLabel.GetComponent<UIEffects>().txtFadeInOut(dialogManager.dialogList["SHOWCASEPLAYER"]);
    }


    public override void onFinishHost()
    {
        State = phaseState.off;
        roundManager.currentPhase phase;

        if (turnManager.getTurnsPlayed() < turnManager.getTurns())
          phase = roundManager.currentPhase.target;
        else
          phase = roundManager.currentPhase.ending;

        lobbyManager.Instance.RpcChangePhase((int)phase);
        round_Manager.StartCoroutine(animOut());

    }



    public override void onFinishPlayer()
    {
        round_Manager.StartCoroutine(animOut());
    }



    private IEnumerator onDisplayAnswer()
    {
        // Animation that reveal the winning answers to everyone
        // player can now confirm they ar ready for next turn

        string winningAnswer = answerManager.getAnswerByPosition(false,1).getValue();
        soundManager.playVoice(colorManager.defaultColor, turnManager.getQuestionSelected().getValue());
        uiManager.Instance.GetComponent<UIEffects>().beginShuffle(winningAnswer, 6f);
        yield return new WaitForSeconds(7f);
        uiManager.Instance.GetComponent<UIEffects>().txtFadeInOut(winningAnswer);
        soundManager.playSound(soundManager.soundList["lobbyIn"], false, 0.95f);
        soundManager.playVoice(colorManager.defaultColor, winningAnswer);
        yield return new WaitForSeconds(7f);
        onStartPhase();
        yield break;


    }

  


    public override void onSubmit()
    {
        //Player confirm he's ready for next round,wait for remaining players
        base.onSubmit();
        uiManager.Instance.infoLabel.GetComponent<UIEffects>().txtFadeInOut(dialogManager.dialogList["WAIT"]);
        uiManager.Instance.completedRoundBtn.onClick.RemoveAllListeners();
        currentPlayer.CmdPlayerFinishedPhase(currentPlayer.getID());
        uiManager.setPhaseCanvas(ref uiManager.Instance.showcasingCanvas, false);

    }




}
