using UnityEngine;
using Crosstales.RTVoice;
using System.Collections.Generic;






public class soundManager : MonoBehaviour
{

    //Components
    public static AudioSource sound_AudioSource;
    public static AudioSource voice_AudioSource;
    public static AudioSource music_AudioSource;
    private static systemVoice SystemVoice;
    public systemVoice _SystemVoice;
    public static Dictionary<string, AudioClip> soundList;

    //Singleton
    public static soundManager Instance { get; private set; }

    // Use this for initialization
    void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(transform.gameObject);
        else
        {
            Instance = this;
            sound_AudioSource = GameObject.Find("musicChannel").GetComponent<AudioSource>();
            voice_AudioSource = GameObject.Find("voiceChannel").GetComponent<AudioSource>();
            music_AudioSource = GameObject.Find("soundChannel").GetComponent<AudioSource>();
            onLoad();
            onEnableTextToSpeech();
        }

    }


    //fill soundlist at loading (hashtable)
    private void onLoad()
    {
        soundList = new Dictionary<string, AudioClip>();
        soundList.Add("showcaseJingle", Resources.Load("Sounds/jingles/Jingle1") as AudioClip);
        soundList.Add("voteJingle", Resources.Load("Sounds/jingles/Jingle9") as AudioClip);
        soundList.Add("shuffleLoop", Resources.Load("Sounds/familyBond/LoopShuffle") as AudioClip);
        soundList.Add("typeJingle", Resources.Load("Sounds/jingles/Jingle8") as AudioClip);
        soundList.Add("lobbyIn", Resources.Load("Sounds/familyBond/networkSound") as AudioClip);
        soundList.Add("tick", Resources.Load("Sounds/background/tick") as AudioClip);
        soundList.Add("accept1", Resources.Load("Sounds/familyBond/Accept") as AudioClip);
        soundList.Add("decline1", Resources.Load("Sounds/familyBond/Decline") as AudioClip);
        soundList.Add("introMenu", Resources.Load("Sounds/familyBond/introMenu") as AudioClip);
        soundList.Add("ambiance", Resources.Load("Sounds/background/ambiance") as AudioClip);
    }

   
    private void onEnableTextToSpeech()
    {
        GameObject rtVoice = Instantiate(Resources.Load("rtVoice"), gameObject.transform.position, Quaternion.identity) as GameObject;
        rtVoice.transform.parent = gameObject.transform;
        rtVoice.tag = "rtVoices";
        SystemVoice = _SystemVoice;
    }


    //Change sound ambiance depending of current Phase
    public static void setAmbiance(bool isActive)
    {
        sound_AudioSource.Stop();
        music_AudioSource.Stop();
        if (isActive)
        {
            playSound(soundList["tick"], true, 0.08f, 1.1f);
            playMusic(soundList["ambiance"], true, 0.5f, 0.90f);
        }
        else
            playMusic(soundList["ambiance"], true, 0.5f, 1.2f);


    }


    public static void playSound(AudioClip sound, bool loop, float pitch)
    {
        playSound(sound, loop, 0.15f, pitch);
    }

    public static void playSound(AudioClip sound, bool loop, float volume, float pitch)
    {
        sound_AudioSource.loop = loop;
        sound_AudioSource.volume = volume;
        sound_AudioSource.pitch = pitch;
        sound_AudioSource.clip = sound;
        sound_AudioSource.Play();
    }

    public static void playMusic(AudioClip sound, bool loop, float vol, float pitch)
    {
        music_AudioSource.loop = loop;
        music_AudioSource.volume = vol;
        music_AudioSource.pitch = pitch;
        music_AudioSource.clip = sound;
        music_AudioSource.Play();

    }



    public static void playVoice(string systemName, string msg)
    {
        systemVoice.Voice system = returnVoiceByName(systemName);
        voice_AudioSource.loop = false;
        voice_AudioSource.volume = SystemVoice.voiceList[0].volume;
        voice_AudioSource.pitch = SystemVoice.voiceList[0].pitch;
        Speaker.Speak(msg, voice_AudioSource, Speaker.VoicesForCulture(SystemVoice.voiceList[0].culture.ToString())[SystemVoice.voiceList[0].cultureIndex]);
    }


    public static void playVoice(string systemName, systemVoice.Tone tone, string msg)
    {
        systemVoice.Voice system = SystemVoice.applyVoiceTone(returnVoiceByName(systemName), tone);
        voice_AudioSource.loop = false;
        voice_AudioSource.volume = system.volume;
        voice_AudioSource.pitch = system.pitch;
        Speaker.Speak(msg, voice_AudioSource, Speaker.VoicesForCulture(system.culture.ToString())[system.cultureIndex]);

    }


    public static void playJingle(AudioClip jingle)
    {
        playMusic(jingle, false, 0.2f, 1.25f);
    }



    public static systemVoice.Voice returnVoiceByName(string voiceName)
    {
        //Return character text-to-speech reference by name
        systemVoice.Voice voice;
        foreach (systemVoice.Voice curVoice in SystemVoice.voiceList)
        {
            if (curVoice.systemName == voiceName)
            {
                voice = curVoice;
                return voice;
            }
        }
        voice.systemName = "NULL";
        Debug.Assert(voice.systemName != "NULL","NO SYSTEM VOICE FOUND : returnVoiceByName" + voiceName);
        return SystemVoice.voiceList[0];

    }

    public static void onStop()
    {
        voice_AudioSource.Stop();
        sound_AudioSource.Stop();
        music_AudioSource.Stop();
    }












}

