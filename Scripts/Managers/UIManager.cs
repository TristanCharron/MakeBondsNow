using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uiManager : MonoBehaviour {

    //singleton
    public static uiManager Instance;

    //UI Elements
    public Animator animator;

    public InputField answerLabel;

    public animationManager _mainLabelAnimator;

    public static animationManager mainLabelAnimator;

    public Text turnsTxt, timerTxt, roomCode, tiebreakerAns1, tiebreakerAns2, infoLabel, targetLabel,
        votingAnswerTxt1, votingAnswerTxt2;

    public Settings settings;
    public CanvasGroup targetAndTypingCanvas, mainCanvas, lobbyCanvas,
      votingCanvas, showcasingCanvas, tiebreakingCanvas;

    public Button sendAnswerBtn, topVoteBtn, downVoteBtn, completedRoundBtn;

    public GlitchEffect1 glitchEffect;

    void Awake()
    {
        if (Instance == null && Instance != this)
            Instance = this;
    }

    void Update()
    {
        //Force uppercase and create generate characters for shuffle visual effect.
        Instance.answerLabel.text = Instance.answerLabel.text.Length > 0 ? Instance.answerLabel.text.ToUpper() : "";
        UIEffects.shuffleRandomString();
    }

    public static void setPhaseCanvas(ref CanvasGroup canvas, bool isCanvasOn)
    {
        // enable or disable interaction and alpha of referenced canvas
        canvas.alpha = isCanvasOn ? 1 : 0;
        canvas.interactable = isCanvasOn;
        canvas.blocksRaycasts = isCanvasOn;
        canvas.transform.parent.gameObject.SetActive(isCanvasOn);
    }

    public static void setDefaultUI()
    {
        // hide every phase specific UI except constant information (Time, game progress)
        setPhaseCanvas(ref Instance.targetAndTypingCanvas, false);
        setPhaseCanvas(ref Instance.votingCanvas, false);
        setPhaseCanvas(ref Instance.mainCanvas, true);
        setPhaseCanvas(ref Instance.showcasingCanvas, false);
        setPhaseCanvas(ref Instance.tiebreakingCanvas, false);

    }

    public static UIEffects getUIEffect(GameObject parent)
    {
        //return desired ui-text effect on ui element.
        if (parent.GetComponent<UIEffects>() != null)
            return parent.GetComponent<UIEffects>();
        Debug.Assert(parent.GetComponent<UIEffects>() != null, "NO UI EFFECT COMPONENT FOUND" + parent.name);
            return null;
    }
}
