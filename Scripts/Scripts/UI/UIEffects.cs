using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class UIEffects : MonoBehaviour
{
    // flicker variables
    public const float flickerAlphaMin = 0.70f, flickerAlphaMax = 0.99f, flickerRate = 0.01f;
    public const string charTable = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789*&?%$#@!";
    public static string randomString;

    //UI handling
    private Graphic curGraph;
    private Text curText;
    private Image curImg;
    private InputField curInput;
    private CanvasGroup canvas;

    //Parameters
    private static int shuffleLength;
    // booleans trigger
    private bool isFading = false, isFlickering = true;
    public static bool isShuffling = false;
    public bool isBlinking = false;



    //Shuffle effect variables (random list)
    public void beginShuffle(string finalTxt, float length) {
        isShuffling = true;
        StartCoroutine(shuffle(finalTxt, length));
    }

    public void beginBlink(float length) { StartCoroutine(blink(length)); }
    
    //Text alternation
    public void txtFadeInOut(string txt)
    {
        StartCoroutine(txtFlash(txt));
    }
    public void txtFadeIn(string txt)
    {
        StartCoroutine(txtIn(txt));
    }
    public void txtFadeIn()
    {
        StartCoroutine(txtIn());
    }
    public void txtFadeOut()
    {
        StartCoroutine(txtOut());
    }


    void Awake()
    {
      
        curGraph = GetComponent<Graphic>();
        canvas = GetComponent<CanvasGroup>();
        curText = GetComponent<Text>() != null ? GetComponent<Text>() : null;
        curImg = GetComponent<Image>() != null ? GetComponent<Image>() : null;
        curInput = GetComponent<InputField>() != null ? GetComponent<InputField>() : null;


        randomString = charTable;
        InvokeRepeating("flicker", 0, flickerRate);
        if (isBlinking)
            beginBlink(1);

    }
    void Update()
    {
        if (curText != null)
        {
            string newText = curText.text.ToUpper();
            curText.text = newText;
           
        }
        if (curInput != null)
        {
            string newText = curInput.text.ToUpper();
            curInput.text = newText;

        }

    }


  

    public IEnumerator shuffle(string text, float length)
    {
        soundManager.playSound(soundManager.soundList["shuffleLoop"], false, 1.2f);
        shuffleLength = text.Length;
        InvokeRepeating("shuffleText", 0, 0.01f);

        yield return new WaitForSeconds(length);

        CancelInvoke("shuffleText");
        curText.text = text;
        isShuffling = false;
        yield break;
    }


 
   
    void shuffleText()
    {
        curText.text = randomString.Substring(0,shuffleLength);
    }
    public static void shuffleRandomString()
    {
        if(isShuffling)
        {
            char[] stringChars = new char[40];
            for (int i = 0; i < stringChars.Length; i += 1)
                stringChars[i] = Random.value > 0.75f ? charTable[Random.Range(0, charTable.Length - 1)] : charTable[0];
            randomString = new string(stringChars).ToUpper();
        }
    }
       


    private IEnumerator txtFlash(string txt)
    {
        if (isFading)
            yield break;
        StartCoroutine(txtOut());
        yield return new WaitForSeconds(0.18f);
        curText.text = txt;
        yield return new WaitForSeconds(0.07f);
        StartCoroutine(txtIn());
        yield return new WaitForSeconds(curText.text.Length / 3);
        isFading = false;
        yield break;
    }

    private IEnumerator txtIn()
    {
        if (isFading)
            yield break;

        isFading = true;
        curGraph.canvasRenderer.SetAlpha(0.01f);
        curGraph.CrossFadeAlpha(1, 0.18f, true);

        yield return new WaitForSeconds(0.18f);

        isFading = false;
        yield break;
    }

    private IEnumerator txtIn(string txt)
    {
        if (isFading)
            yield break;

        curText.text = txt;
        isFading = true;
        curGraph.canvasRenderer.SetAlpha(0.01f);
        curGraph.CrossFadeAlpha(1, 0.18f, true);

        yield return new WaitForSeconds(0.18f);

        isFading = false;
        yield break;
    }

    private IEnumerator txtOut()
    {
        if (isFading)
            yield break;

        isFading = true;
        curGraph.canvasRenderer.SetAlpha(0.99f);
        curGraph.CrossFadeAlpha(0.01f, 0.18f, true);

        yield return new WaitForSeconds(0.18f);

        isFading = false;
        yield break;
    }

    void flicker()
    {
        if(isFlickering)
        canvas.alpha = Random.Range(flickerAlphaMin, flickerAlphaMax);
       
    }
    private IEnumerator blink(float length)
    {
        float Length = Random.Range(0.1f, length);

        if (isBlinking && curText != null)
        {
            isBlinking = false;
            isFlickering = false;
            canvas.alpha = Random.Range(0.60f,0.80f);

            yield return new WaitForSeconds(Length);

            canvas.alpha = 1;
            isFlickering = true;
            isBlinking = true;

            yield return new WaitForSeconds(length - (Length * 2));
            beginBlink(1);
        }
        yield break;
    }





}
