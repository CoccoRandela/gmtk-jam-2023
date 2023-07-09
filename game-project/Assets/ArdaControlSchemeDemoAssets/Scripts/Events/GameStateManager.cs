using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public static event Action MenuStarted;
    /*
     * Not sure if this is necessary, we can just use start method of the menu scene for anything we may do here
     */

    public static event Action GameStarted;
    /*
     * -On event "GameStarted"
     * the very first level starts, in this level, LevelStarted will not be invoked so we can do
     * tutorial only stuff with this event
     * Level Ended will be invoked after the tutorial ends to show stats or the next level button or whatnot
     */



    public static event Action LevelStarted;
    /*
    -On event “LevelStarted”,
    The First non tutorial level starts
   1) The map is instantiated with a cool tween/ each block comes from bottom and settles with an elastic tween
   2) The moles are placed both sleeping/ when you pick one, it wakes up, when you pick the other, it sleeps
   3) The hammer starts going:
   There is an area, representing the possible hammers hits, which decreases with time to signal 
   the players where the hammer is going to probably hit. The area should be colored because at a 
   point, there is going to be another hammer and (maybe) it would be nice to differentiate between
    the areas (but maybe not)
   4) A coin gets placed on each run of the hammer in a location without moles
   */

    public static event Action GamePaused;
    public static event Action GameResumed;


    public static event Action LevelEnded;
    /*
     * Both Moles start sleeping, Hammers stop, points shown, next level button shown.
     */

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
    public static void InvokeGameStartedEvent()
    {
        GameStarted?.Invoke();
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
