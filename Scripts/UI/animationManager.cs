using UnityEngine;
using UnityEngine.UI;
using tools;

public class animationManager : MonoBehaviour {

    private animationParameters inAnimation;
    private animationParameters outAnimation;

    public  animationParameters getInAnimation() { return inAnimation; }
    public animationParameters getOutAnimation() { return outAnimation; }

    private CanvasGroup canvas;
    private Text textBox;
    private bool isPlaying;
    [HideInInspector]
    public string text;
    private float alpha;
    private int counter = 1;
    private bool inOrOut = false, isSoundPlayed = false;


    public struct initialTransform
    {
        public Vector3 initialScale;
        public Vector3 initialPos;
        public Quaternion initialRot;
    }

    private initialTransform initialTrans;

    public initialTransform returnInitialTransform() { return initialTrans; }






    // Use this for initialization
    void Start() {
        if (GetComponent<animationParameters>() == null)
            Debug.LogError("No Animation parameters has been found : " + gameObject.name);

        if (GetComponent<CanvasGroup>() != null)
            canvas = GetComponentInParent<CanvasGroup>();

        initialTrans.initialScale = transform.localScale;
        initialTrans.initialRot = transform.rotation;
        initialTrans.initialPos = transform.position;
    }



 
    public void play( animationParameters anim)
    {
        inAnimation = anim;
        isSoundPlayed = false;
        setAudio();
        alpha = anim.alphaBegin;
        canvas.alpha = alpha;
        onAnimateObject(anim);
        onAnimateTextBox(anim);
    }

    private void onAnimateObject(animationParameters anim)
    {
       //Move, rotate, scale and apply alpha tween
        if (anim.destinationTransform == null)
            iTween.MoveBy(anim.gameObject, iTween.Hash("y", anim.translation.y, "x", anim.translation.x, "looptype", "none", "time", anim.length, "easeType", anim.easeTypes.ToString(), "oncomplete", "finishAnimation"));
        else
            iTween.MoveTo(anim.gameObject, iTween.Hash("y", anim.destinationTransform.position.y, "x", anim.destinationTransform.position.x, "looptype", "none", "time", anim.length, "easeType", anim.easeTypes.ToString(), "oncomplete", "finishAnimation"));

        iTween.RotateBy(anim.gameObject, iTween.Hash("z", anim.rotation.z, "y", anim.rotation.y, "x", anim.rotation.z, "time", anim.length, "easeType", anim.easeTypes.ToString()));

        iTween.ScaleBy(anim.gameObject, iTween.Hash("x", anim.scaling, "y", anim.scaling, "looptype", "none", "time", anim.length, "easeType", anim.easeTypes.ToString()));

        if (anim.alphaBegin != anim.alphaEnd)
            iTween.ValueTo(anim.gameObject, iTween.Hash("from", anim.alphaBegin, "to", anim.alphaEnd, "time", anim.length, "onupdate", "setAlpha", "looptype", "none", "easeType", anim.easeTypes.ToString()));
    }

    private void onAnimateTextBox(animationParameters anim)
    {
        if (textBox != null)
        {
            /// TEXT BOX , APPLY ANIMATIONS
            iTween.MoveBy(textBox.gameObject, iTween.Hash("y", anim.translation.y, "x", anim.translation.x, "looptype", "none", "time", anim.length, "easeType", anim.easeTypes.ToString()));
            iTween.RotateBy(textBox.gameObject, iTween.Hash("z", anim.rotation.z, "y", anim.rotation.y, "x", anim.rotation.z, "C", "none", "time", anim.length, "easeType", anim.easeTypes.ToString()));
            iTween.ScaleBy(textBox.gameObject, iTween.Hash("x", anim.scaling, "y", anim.scaling, "looptype", "none", "time", anim.length, "easeType", anim.easeTypes.ToString()));
            iTween.ValueTo(textBox.gameObject, iTween.Hash("from", anim.alphaBegin, "to", anim.alphaEnd, "time", anim.length, "onupdate", "setAlpha", "looptype", "none", "easeType", anim.easeTypes.ToString()));
        }
    }

