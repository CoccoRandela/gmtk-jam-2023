using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public class LevelData//Level needs moles, holes, hammers, coins, level speed(bpm)
    {
        public int HoleCount;
        public int MoleCount;
        public int HammerCount;
        public int CoinCount;
        public int LevelBPM;
    }

    [Header("Levels")] public LevelData[] LevelDatas;
    public int CurrentLevel;
    [Header("GamePlay Variables")] public int moleCount;
    
    
    [Header("Rest of the stuff")]
    
    public static GameManager Instance;

    public Hole[] holes;
    public List<KeyCode> usableKeys = new List<KeyCode>();
    private Dictionary<KeyCode,Hole> holeCodes = new Dictionary< KeyCode,Hole>();
    private Dictionary<Vector2,Hole> holePoss = new Dictionary< Vector2,Hole>();

    public GameObject holePrefab;
    
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

        CurrentLevel = 0;
    }

    private void Start()
    {
        LevelStart();
    }

    void LevelStart()//TODO: instantiate holes and moles looking up from a double array on each levelStart
    {
        CurrentLevel++;

        if (CurrentLevel == LevelDatas.Length)
        {
            GameStateManager.Instance.InvokeGameEndedEvent();
            return;
        }

        var holePosParent = GameObject.FindWithTag("HolePositions");
        //holePosParent.chil
        for (int i = 0; i < LevelDatas[i].HoleCount; i++)
        {
            //Instantiate(holePrefab)
        }
        
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
            molesInGame[i].transform.parent = holes[i].transform;
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
