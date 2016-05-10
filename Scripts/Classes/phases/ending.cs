using UnityEngine;
using System.Collections;


public class endingPhase : Phase
{

    public endingPhase(int id)
    {
        ID = id;
    }

    public override IEnumerator onStartJingle()
    {
        soundManager.playJingle(soundManager.soundList["showcaseJingle"]);
        yield return new WaitForSeconds(1f);
        onInitialize();
        if (roundManager.isPlayerHost())
            statsManager.returnStatistics();
        yield break;


    }





    public override void onFinishHost()
    {
        State = phaseState.off;
    }



    public override void onFinishPlayer()
    {
        State = phaseState.off;
    }




    public override void onTransition()
    {
        if (roundManager.isPlayerHost())
        {
            uiManager.Instance.animator.Play(Animator.StringToHash("hostShowcase"), -1, 0);
            round_Manager.StartCoroutine(animOut());
        }
    }

   



}
