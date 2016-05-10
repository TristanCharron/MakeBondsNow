
using UnityEngine;

public class animationParameters : MonoBehaviour {
    public string animationName;
    public Vector3 translation;
    public Quaternion rotation;
    public float scaling;
    public Transform destinationTransform;
	public float length;
    public float delay;
	public int nbRepeats;
    [HideInInspector]

    public enum easeType{easeInQuad,easeOutQuad,easeInOutQuad,
	easeInCubic,easeOutCubic,easeInOutCubic,easeInQuart,easeOutQuart,
	easeInOutQuart,easeInQuint,easeOutQuint,easeInOutQuint,easeInSine,
	easeOutSine,easeInOutSine,easeInExpo,easeOutExpo,easeInOutExpo,easeInCirc,easeOutCirc,
	easeInOutCirc,linear,spring,easeInBack,
	easeOutBack,easeInOutBack};
	public easeType easeTypes;
    public enum afterAnimation
    {
        hideIt = 1,
        freezeIt = 2,
        loopIt = 3,
        none = 4,
    }
    public afterAnimation AfterAnimation;

    public float alphaBegin;
	public float alphaEnd;
    public bool inAndOut;
  



	//public animationParameters[] animationList;
	public AudioClip sound;
	public ParticleSystem particles;


	void Awake(){
		set();
       
}

	void Start(){



	
	}


	public void set()
	{
        if(!inAndOut)
            nbRepeats = 1;

        if (nbRepeats < 1)
			nbRepeats = 1;
		
		if (scaling == 0)
			scaling = 1;

	}

}
