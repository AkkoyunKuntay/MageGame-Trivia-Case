using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    [SerializeField] CanvasVisibilityController boardPopup;
    [SerializeField] Button leaderBoardButton;
    [SerializeField] Button exitLbButton;

    private void Start()
    {
        GameManager.instance.LevelStartedEvent += OnGameStarted;
    }

    private void OnGameStarted()
    {
        CanvasManager.instance.SetCurrentCanvas(CanvasType.Game);
    }

    public void OnLeaderBoardButtonPressed()
    {
        boardPopup.Show();
    }
    public void OnExitLbButtonPressed()
    {
        boardPopup.Hide();
    }
    public void OnHomeButtonPressed()
    {
        CanvasManager.instance.SetCurrentCanvas(CanvasType.Menu);
        ScoreManager.instance.ResetScore();
    }
}
