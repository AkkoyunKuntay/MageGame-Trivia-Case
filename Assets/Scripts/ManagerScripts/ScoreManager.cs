using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : CustomSingleton<ScoreManager>
{
    [Header("Point Settings")]
    [SerializeField] int correctPoint;
    [SerializeField] int wrongPoint;
    [SerializeField] int outOfTimePoint;

    [SerializeField] [Range(2,20)]float animationSpeed;


    [Header("References")]
    [SerializeField] TextMeshProUGUI in_gameScoreTXT;
    [SerializeField] TextMeshProUGUI winPanelScoreTXT;

    [Header("Debug")]
    public float score;
    [SerializeField] float targetScore;
    [SerializeField] float totalScore;
    [SerializeField] bool isAnimating;


    private void Start()
    {
        TimerController.instance.TimeIsUpEvent += OnTimeIsUp;
        ResponseManager.instance.SelectedOptionIsSuccesfullEvent += OnSelectionIsSuccessfull;
        ResponseManager.instance.SelectedOptionIsFailedEvent += OnSelectionIsFailed;
        GameManager.instance.LevelSuccessEvent += OnLevelFinished;
    }

    private void Update()
    {
        if (isAnimating)
        {
            score = Mathf.Lerp(score, targetScore, Time.deltaTime * animationSpeed);
            PlayerPrefs.SetFloat("score", targetScore);
            in_gameScoreTXT.text = "Score : " + Mathf.RoundToInt(score).ToString();

            float diff = Mathf.Abs(targetScore - score);
            if (diff <= 0.1f)
            {
                score = (int)targetScore;
                isAnimating = false;
            }
        }
    }
    private void OnLevelFinished()
    {
        SetTotalScore();
    }
    public int GetCurrentScore()
    {
        return (int)score;
    }
    public void ResetScore()
    {
        score = 0;
        targetScore = 0;
        in_gameScoreTXT.text = "Score : " + Mathf.RoundToInt(score).ToString();
    }
    private void OnSelectionIsFailed()
    {
        SetTargetScore(wrongPoint);
    }
    private void OnSelectionIsSuccessfull()
    {
        SetTargetScore(correctPoint);
    }
    private void OnTimeIsUp()
    {
        SetTargetScore(outOfTimePoint);
    }
    private void SetTargetScore(int parameter)
    {
        targetScore += parameter;
        isAnimating = true;
    }
    private void SetTotalScore()
    {
        totalScore += PlayerPrefs.GetFloat("score");
        winPanelScoreTXT.text = "TotalScore : " + totalScore;
    }
}
