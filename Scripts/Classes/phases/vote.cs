using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class votePhase : Phase
{
    public votePhase(int id)
    {
        ID = id;
        outBondsDown = GameObject.Find("outMaskDown").GetComponent<Transform>();
        outBondsTop = GameObject.Find("outMaskTop").GetComponent<Transform>();
        Debug.Assert(outBondsDown != null || outBondsTop != null, "votePhase : OutBonds is undefined on constructor !");
    }

    public enum vote
    {
        none = 0,
        first,
        second,
    }
    private vote voteChoice;
    private Answer topAnswer, downAnswer;
    private Transform outBondsDown, outBondsTop;

    public override IEnumerator onStartJingle()
    {
        // reset UI and phase member variables
        voteChoice = vote.none; 
        isActive = false;
    
        turnManager.setTime(turnManager.getLength());
        soundManager.playJingle(soundManager.soundList["voteJingle"]);
        uiManager.setDefaultUI();
        yield return new WaitForSeconds(2.25f);

        onStartPhase();
        isActive = true;
        soundManager.setAmbiance(true);
        soundManager.playVoice(colorManager.defaultColor, " MAKE BONDS NOW ! ");
        answerManager.setAnswerList();

        yield break;
    }


    public override void onStartPhase()
    {
        // Fill out voting queue with other clients answers.
        answerManager.setVotingQueue();
        onTransition(phaseState.on);
        uiManager.setPhaseCanvas(ref uiManager.Instance.votingCanvas, true);
        
        // animate and generate top and bottom answer for vote on screen
        onDisplayAnswer(vote.first,ref topAnswer, uiManager.Instance.votingAnswerTxt1.gameObject);
        onDisplayAnswer(vote.second, ref downAnswer, uiManager.Instance.votingAnswerTxt2.gameObject);
        topAnswer.getGObject().GetComponent<animationManager>().play("topAnswer");
        downAnswer.getGObject().GetComponent<animationManager>().play("downAnswer");

        uiManager.Instance.topVoteBtn.onClick.AddListener(() =>{ voteChoice = vote.first; onSubmit(); });
        uiManager.Instance.downVoteBtn.onClick.AddListener(() => { voteChoice = vote.second; onSubmit(); });
      
    }
    void onDisplayAnswer(vote Vote, ref Answer answer, GameObject answerObject)
    {
        // Peek from queue and display on screen
        answer = answerManager.peekAnswer();
        answer.setGObject(answerObject);
        Text txt = Vote == vote.first ? uiManager.Instance.votingAnswerTxt1 : uiManager.Instance.votingAnswerTxt2;
        txt.text = answer.getValue();
        answerManager.deqAnswer();
    }
  


    public override void onSubmit()
    {
       if (turnManager.getTime() <= 1f || isSubmiting)
            return;

        base.onSubmit();

        switch (voteChoice)
        {
            case vote.first:
                round_Manager.StartCoroutine(onSelectAnswerUp());
                break;
            case vote.second:
                round_Manager.StartCoroutine(onSelectAnswerDown());
                break;
        }

        if (answerManager.isQueueEmpty())
        {
            //Complete the phase and send a message to host that he's done
            onFinishPlayer();
            currentPlayer.CmdPlayerFinishedPhase(currentPlayer.getID());
            currentPlayer.CmdOnSendVote(topAnswer.getValue(),currentPlayer.getID());
            return;
        }

    }

   IEnumerator onSelectAnswerUp()
    {
        // Anchor points are made on the screen with a mask, if answer goes out higher than the mask
        // loop at bottom and vice versa
        if (answerManager.isQueueEmpty())
            downAnswer.getGObject().GetComponent<animationManager>().play("outBondsDown");
        else
        {
            downAnswer.getGObject().GetComponent<animationManager>().play("outBondsDown");
            yield return new WaitForSeconds(0.7f);
            onDisplayAnswer(vote.second, ref downAnswer, uiManager.Instance.votingAnswerTxt2.gameObject);
            downAnswer.getGObject().GetComponent<animationManager>().play("downAnswer");
        }
        isSubmiting = false;
        yield break;
    }
    IEnumerator onSelectAnswerDown()
    {

        topAnswer.getGObject().GetComponent<animationManager>().play("outBondsTop");
        downAnswer.getGObject().GetComponent<animationManager>().play("topAnswer");
        yield return new WaitForSeconds(0.7f);
        if (!answerManager.isQueueEmpty())
        {
            onDisplayAnswer(vote.first, ref topAnswer, uiManager.Instance.votingAnswerTxt1.gameObject);
            topAnswer.getGObject().GetComponent<animationManager>().play("downAnswer");
            GameObject ans = topAnswer.getGObject();
            topAnswer.setGObject(downAnswer.getGObject());
            downAnswer.setGObject(ans);
        }
       
        isSubmiting = false;
        yield break;
    }
 


    public override void onTransition(phaseState state)
    {
        State = state;
        string anim = "";
        switch (state)
        {

            default:
                if (uiManager.Instance.infoLabel.text != dialogManager.dialogList["WAIT"])
                {
                    anim = "playerVoteEnd";
                    uiManager.Instance.infoLabel.GetComponent<UIEffects>().txtFadeInOut(dialogManager.dialogList["WAIT"]);
                }
                break;
            case phaseState.on:
                uiManager.Instance.infoLabel.text = answerManager.peekAnswer().getValue();
                anim = "playerVote";
                break;
        }

        uiManager.Instance.animator.Play(Animator.StringToHash(anim), -1, 0);


    }




    public override void onFinishHost()
    {
        State = phaseState.off;
        lobbyManager.Instance.RpcChangePhase((int)roundManager.currentPhase.tiebreaker);
        onFinishPlayer();
    }


    public override void onFinishPlayer()
    {
        State = phaseState.completed;
        onTransition(phaseState.completed);
        uiManager.Instance.topVoteBtn.onClick.RemoveAllListeners();
       uiManager.Instance.downVoteBtn.onClick.RemoveAllListeners();

    }

    public override void onUpdate()
    {
        float time = turnManager.getTime();
        base.onUpdate();
        if (time < 11.01f && time > 11.0f && State == phaseState.on)
            soundManager.playVoice(colorManager.defaultColor, systemVoice.Tone.Anxious, dialogManager.dialogList["HURRYUP"]);

        else if (time == 0 && isActive)
            if (roundManager.isPlayerHost())
                finishPhase();

    }


}

