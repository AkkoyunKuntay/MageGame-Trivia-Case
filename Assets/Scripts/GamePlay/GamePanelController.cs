using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : CustomSingleton<GamePanelController>
{
    private Vector3 originalPos;
    private Vector3 startingPos;
    private Vector3 targetPos;

    public event System.Action AllowNewQuestionEvent;

    protected override void Awake()
    {
        base.Awake();
        originalPos = transform.position;
        targetPos = new Vector3(originalPos.x + 2000, originalPos.y, originalPos.z);
        startingPos = new Vector3(originalPos.x - 2000, originalPos.y , originalPos.z);
        transform.position = startingPos;
    }
    public void RefresGamePanel()
    {
        StartCoroutine(AnimateUI());    
    }

    private IEnumerator AnimateUI()
    {
        transform.DOMove(originalPos, 1f).From(startingPos);
   
        yield return new WaitForSeconds(1f);
        ResponseManager.instance.SetBlockerPanelStatus(false);
        yield return new WaitUntil(() =>
        {
            return ResponseManager.instance.isAnswerGiven || TimerController.instance.GetCurrentTimer() <= 0;
        });
        ResponseManager.instance.isAnswerGiven = false;

        yield return new WaitForSeconds(1f); 
        transform.DOMove(targetPos, 1f).From(originalPos).OnComplete(()=> 
        {

            TimerController.instance.ResetTimer();
        });
        yield return new WaitForSeconds(0.5f);
        
        AllowNewQuestionEvent?.Invoke();

    }
}
