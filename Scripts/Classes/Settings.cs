using UnityEngine;
using UnityEngine.UI;





public class Settings : MonoBehaviour
	{
	// Settings txts for lobby
	public Text txtSfx, txtTimer, txtTurns, txtSystem, txtRoomCode;

    // Handle parameters in-game for host
    public static class Parameters
    {
        private static string System;
        public static string getSystem() { return System; }

        private static float Turns;
        public static float getTurns() { return Turns; }

        private static float Length;
        public static float getLength() { return Length; }

        public static void onChange(string system, float turns, float length)
        {
            System = system;
            Turns = turns;
            Length = length;
        }
    }

    //Parameters arrays
    private readonly int[] timerOptions = new int[]{20,60,75,90};
	private readonly int[] turnsOptions = new int[]{3,10,15,20,25,30};
	private readonly string[] systemOptions = new string[]{ colorManager.defaultColor, colorManager.color1, colorManager.color2};
    private readonly string[] sfxOptions = new string[] { "ON","OFF" };
	
    // Settings Indexes
	private static bool sfxState = true;
	private static int timerIndex = 0, turnsIndex = 0, systemIndex = 0;

    //Const defined parameters in-game
    public const int maxPlayers = 8, minPlayers = 2;
    public const int minTurns = 5, maxTurns = 20;
    public const int maxAnswerLength = 30, maxPlayerNameLength = 16;
    public const int minPlayerNameLength = 2;
    public const float roundLength = 60f;


    void Start()
	{
        enabled = roundManager.isPlayerHost();
		txtRoomCode.text = "ROOM::" + PlayerPrefs.GetString ("roomCode");
        onUpdateUI();
        onSave();
    }



	public void onChangeSFX()
	{
        // is sound activated or not.
        sfxState = !sfxState;
        string sound = sfxState ? "accept1" : "decline1";
        AudioListener.pause = !sfxState;
        soundManager.playSound(soundManager.soundList[sound], false, 3f);
        onUpdateUI();
	}

    

	public void onChangeTimer(Button btn)
	{
        // Change game length
        onChangeParameter(btn, ref timerIndex);

        if (timerIndex < 0)
			timerIndex = timerOptions.Length-1;
		else if(timerIndex > timerOptions.Length-1)
			timerIndex = 0;

        onUpdateUI();
    }

	public void onChangeTurns(Button btn)
	{
        // Change turns nb
        onChangeParameter(btn, ref turnsIndex);

        if (turnsIndex < 0)
			turnsIndex = turnsOptions.Length-1;
		else if(turnsIndex > turnsOptions.Length-1)
			turnsIndex = 0;

        onUpdateUI();
	}

	public void onChangeSystem(Button btn)
	{
        // Change character text-to-speech voice
        onChangeParameter(btn, ref systemIndex);

        if (systemIndex < 0)
            systemIndex = systemOptions.Length - 1;

        else if (systemIndex > systemOptions.Length - 1)
            systemIndex = 0;
            

        onUpdateUI();

    }

    public void setSystem(string system)
    {
        for (int i = 0; i < systemOptions.Length; i++)
        {
            if (systemOptions[i] == system)
            {
                txtSystem.text = systemOptions[i];
                systemIndex = i;
                break;
            }

        }


    }


    private void onChangeParameter(Button btn, ref int index)
    {
        //Based on button selection, trigger appropriate sound & UI Changes
        bool isLeft = btn.name.IndexOf("Left") > -1;
        string sound = isLeft ? "accept1" : "decline1";
        soundManager.playSound(soundManager.soundList[sound], false, 3f);
        index = isLeft ? index - 1 : index + 1;
      
    }

    private void onUpdateUI()
	{

        txtSfx.text = sfxState ? sfxOptions[0] : sfxOptions[1];
        txtTimer.text = timerOptions[timerIndex].ToString();
        txtTurns.text = turnsOptions[turnsIndex].ToString();
        txtSystem.text = systemOptions[systemIndex];

        onSave();

	}

	private void onSave()
	{
		PlayerPrefs.SetInt ("turns",turnsOptions[turnsIndex]);
		PlayerPrefs.SetInt ("timer",timerOptions[timerIndex]);
		PlayerPrefs.SetString (colorManager.system,systemOptions[systemIndex]);

        if (roundManager.isPlayerHost())
            lobbyManager.Instance.RpcChangeSettings(systemOptions[systemIndex],timerOptions[timerIndex], turnsOptions[turnsIndex]);
    }

  

   
 
		

	}


