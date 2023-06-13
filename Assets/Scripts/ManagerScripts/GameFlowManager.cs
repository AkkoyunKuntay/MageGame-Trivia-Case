using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : CustomSingleton<GameFlowManager>
{
    public event System.Action RefreshGamePanelEvent;

    private void Start()
    {
        ResponseManager.instance.AnOptionSelectedEvent += OnOptionSelected;
        TimerController.instance.TimeIsUpEvent += OnTimeIsUp;

        GamePanelController.instance.RefresGamePanel();
    }

   

    private void OnTimeIsUp()
    {
        StartCoroutine(RefreshGamePanel());
    }

    private void OnOptionSelected()
    {
        StartCoroutine(RefreshGamePanel());
    }

    IEnumerator RefreshGamePanel()
    {
        yield return new WaitForSeconds(2);
        GamePanelController.instance.RefresGamePanel();
        yield return new WaitForSeconds(.75F);
        RefreshGamePanelEvent?.Invoke();
    }
}
