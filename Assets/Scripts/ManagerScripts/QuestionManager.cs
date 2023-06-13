using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionManager : CustomSingleton<QuestionManager>
{
    [Header("References")]
    [SerializeField] QuestionReader questionReader;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI categoryText;
    [SerializeField] TextMeshProUGUI[] choiseTexts;
    string[] choises;

    public System.Action QuestionChangedEvent;

    [Header("Debug")]
    [SerializeField] QuestionData questionData;
    [SerializeField] int index;

    protected override void Awake()
    {
        base.Awake();
        questionReader = GetComponent<QuestionReader>();
    }
    private void Start()
    {
        DisplayQuestionData();

        GamePanelController.instance.AllowNewQuestionEvent += OnNewQuestionAllowed;
    }
    public QuestionData GetCurrentQuestionData()
    {
        return questionData;
    }
    private void OnNewQuestionAllowed()
    {
        GetNewQuestion();
    }
    private void GetNewQuestion()
    {
        StartCoroutine(ChangeQuestionData());
    }
    private IEnumerator ChangeQuestionData()
    {
        if (index < questionReader.questionList.questions.Length)
        {
            index++;
            yield return new WaitForSeconds(0.5f);
            DisplayQuestionData();
            QuestionChangedEvent?.Invoke();
        } 
        else
        {
            GameManager.instance.EndGame(true);
            index = 0;
        }
    }
    private void DisplayQuestionData()
    {
        questionData = questionReader.RequestQuestionData(index);

        questionText.text = questionData.question.ToString();
        categoryText.text = questionData.category.ToString();

        for (int j = 0; j < choiseTexts.Length; j++)
        {
            string choise = questionData.choices[j].ToString();
            choiseTexts[j].text = choise;
        }
    }

   
   
}