    public void play(string animationName, Vector3 position, string message) {
        if (!isPlaying)
        {
            counter = 1;
            transform.position = position;
            textBox.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, textBox.transform.position.z);
            textBox.text = message;
            inAnimation = returnAnimationByName(animationName);
            outAnimation = onMirrorAnimation();
            play(inAnimation);
            isPlaying = true;
        }


    }
    public void play(string animationName)
    {
        if (!isPlaying)
        {
            counter = 1;
            inAnimation = returnAnimationByName(animationName);
            outAnimation = onMirrorAnimation();
            play(inAnimation);
            isPlaying = true;
        }


    }


    public animationParameters returnAnimationByName(string animName)
    {
        animationParameters[] animList = GetComponents<animationParameters>();
        foreach (animationParameters anim in animList)
            if (anim.animationName == animName)
                return anim;

        Debug.LogError("Animation" + animName + " doesn't exist !");
        return animList[0];

    }

    public animationParameters returnMirrorAnimationByName(string animName)
    {
        animationParameters[] animList = GetComponents<animationParameters>();
        foreach (animationParameters anim in animList)
            if (anim.animationName == animName)
            {
                inAnimation = anim;
                return onMirrorAnimation();
            }
                

        Debug.LogError("Animation" + animName + " doesn't exist !");
        return animList[0];

    }




    private animationParameters onMirrorAnimation()
    {
        //will mirror an in to out or out to in by switch values

        animationParameters mirrorAnimation = gameObject.AddComponent<animationParameters>();
        mirrorAnimation.length = inAnimation.length;
        mirrorAnimation.scaling = 1 / inAnimation.scaling;
        mirrorAnimation.inAndOut = inAnimation.inAndOut;
        mirrorAnimation.AfterAnimation = inAnimation.AfterAnimation;
        mirrorAnimation.alphaBegin = inAnimation.alphaBegin;
        mirrorAnimation.alphaEnd = inAnimation.alphaEnd;

        mirrorAnimation.rotation = new Quaternion(-1 * inAnimation.rotation.x, 
            -1 * inAnimation.rotation.y, -1 * inAnimation.rotation.z, inAnimation.rotation.w);
        mirrorAnimation.translation = new Vector3(-1 * inAnimation.translation.x, -1 *
            inAnimation.translation.y, -1 * inAnimation.translation.z);

        mirrorEaseType(ref mirrorAnimation.easeTypes);
        
        // Inverse Alpha values based on in animation
        if (inAnimation.alphaBegin == 0 && inAnimation.alphaEnd == inAnimation.alphaBegin)
            onSetAlpha(ref mirrorAnimation, 1, 1);

        else if (inAnimation.alphaBegin == 1 && inAnimation.alphaEnd == inAnimation.alphaBegin)
            onSetAlpha(ref mirrorAnimation, 0, 0);

        else // Swap begin and ending alpha value
            onSetAlpha(ref mirrorAnimation, inAnimation.alphaEnd, inAnimation.alphaBegin);
        
        return mirrorAnimation;
    }

    private void onSetAlpha(ref animationParameters anim, float alpha1, float alpha2)
    {
        anim.alphaBegin = alpha1;
        anim.alphaEnd = alpha2;
    }


    private void mirrorEaseType(ref animationParameters.easeType easeType)
    {
        // Send ease in or out, and match with opposite ease.

        switch (easeType)
        {
            case animationParameters.easeType.easeInBack:
                easeType = animationParameters.easeType.easeOutBack;
                break;
            case animationParameters.easeType.easeOutBack:
                easeType = animationParameters.easeType.easeInBack;
                break;
            case animationParameters.easeType.easeInCirc:
                easeType = animationParameters.easeType.easeOutCirc;
                break;
            case animationParameters.easeType.easeOutCirc:
                easeType = animationParameters.easeType.easeInCirc;
                break;
            case animationParameters.easeType.easeInCubic:
                easeType = animationParameters.easeType.easeOutCubic;
                break;
            case animationParameters.easeType.easeOutCubic:
                easeType = animationParameters.easeType.easeInCubic;
                break;
            case animationParameters.easeType.easeInExpo:
                easeType = animationParameters.easeType.easeOutExpo;
                break;
            case animationParameters.easeType.easeOutExpo:
                easeType = animationParameters.easeType.easeInExpo;
                break;
            case animationParameters.easeType.easeInQuad:
                easeType = animationParameters.easeType.easeOutQuad;
                break;
            case animationParameters.easeType.easeOutQuad:
                easeType = animationParameters.easeType.easeInQuad;
                break;
            case animationParameters.easeType.easeInQuart:
                easeType = animationParameters.easeType.easeOutQuart;
                break;
            case animationParameters.easeType.easeOutQuart:
                easeType = animationParameters.easeType.easeInQuart;
                break;
            case animationParameters.easeType.easeInQuint:
                easeType = animationParameters.easeType.easeOutQuint;
                break;
            case animationParameters.easeType.easeOutQuint:
                easeType = animationParameters.easeType.easeInQuint;
                break;
            case animationParameters.easeType.easeInSine:
                easeType = animationParameters.easeType.easeOutSine;
                break;
            case animationParameters.easeType.easeOutSine:
                easeType = animationParameters.easeType.easeInSine;
                break;
        }
    }


    private void finishAnimation()
    {
        if(inAnimation.AfterAnimation == animationParameters.afterAnimation.loopIt)
        {
            timer.setTimer(gameObject, inAnimation.delay, "playOnDelay");
            inOrOut = !inOrOut;
            return;
        }


        if (counter > inAnimation.nbRepeats)
        {
            timer.setTimer(gameObject, inAnimation.delay, "resetAnimation");
            return;
        }

        if (counter == inAnimation.nbRepeats)
        {
            
            if(inAnimation.inAndOut)
            {
                inOrOut = true;
                counter++;
                timer.setTimer(gameObject, inAnimation.delay, "playOnDelay");
            }
            else
                timer.setTimer(gameObject, inAnimation.delay, "resetAnimation");

            return;
        }

       
        else
        {
            timer.setTimer(gameObject, inAnimation.delay, "playOnDelay");
            inOrOut = !inOrOut;
            return;
        }
             
    }
	
    private void playOnDelay()
    {
        if (!inOrOut)
        {
            play(inAnimation);
            counter++;
        }
           
        else
            play(outAnimation);
        
          

    }

    private void resetAnimation()
    {
        isPlaying = false;
        inOrOut = false;

        if (inAnimation.AfterAnimation == animationParameters.afterAnimation.freezeIt)
            return;

        if (inAnimation.AfterAnimation == animationParameters.afterAnimation.hideIt)
            canvas.alpha = 0;
        
        gameObject.transform.localScale = initialTrans.initialScale;
        gameObject.transform.rotation = initialTrans.initialRot;
        gameObject.transform.position = initialTrans.initialPos;




    }
	
	
	void setAudio(){


		if (inAnimation.sound != null && !isSoundPlayed) {
            soundManager.playSound(inAnimation.sound, false,1.25f);
			isSoundPlayed = true;
		}

	}

	
	void setAlpha(float newValue)
	{
		alpha = newValue;
        canvas.alpha = alpha;
    }
	
	
	


}
