using UnityEngine;

public static class turnManager
{
    private static float curTime = 0, nbTurnsPlayed;
    private static Librairy.Question questionSelected;
    private static string playerNameTargeted = " ", playerIDTargeted = " ";

    public static void onInitialize(float length, string system, float turns)
    {
        Settings.Parameters.onChange(system,turns,length);
        nbTurnsPlayed = 0;
      
    }
    public static float getTurns()
    {
        return Settings.Parameters.getTurns();
    }
    public static string getGameCompletionPercentage()
    {
        float completion = (nbTurnsPlayed / Settings.Parameters.getTurns()) * 100;
        return Mathf.Clamp(completion, 0, 99).ToString("N0");
    }
    public static float getTurnsPlayed()
    {
        return nbTurnsPlayed;
    }
    public static void onNextTurn()
    {
        statsManager.resetTurnBonds();
        nbTurnsPlayed++;
        uiManager.Instance.turnsTxt.text = getGameCompletionPercentage();
        uiManager.Instance.answerLabel.text = "";

    }

    public static void setQuestionInfo(string id, string name, Librairy.Question quest)
    {
        questionSelected = quest;
        playerNameTargeted = name;
        playerIDTargeted = id;
    }
    public static string getPlayerNameTargeted()
    {
        return playerNameTargeted;
    }
    public static string getPlayerIDTargeted()
    {
        return playerIDTargeted;
    }
    public static Librairy.Question getQuestionSelected()
    {
        return questionSelected;
    }
    public static string getSystem()
    {
        return Settings.Parameters.getSystem();
    }

    public static float getLength()
    {
        return Mathf.Clamp(Settings.Parameters.getLength(), 20, 100);
    }


    public static void setTime(float time)
    {
        curTime = Mathf.Clamp(time, 0, time);
        if (time == 0)
            uiManager.Instance.timerTxt.text = "00";
        else
            uiManager.Instance.timerTxt.text = (curTime < 10) ? "0" + curTime.ToString() : curTime.ToString();

    }

    public static float getTime()
    {
        return curTime;
    }

    public static void onUpdate()
    {
        if (curTime > 0)
            setTime(curTime - Time.deltaTime);
    }





}
