using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public static event Action MenuStarted;

    public static event Action MenuEnded;
    /*
     * Not sure if this is necessary, we can just use start method of the menu scene for anything we may do here
     */

    public static event Action GameStarted;


    public static event Action GameRestarted;
    /*
     * -On event "GameStarted"
     * the very first level starts, in this level, LevelStarted will not be invoked so we can do
     * tutorial only stuff with this event
     * Level Ended will be invoked after the tutorial ends to show stats or the next level button or whatnot
     */





    public static event Action GamePaused;
    public static event Action GameResumed;

    public static event Action GameEnded;
    /*
     * All the lives are finished or all moles are hit. Final score is shown, restart/menu/exit is shown. 
     */

    public static event Action CoinCollected;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void InvokeMenuStartedEvent()
    {
        MenuStarted?.Invoke();
    }

    public static void InvokeMenuEndedEvent()
    {
        MenuEnded?.Invoke();
    }
    public static void InvokeGameStartedEvent()
    {
        GameStarted?.Invoke();
    }
    public static void InvokeGameRestartedEvent()
    {
        GameRestarted?.Invoke();
    }
    public static void InvokeGamePausedEvent()
    {
        GamePaused?.Invoke();
    }
    public static void InvokeGameResumedEvent()
    {
        GameResumed?.Invoke();
    }
    public static void InvokeGameEndedEvent()
    {
        GameEnded?.Invoke();
    }

    public static void InvokeCoinCollected()
    {
        CoinCollected?.Invoke();
    }

}
