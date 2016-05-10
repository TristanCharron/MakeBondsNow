
using System.Collections.Generic;
using System.Text.RegularExpressions;


public static class Librairy
{
   
    private static List<Question> questionList;
    private static List<string> questionHistory;



    public class Question
    {
        public Question()
        {
            Value = "";
            selected = false;
            Tag = tag.off;
            Target = "";
        }
        public Question(string entryValue)
        {
            Value = entryValue;
            selected = false;
            Tag = tag.off;
            Target = "";
        }
        public Question(string entryValue, bool state)
        {
            Value = entryValue;
            selected = state;
            Tag = tag.off;
            Target = "";
        }
        private bool selected;
        private string Value;
        private string Target;
        public string getTarget() { return Target; }
        public bool isSelected() { return selected; }
        public void setSelection(bool selection) { selected = selection; }
        public string getValue() { return Value; }
        public void setValue(string value) { Value = value; }
        public string onRegexPlayerNameInQuestion(string playerName)
        {
            // Use Regex.Replace to replace the target PLayer in the output.
            string pattern = "<.+>";
            Target = playerName;
            Regex rgx = new Regex(pattern);
            string question = rgx.Replace(Value, playerName, 1);
            return question.Substring(question.LastIndexOf(Target));
        }
 
      
        public enum tag
        {
            off = 0,
            NSFW,
            Other,
        };
        tag Tag;

    }

    public static void createQuestionList()
    {
        questionList = new List<Question>();
        questionHistory = new List<string>();

        /// Fill Question List once (will eventually be a .txt format)

        questionList.Add(new Question("What has <BILL> recently been accused of ?"));
        questionList.Add(new Question("What can <BILL> never get enough of?"));
        questionList.Add(new Question("How does <BILL> smell like?"));
        questionList.Add(new Question("What would <BILL>'s superpower be?"));
        questionList.Add(new Question("Describe <BILL> in one single word"));
        questionList.Add(new Question("What's <BILL>'s favorite pick-up line?"));
        questionList.Add(new Question("What <BILL> is afraid of?"));
        questionList.Add(new Question("How <BILL> is named in Dungeons & Dragons ?"));
        questionList.Add(new Question("Give a name to <BILL>'s future child !"));
        questionList.Add(new Question("First thing <BILL> does in the morning ."));
        questionList.Add(new Question("Determine <BILL>'s new name in the hood."));
        questionList.Add(new Question("Imagine <BILL>'s cocktail name."));
        questionList.Add(new Question("Why is <BILL> crying in the shower ?"));
        questionList.Add(new Question("What was <BILL> dreaming about last night ?"));
        questionList.Add(new Question("What will generations to come always remember <BILL> for ?"));
        questionList.Add(new Question("What does <BILL> look for in a potential partner ?"));
        questionList.Add(new Question("What was <BILL>'s nickname in middle school ?"));
        questionList.Add(new Question("What does <BILL> never leave home without ?"));
        questionList.Add(new Question("What would <BILL> buy first if they won the lottery ?"));
        questionList.Add(new Question("What's the name of <BILL>'s favorite song ?"));
        questionList.Add(new Question("What is <BILL>'s key to success ?"));
        questionList.Add(new Question("What is <BILL>'s biggest talent ?"));
        questionList.Add(new Question("What puts <BILL> in a good mood ?"));
        questionList.Add(new Question("What is <BILL>'s spirit animal?"));
    }

  
    public static List<Question> getQuestionList()
    {
        return questionList;
    }

 

    public static void pushQuestion(string question)
    {
        questionHistory.Add(question);
    }

   





}