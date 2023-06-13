using UnityEngine;
public class QuestionReader : MonoBehaviour
{
    public TextAsset questionsJSON;
    public QuestionList questionList = new QuestionList();
    private void Awake()
    {
        questionList = JsonUtility.FromJson<QuestionList>(questionsJSON.text);
    }
   
    public QuestionData RequestQuestionData(int index)
    {
        if(index<0) index = Mathf.Max(index, 0);
        else index = Mathf.Min(index, questionList.questions.Length - 1);

        return questionList.questions[index];
    }
}


[System.Serializable]
public class QuestionList
{
    public QuestionData[] questions;
}
[System.Serializable]
public class QuestionData
{
    public string category;
    public string question;
    public string[] choices;
    public string answer;
}