using UnityEngine;
using System.Collections;


public abstract class Phase
{

    public enum phaseState
    {
        off = 0,
        on,
        completed,
    };

    //Member variables
    protected static roundManager round_Manager;
    protected static lobbyManager lobby_Manager;
    protected static Player currentPlayer;
    protected static phaseState State;
    protected static bool isActive = false, isSubmiting = false;
    protected int ID;





    public Phase()
    {
        currentPlayer = roundManager.getCurrentPlayer();
        lobby_Manager = lobbyManager.Instance;
        round_Manager = lobby_Manager._round_Manager;

    }



    public int getID() { return ID; }

    //Execute code when transitioning
    public virtual void onTransition(phaseState state) { }
    public virtual void onTransition() { }


    public IEnumerator animIn(string txtValue)
    {
        //lobbyManager.mainLabelAnimator.play(animator.animIn);
        yield return new WaitForSeconds(1f);
        uiManager.Instance.infoLabel.text = txtValue;
    }

    public IEnumerator animOut()
    {
        //lobbyManager.mainLabelAnimator.play(animator.animOut);
        yield break;
    }





    public void Update()
    {
        onUpdate();
    }



    public virtual void onSubmit(string data)
    {
        onSubmit();
    }

    public virtual void onSubmit()
    {
        soundManager.playSound(soundManager.soundList["accept1"], false, 1.5f);
        isSubmiting = true;
    }


    public virtual bool isTargeted() { return roundManager.isTargeted(); }



    public abstract IEnumerator onStartJingle();
    public virtual void onStartPhase() { }
    public void finishPhase() { if (roundManager.isPlayerHost()) onFinishHost(); else onFinishPlayer(); }

    public abstract void onFinishHost();
    public virtual void onFinishPlayer() { }


    public void setPhaseState(phaseState state) { State = state; }
    public phaseState getPhaseState() { return State; }


    public virtual void onInitialize()
    {
        State = phaseState.on;
        isActive = false;
        onTransition();
        uiManager.setDefaultUI();
    }

    public virtual void onUpdate()
    {
        if (isActive)
            turnManager.onUpdate();
    }









}

