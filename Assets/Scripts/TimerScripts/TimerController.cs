using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : CustomSingleton<TimerController>
{
    [Header("Settings")]
    [SerializeField] float startingTime;
    [SerializeField] float currentTime;
    [SerializeField] Image fillerImg;

    [Header("References")]
    public TextMeshProUGUI timerTXT;

    public event System.Action TimeIsUpEvent;

    [Header("Debug")]
    [SerializeField] bool counting;

    protected override void Awake()
    {
        base.Awake();
        currentTime = startingTime;
        
    }
    private void Start()
    {
        StartTimer();
        timerTXT.text = ((int)(currentTime)).ToString();
        QuestionManager.instance.QuestionChangedEvent += OnQuestionChanged;
    }

    private void Update()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (!counting) return;

        CountDownTimer();
    }
    public void StopTimer()
    {
        counting = false;
    }
    public void ResetTimer()
    {
        currentTime = startingTime;
        fillerImg.fillAmount = currentTime / startingTime;
        timerTXT.text = ((int)(currentTime)).ToString();
    }
    public bool IsCounting()
    {
        return counting;
    }
    public float GetCurrentTimer()
    {
        return currentTime;
    }
    private void OnQuestionChanged()
    {
        StartCoroutine(RestartTimer());
    }
    private IEnumerator RestartTimer()
    {
        ResetTimer();
        yield return new WaitForSeconds(2);
        StartTimer();
    }
    private void CountDownTimer()
    {
        currentTime -= Time.deltaTime;
        float t = currentTime / startingTime;
        fillerImg.fillAmount = t;
        timerTXT.text = ((int)(currentTime)).ToString();
        if (currentTime <= 0)
        {
            StopTimer();
            TimeIsUpEvent?.Invoke();
        }
        
    }
    private void StartTimer()
    {
        counting = true;
    }
  
}
