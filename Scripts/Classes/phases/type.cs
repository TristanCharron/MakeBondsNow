using UnityEngine;
using System.Collections;

public class typePhase : Phase
{

    public typePhase(int id)
    {
        ID = id;
    }

    public override IEnumerator onStartJingle()
    {
        base.onInitialize();
        soundManager.playJingle(soundManager.soundList["typeJingle"]);
        yield return new WaitForSeconds(2.25f);
        onStartPhase();
    }



    public override void onStartPhase()
    {
        isActive = true;
        soundManager.setAmbiance(true);
        soundManager.playVoice(colorManager.defaultColor,turnManager.getQuestionSelected().getValue());
        uiManager.setPhaseCanvas(ref uiManager.Instance.targetAndTypingCanvas, true);
        uiManager.Instance.answerLabel.ActivateInputField();
        uiManager.Instance.sendAnswerBtn.onClick.AddListener(() =>{onSubmit(uiManager.Instance.answerLabel.text); });
    }


    public override void onTransition()
    {
        string anim;
        switch (State)
        {
            default:
                anim = "playerTypeWait";
                break;
            case phaseState.on:
                anim = "playerType";
                break;
        }
        uiManager.Instance.animator.Play(Animator.StringToHash(anim), -1, 0);
    }



    public override void onFinishHost()
    {
        isActive = false;
        lobbyManager.Instance.RpcChangePhase((int)roundManager.currentPhase.vote);
    }




    public override void onFinishPlayer() 
    {
        if (State == phaseState.on)
        {
            // get playerInput and send it to clients based on length
            string playerInput = uiManager.Instance.answerLabel.text;
            string answer = playerInput.Length < 1 ? currentPlayer.getName().ToLower() + answerManager.noAnswer : playerInput;
            currentPlayer.CmdSendAnswer(answer, currentPlayer.getID());

            State = turnManager.getTime() > 0 ? phaseState.completed : phaseState.off;
            onTransition();
            uiManager.Instance.sendAnswerBtn.onClick.RemoveAllListeners();
            uiManager.Instance.answerLabel.DeactivateInputField();
            soundManager.playSound(soundManager.soundList["accept1"], false, 1.5f);
        }
    }


    public override void onSubmit(string answerValue)
    {

        if (answerValue.Length > 0 && turnManager.getTime() >= 1f)
        {
            onFinishPlayer();
            currentPlayer.CmdPlayerFinishedPhase(currentPlayer.getID());
            isSubmiting = false;
        }
        


    }

    public override void onUpdate()
    {
        float time = turnManager.getTime();
        base.onUpdate();
       
        if (Mathf.Ceil(time) == 10 && State == phaseState.on)
            soundManager.playVoice(colorManager.defaultColor, systemVoice.Tone.Anxious, dialogManager.dialogList["HURRYUP"]);

        // Host will terminate the phase at count 0
        else if (time == 0 && isActive && roundManager.isPlayerHost())
                finishPhase();
        
    }




}
