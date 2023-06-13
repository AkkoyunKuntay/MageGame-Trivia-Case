using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseManager : CustomSingleton<ResponseManager>
{
    [Header("Properties")]
    [SerializeField] List<OptionController> choiseList;
    [SerializeField] Transform choiseContainer;
    [SerializeField] GameObject blockerPanel;

    [Header("Debug")]
    [SerializeField] OptionController trueAnswer;
    public bool isAnswerGiven;

    public event System.Action AnOptionSelectedEvent;
    public event System.Action SelectedOptionIsSuccesfullEvent;
    public event System.Action SelectedOptionIsFailedEvent;

    protected override void Awake()
    {
        base.Awake();
        InitializeList();
        SubscibeToOptionEvents();
    }
    private void Start()
    {
        QuestionManager.instance.QuestionChangedEvent += OnQuestionChanged;
        TimerController.instance.TimeIsUpEvent += OnTimeIsUp;
    }
    public List<OptionController> GetChoiseList()
    {
        return choiseList;
    }
    private void SetBlockerPanelStatus(bool isActive)
    {
        if (isActive) blockerPanel.SetActive(true);
        else blockerPanel.SetActive(false);
    }
    private void InitializeList()
    {
        for (int i = 0; i < choiseContainer.childCount; i++)
        {
            OptionController choise = choiseContainer.GetChild(i).GetComponent<OptionController>();
            if (!choiseList.Contains(choise))
                choiseList.Add(choise);
        }
    }
    private void SubscibeToOptionEvents()
    {
        foreach (OptionController option in choiseList)
        {
            option.OptionSelectedEvent += OnOptionSelected;
        }
    }  
    private OptionController FindTrueAnswer()
    {
        QuestionData question = QuestionManager.instance.GetCurrentQuestionData();
        for (int i = 0; i < choiseList.Count; i++)
        {
            if(choiseList[i].optionAnswer == question.answer)
            {
                trueAnswer = choiseList[i];
                return choiseList[i];
            }
        }
        return null;
    }
    private bool CheckIsGivenAnswerCorrect(string answer)
    {
        QuestionData question = QuestionManager.instance.GetCurrentQuestionData();
        return answer == question.answer;
    }

    #region Events
    private void OnOptionSelected(OptionController option)
    {
        SetBlockerPanelStatus(true);
        AnOptionSelectedEvent?.Invoke();
        isAnswerGiven = true;
        if (CheckIsGivenAnswerCorrect(option.optionAnswer))
        {
            option.SetButtonColor(Color.green);
            SelectedOptionIsSuccesfullEvent?.Invoke();
            option.AnimateButton(true);
        }
        else
        {
            OptionController trueChoise = FindTrueAnswer();
            option.SetButtonColor(Color.red);
            trueChoise.SetButtonColor(Color.green);
            SelectedOptionIsFailedEvent?.Invoke();
            option.AnimateButton(false);
        }  
    }
    private void OnTimeIsUp()
    {
        SetBlockerPanelStatus(true);
        OptionController trueChoise = FindTrueAnswer();
        trueChoise.SetButtonColor(Color.green);
    }
    private void OnQuestionChanged()
    {
        SetBlockerPanelStatus(false);
    }

    #endregion
}
