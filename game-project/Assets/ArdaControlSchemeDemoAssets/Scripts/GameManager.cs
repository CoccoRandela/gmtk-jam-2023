using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Hole[] holes;
    private Dictionary<KeyCode,Hole> holeCodes = new Dictionary< KeyCode,Hole>();

    public MoleController[] molePrefabs = new MoleController[4];

    public MoleController[] molesInGame = new MoleController[4];

    private void Awake()
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

    
    void Start()
    {
        foreach (var hole in holes)
        {
            holeCodes.Add(hole.keyCode, hole);
        }

        foreach (var pair in holeCodes)
        {
            Debug.Log(pair.Key + "--" + pair.Value);
        }
        
        molesInGame[0] = Instantiate(molePrefabs[0], holes[0].transform.position, Quaternion.identity);
        holes[0].occupyingMole = molesInGame[0];
        
        //molesInGame[1] = Instantiate(molePrefabs[0], holes[1].transform.position, Quaternion.identity);
        //holes[1].occupyingMole = molesInGame[1];
    }

    
    public void OnKeyUpEvent(KeyCode keyCode)
    {
        
        
    }

    private bool first = true;
    public void OnKeyDownEvent(KeyCode keyCode)
    {
        if (first)
        {
            first = false;
            return;
        }
        KeyCode holeToMoveFrom = InputManager.Instance.keyUps[0];
        KeyCode holeToMoveTo = InputManager.Instance.keyDowns[0];
        
        Debug.Log("move from " + holeToMoveFrom + " to " + holeToMoveTo);

        holeCodes[holeToMoveFrom].occupyingMole.MoveTo(holeCodes[holeToMoveTo]);
        
        InputManager.Instance.ClearLists();
    }
}
