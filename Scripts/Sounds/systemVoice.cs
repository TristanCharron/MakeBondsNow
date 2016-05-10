using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class systemVoice  {
     [System.Serializable]
    public struct Voice { 
        public string systemName;
        public Culture culture;
        public float pitch;
        public float volume;
        public int cultureIndex;
        public bool byPassEffects;
        public Tone voiceTone;
        
    }


    public enum Tone
    {
        Neutral = 0,
        Anxious = 1,
    }

    public enum Culture
    {
        en = 0,
    }

    public List<Voice> voiceList;

    public Voice applyVoiceTone(Voice voice, Tone tone)
    {
        Voice curVoice = voice;
        switch(tone)
        {
            case Tone.Anxious:
                curVoice.pitch += 0.03f;
                curVoice.volume += 0.2f;
                break;
                
        }
        return curVoice;
    }
     

    
}
