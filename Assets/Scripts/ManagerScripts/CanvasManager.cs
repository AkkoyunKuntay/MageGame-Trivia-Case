using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CanvasType { Menu, Game, Win }

public class CanvasManager : CustomSingleton<CanvasManager>
{
    [SerializeField] CanvasVisibilityController menuCanvas;
    [SerializeField] CanvasVisibilityController gameCanvas;
    [SerializeField] CanvasVisibilityController winCanvas;

    [Header("Debug")]
    [SerializeField] CanvasVisibilityController selectedCanvas;

    private void Start()
    {
        GameManager.instance.LevelSuccessEvent += OnLevelSuccessfull;
    }

    private void OnLevelSuccessfull()
    {
        SetCurrentCanvas(CanvasType.Win);
    }

    public void SetCurrentCanvas(CanvasType type)
    {
        selectedCanvas.Hide();

        switch (type)
        {
            case CanvasType.Menu:
                selectedCanvas = menuCanvas;
                break;
            case CanvasType.Game:
                selectedCanvas = gameCanvas;
                break;
            case CanvasType.Win:
                selectedCanvas = winCanvas;
                break;
            default:
                break;
        }

        selectedCanvas.Show();
    }
   
}
