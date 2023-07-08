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
        
        //instantiate 1 mole and set the hole and mole parameters
        for (int i = 0; i < moleCount; i++)
        {
            molesInGame.Add(Instantiate(molePrefabs[i], holes[i].transform.position, Quaternion.identity));
            holes[i].occupyingMole = molesInGame[i];
            molesInGame[i].currentHole = holes[i];
            holes[i].occupationState = Hole.Occupation.Full;
        }
    }
    
    public void OnKeyDownEvent(KeyCode keyCode)
    {
        if (InputManager.Instance.keyUps.Count > 0)
        {
            KeyCode holeToMoveFrom = InputManager.Instance.keyUps[0];
            if (InputManager.Instance.keyDowns.Count > 0)
            {
                KeyCode holeToMoveTo = InputManager.Instance.keyDowns[0];
                if (holeCodes[holeToMoveTo].occupationState == Hole.Occupation.Full ||
                    holeCodes[holeToMoveTo].occupationState == Hole.Occupation.Unusable)
                {
                    //holeCodes[holeToMoveFrom].GetComponent<SpriteRenderer>().color = Color.gray;
                    InputManager.Instance.keyDowns.Remove(holeToMoveTo);
                    return;
                }
                
                
                holeCodes[holeToMoveFrom].occupyingMole.MoveTo(holeCodes[holeToMoveTo]);
                holeCodes[holeToMoveTo].GetComponent<SpriteRenderer>().color = Color.black;
                InputManager.Instance.keyDowns.Remove(holeToMoveTo);
                Debug.Log("removing " + holeToMoveFrom + " wants to move " + holeToMoveTo);
                InputManager.Instance.keyUps.Remove(holeToMoveFrom);
            }
            else
            {
                Debug.Log("cant move " + holeToMoveFrom);
            }

            holeCodes[holeToMoveFrom].GetComponent<SpriteRenderer>().color = Color.gray;
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

    public Hole GetHoleFromHolePos(Vector2 holePos)
    {
        return holePoss[holePos];
    }
}
