using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("GamePlay Variables")] public int moleCount;
    
    public static GameManager Instance;

    public Hole[] holes;
    public List<KeyCode> usableKeys = new List<KeyCode>();
    private Dictionary<KeyCode,Hole> holeCodes = new Dictionary< KeyCode,Hole>();
    private Dictionary<Vector2,Hole> holePoss = new Dictionary< Vector2,Hole>();

    public List<MoleController> molePrefabs = new List<MoleController>();

    public List<MoleController> molesInGame = new List<MoleController>();

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
            holePoss.Add(hole.holePosition, hole);
            usableKeys.Add(hole.keyCode);
        }
        
        //TODO Change molePrefabs[0] to molePrefabs[i] when the different moles are here
        for (int i = 0; i < moleCount; i++)
        {
            molesInGame.Add(Instantiate(molePrefabs[0], holes[i].transform.position, Quaternion.identity));
            holes[i].occupyingMole = molesInGame[i];
            molesInGame[i].currentHole = holes[i];
            holes[i].occupationState = Hole.Occupation.Full;
        }
    }
    

    public void OnKeyUpEvent(KeyCode keyCode)
    {
        if (holeCodes[keyCode].occupationState != Hole.Occupation.Full)
        {
            InputManager.Instance.keyUps.Remove(keyCode);
            return;
        }
        holeCodes[keyCode].GetComponent<SpriteRenderer>().color = Color.black;
        
    }

    public void OnKeyDownEvent(KeyCode keyCode)
    {
        
        for (int i = 0; i < InputManager.Instance.keyUps.Count; i++)
        {
            KeyCode holeToMoveFrom = InputManager.Instance.keyUps[i];
            KeyCode holeToMoveTo = InputManager.Instance.keyDowns[i];
            holeCodes[holeToMoveFrom].occupyingMole.MoveTo(holeCodes[holeToMoveTo]);

            holeCodes[holeToMoveFrom].GetComponent<SpriteRenderer>().color = Color.gray;
            holeCodes[holeToMoveTo].GetComponent<SpriteRenderer>().color = Color.black;

        }

        InputManager.Instance.ClearLists();
    }


    public Hole GetHoleFromHolePos(Vector2 holePos)
    {
        return holePoss[holePos];
    }
}
