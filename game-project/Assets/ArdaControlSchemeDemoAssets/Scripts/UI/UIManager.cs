using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public StartMenu startMenu;
    void Start()
    {
        GameStateManager.MenuStarted += ShowStartMenu; 
    }

    void ShowStartMenu()
    {
        startMenu.Fall();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
