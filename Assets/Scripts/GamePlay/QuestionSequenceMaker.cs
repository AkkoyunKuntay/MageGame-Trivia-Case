using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSequenceMaker : MonoBehaviour
{
    private void Start()
    {
        GameFlowManager.instance.RefreshGamePanelEvent += OnRefreshPanel;
    }

    private void OnRefreshPanel()
    {
        
        AnimateAllOptionButtons();
    }
    private void AnimateAllOptionButtons()
    {
        StartCoroutine(ScaleInOrder());
    }
    private IEnumerator ScaleInOrder()
    {
        List<OptionController> optionsList = ResponseManager.instance.GetChoiseList();
        for (int i = 0; i < optionsList.Count; i++)
        {
            OptionController option = optionsList[i];
            option.ScaleButtonUp();
            yield return new WaitForSeconds(.1f);
        }
    }
}
