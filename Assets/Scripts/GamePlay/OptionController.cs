using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class OptionController : MonoBehaviour 
{

    [Header("References")]
    public Transform cradle;

    [Header("Animation Settings")]
    public float magnitude;
    public float shakeDuration;
    private Vector3 originalScale;

    [Space(20)]
    [Header("Settings")]
    public string optionAnswer;
    public Button button;
    public Color defaultColor;

    public event System.Action<OptionController> OptionSelectedEvent;
    private void Awake()
    {
        button = GetComponent<Button>();
        defaultColor = button.targetGraphic.color;
        originalScale = transform.localScale;
    }
    private void Start()
    {
        QuestionManager.instance.QuestionChangedEvent += OnQuestionChanged;
        GameFlowManager.instance.RefreshGamePanelEvent += OnPanelRefreshed;
    }

  

    public void OnButtonClicked()
    {
        if (!TimerController.instance.IsCounting()) return;

        TimerController.instance.StopTimer();
        OptionSelectedEvent?.Invoke(this);
    }
    public void ScaleButtonUp()
    {
        transform.DOScale(originalScale, 0.35f).From(Vector3.zero);
    }
    public void SetButtonColor(Color color)
    {
        button.targetGraphic.color = color;
    }
    public void AnimateButton(bool isSuccesfull)
    {
        if (isSuccesfull) cradle.DOScale(Vector3.one / 1.2f, 0.15f).From(Vector3.one).SetLoops(2, LoopType.Yoyo);
        else StartCoroutine(Shake(cradle, shakeDuration, magnitude));
    }
    private IEnumerator Shake(Transform buttonCradle, float duration, float magnitude)
    {
        Vector3 originalPos = buttonCradle.localPosition;
        float elapsed = 0;
        while (elapsed < duration)
        {
            float x = Random.Range(-5, 5) * magnitude;

            buttonCradle.localPosition = new Vector3(x, originalPos.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        buttonCradle.localPosition = originalPos;
    }
    private void ResetButton()
    {
        transform.localScale = Vector3.zero;
        button.targetGraphic.color = defaultColor;
    }

    #region Events
    private void OnQuestionChanged()
    {
        ResetButton();
    }
    private void OnPanelRefreshed()
    {
        ResetButton();
    }
    #endregion
}
