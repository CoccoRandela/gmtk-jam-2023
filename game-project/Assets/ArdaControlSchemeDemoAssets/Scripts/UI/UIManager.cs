using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public StartMenu startMenu;
    public GameObject scoreBoard;

    void Start()
    {
        GameStateManager.MenuStarted += ShowStartMenu;
        GameStateManager.GameStarted += RemoveStartMenu;
    }

    void ShowStartMenu()
    {
        startMenu.Fall();
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

}
