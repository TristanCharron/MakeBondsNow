using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public abstract class Menu
{
    //string used for Room and Player Name
    protected readonly string defaultPlayerName = dialogManager.dialogList["INPUTNAME"];
    protected readonly string defaultRoomCode = dialogManager.dialogList["INPUTROOMCODE"];
    protected static string playerName;
    protected static string roomCode;


    // enum for the current animation state
    public enum menuState
    {
        off,
        system,
        room,
        connected,
    };

    public static menuState state = menuState.off;


    public abstract void onConnect();
    public menuState getState() { return state; }
    public void setState(menuState State) { state = State; }

    public string getRoomCode() { return roomCode.ToUpper(); }
    public void setRoomCode(string room) { roomCode = room; }

    public string getPlayerName() { return playerName.ToUpper(); }
    public void setPlayerName(string name) { playerName = name; }

    protected void onSave()
    {
        roomCode = (state == menuState.room) ? menuManager.Instance.input.text.ToUpper() : roomCode;
        playerName = (state == menuState.system) ? menuManager.Instance.input.text.ToUpper() : playerName;

        PlayerPrefs.SetString("name", playerName);
        PlayerPrefs.SetString("roomCode", roomCode);
    }

    public void onInitialize()
    {
        // Reset Names and Codes
        playerName = defaultPlayerName;
        roomCode = defaultRoomCode;
        PlayerPrefs.SetString("name", playerName);
        PlayerPrefs.SetString("roomCode", roomCode);
        menuManager.Instance.menuInteractionCanvas.alpha = 1;
        menuManager.Instance.menuInteractionCanvas.interactable = true;
        Patient();

    }

    public void Patient()
    {
        //Patient is where you write your player name

        if (state == menuState.system)
            return;

        menuManager.Instance.input.characterValidation = InputField.CharacterValidation.Name;
        menuManager.Instance.input.characterLimit = 16;
        menuManager.Instance.input.text = state != menuState.off ? PlayerPrefs.GetString("name") : defaultPlayerName;

        state = menuState.system;
        menuManager.Instance.animator.Play(Animator.StringToHash("playerPatient"), -1, 0);
        soundManager.playSound(soundManager.soundList["accept1"], false, 3f);
        onSave();



    }
    public void Room()
    {
        //Patient is where you write your room code
        if (state == menuState.room)
            return;
      
        onSave();
        menuManager.Instance.input.characterValidation = InputField.CharacterValidation.Integer;
        menuManager.Instance.input.characterLimit = 8;
        state = menuState.room;

        menuManager.Instance.input.text = PlayerPrefs.GetString("roomCode");
        menuManager.Instance.animator.Play(Animator.StringToHash("playerRoom"), -1, 0);
        soundManager.playSound(soundManager.soundList["decline1"], false, 3f);


    }
    public void onMatch()
    {
       
        onSave();
        if (playerName.Length < Settings.minPlayerNameLength
            || playerName.Length > Settings.maxPlayerNameLength
            || playerName == defaultPlayerName
             )
        {
            soundManager.playSound(soundManager.soundList["accept1"], false, 0.95f);
            return;
        }
        else
            onConnect();
        
            
    }


}




public class menuClient : Menu
{
    public menuClient()
    {
        state = menuState.off;
    }

    public override void onConnect()
    {
        if(state != menuState.connected)
        {
            state = menuState.connected;
            PlayerPrefs.SetString("isHost", false.ToString());
            NetworkManager.singleton.StartClient();
        }


    }
}

public class menuHost : Menu
{
    public menuHost()
    {
        state = menuState.off;
    }

    public override void onConnect()
    {
        if (state != menuState.connected)
        {
            state = menuState.connected;
            PlayerPrefs.SetString("isHost", true.ToString());
            NetworkManager.singleton.StartHost();
        }

    }

}

public class menuManager : MonoBehaviour
{ 
    public static menuManager Instance;
    
    // Components
    public CanvasGroup menuInteractionCanvas;
    public InputField input;
    public Animator animator;
    public Menu menu;
    public GlitchEffect1 glitchEffect;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        // Determine if PC Host or Mobile player, and set parameters based on that.
        animator.Play(Animator.StringToHash("introPlayer"), -1, 0);
        //Play Intro sound
        soundManager.playMusic(soundManager.soundList["introMenu"], false, 1f, 0.95f);
        colorManager.onInitialize();
    }

    void Start()
    {
        if (Application.isEditor)
            menu = new menuHost();
        else
            menu = new menuClient();
    }






    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
            && menu.getState() == Menu.menuState.off)
            menu.onInitialize();
 
       
    }





    public void onMatch()
    {
        if (menu.getState() != Menu.menuState.off)
            StartCoroutine(enableGlitchEffect());
        
          
    }

    public void onSelectOption1()
    {
        if (menu.getState() != Menu.menuState.off)
            menu.Patient();

      
    }

    public void onSelectOption2()
    {
        if (menu.getState() != Menu.menuState.off)
            menu.Room();


    }

    public IEnumerator enableGlitchEffect()
    {
        float length = Random.Range(0.11f, 0.22f);
        StartCoroutine(glitchEffect.onGlitch(length));
        yield return new WaitForSeconds(length);
        menu.onMatch();
    }













}
