using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public StartMenu startMenu;
    public EndMenu endMenu;
    public GameObject scoreBoard;

    void Start()
    {
        GameStateManager.MenuStarted += ShowStartMenu;
        GameStateManager.GameStarted += RemoveStartMenu;
        GameStateManager.GameEnded += PassScoreToEndMenu;
        GameStateManager.MenuEnded += ShowEndMenu;
    }

    void ShowStartMenu()
    {
        startMenu.Fall();
    }

    void ShowEndMenu()
    {
        endMenu.GoUp();
    }

    void RemoveStartMenu()
    {
        Debug.Log("Start Menu");
        startMenu.Remove(ShowScoreBoard);
    }
    public void ShowScoreBoard()
    {
        scoreBoard.SetActive(true);
    }

    public void PassScoreToEndMenu()
    {
        endMenu.scoreText.text = scoreBoard.GetComponent<ScoreBoard>().scoreText.text;
    }

}
