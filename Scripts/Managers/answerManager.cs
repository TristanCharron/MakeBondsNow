using UnityEngine;
using System.Collections.Generic;
using tools;


public class Answer
{

    private int Score;
    private string Value;
    private string Author;
    private GameObject gObject;
    public Answer(string answerValue, string author)
    {
        Score = 0;
        Value = answerValue;
        Author = author;
        gObject = null;
    }
    public Answer(string answerValue, string author, int answerScore)
    {
        Score = answerScore;
        Value = answerValue.ToUpper();
        Author = author.ToUpper();
        gObject = null;
    }

    // get set accessors for answer member variables
    public void resetScore() { Score = 0; }
    public int getScore() { return Score; }
    public void setScore(int nbVotes) { Score += nbVotes; }

    public void setValue(string value) { Value = value.ToUpper(); }
    public string getValue() { return Value.ToUpper(); }
    public string getAuthor() { return Author; }
   
    public GameObject getGObject() { return gObject; }
    public void setGObject(GameObject Object) { gObject = Object; }

}

public static class answerHistory
{
    // Class allowing to keep track of player own answers
    private static Dictionary<int, List<Answer>> list = new Dictionary<int, List<Answer>>();
    public static Dictionary<int, List<Answer>> getList() { return list; }
    public static void insert(int i, List<Answer> List) { list.Add(i, List); }
}




public static class answerManager
{
    // Queue of answer to be voted 
    private static Queue<Answer> votingQueue;
    //List of player personnal answer
    private static List<Answer> answerList;
    public const string noAnswer = " HAD NO ANSWER !";


    public static void Reset()
    {
        roundManager.getCurrentPlayer().CmdSetPlayerAnswer(roundManager.getCurrentPlayer().getID());
        if (answerList != null && answerList.Count > 0)
            answerHistory.insert((int)turnManager.getTurnsPlayed(), answerList);
        votingQueue = new Queue<Answer>();
        answerList = new List<Answer>();
    }


    public static void enqAnswer(Answer answer)
    {
        votingQueue.Enqueue(answer);
    }
    public static void deqAnswer()
    {
        if (votingQueue.Count > 0)
            votingQueue.Dequeue();

    }
    public static Answer peekAnswer()
    {

        if (votingQueue.Count > 0)
            return votingQueue.Peek();
        Debug.Assert(votingQueue.Count > 0, "PEEKING FROM EMPTY VOTING QUEUE");
        return null;


    }
    public static Queue<Answer> getQueue() { return votingQueue; }

    public static List<Answer> getList() { return answerList; }

    public static Answer getAnswerByPosition(bool isTieBreaker,int position)
    {
        // Cannot return his own answer if tiebreaker
        if (isTieBreaker)
        {
            if (answerList[answerList.Count - position].getAuthor().Equals(turnManager.getPlayerIDTargeted()))
                return answerList[answerList.Count - 3];
        }
        return answerList[answerList.Count - position];
    }



    public static void sortAnswerByScore()
    {
        /// [HOST] Sorting answers based on score (descending order), 
        QuickSort.Sort(ref answerList, 0, answerList.Count - 1);
    }


    public static bool isTieBreaker()
    {
        /// [HOST] if tiebreaker, make sure targetedPlayer cannot vote for himself in tiebreaker.

        sortAnswerByScore();
        if (getAnswerByPosition(false,1).getScore().Equals(getAnswerByPosition(false,2).getScore()))
            return true;
        else
        {
            Debug.Log(getAnswerByPosition(false,1).getValue() + "SCORE : " + getAnswerByPosition(false,2).getScore());
            return false;
        }

    }

    public static void setAnswerList()
    {
        for (int i = 0; i < lobbyManager.getPlayerList().Length; i++)
        {
            Player player = lobbyManager.getPlayerList()[i].GetComponent<Player>();
            string playerAnswer = player.getPlayerAnswer();
            answerList.Add(new Answer(playerAnswer, player.getID()));
        }
    }

    public static void setVotingQueue()
    {
        // fill out answer queue except own player answer that he cannot vote

        string ownAnswer = roundManager.getCurrentPlayer().getPlayerAnswer();
        for (int i = 0; i < lobbyManager.getPlayerList().Length; i++)
        {
            Player player = lobbyManager.getPlayerList()[i].GetComponent<Player>();
            string playerAnswer = player.getPlayerAnswer();

            if (!ownAnswer.Equals(playerAnswer))
                enqAnswer(new Answer(playerAnswer, player.getID()));
        }
        Debug.Assert(votingQueue.Count > 0, "VOTING QUEUE IS EMPTY");
    }


    public static Answer getAnswerByName(string answer)
    {
        Answer ans = null;
        for (int i = 0; i < answerList.Count; i++)
            if (answer.Equals(answerList[i].getValue()))
                return answerList[i];

        Debug.Assert(ans != null, "RETURNING ANSWER PITCHED BY NAME:" + answer + "IS NULL");
        return ans;
    }


    public static bool isQueueEmpty()
    {
        return votingQueue.Count < 1 ? true : false;
    }








}
