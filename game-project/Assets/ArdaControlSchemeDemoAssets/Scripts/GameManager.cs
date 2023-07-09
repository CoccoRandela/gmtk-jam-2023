using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("GamePlay Variables")] public int moleCount;
    //if we have time implement these
    //public int startingHoleCount;
    //public bool shouldIncreaseHoles;
    //public int increaseHoleTime;//every "increaseHoleTime" seconds, one more hole will be added
    public int startingHammerCount;
    public float addHammerTimer;
    private float addHammerTick;
    public float coinTimer;//1 coin every this seconds
    private float coinTick;
    [FormerlySerializedAs("LevelBPM")] public int StartingBPM;//60 = hammers hit once every second, 120 means 2 hits every second and so on
    public float BPMRampUpTimer;//ramp up the bpm every x seconds 
    private float BPMRampUpTick;
    public int BPMRampUpAmount;

    [Header("Rest of the stuff")]

    public static GameManager Instance;

    public Hole[] holes;
    public List<KeyCode> usableKeys = new List<KeyCode>();
    private Dictionary<KeyCode, Hole> holeCodes = new Dictionary<KeyCode, Hole>();
    private Dictionary<Vector2, Hole> holePosDict = new Dictionary<Vector2, Hole>();//NOT THE ACTUAL WORLD POS, RELATIVE POS

    public List<GameObject> holeWorldPositions = new List<GameObject>();

    private List<MoleController> molesInGame = new List<MoleController>();
    private List<HammerController> hammersInGame = new List<HammerController>();
    [Header("Prefabs")]
    public List<MoleController> molePrefabs = new List<MoleController>();
    public List<HammerController> hammerPrefabs = new List<HammerController>();
    public List<Transform> hammerRestingPoints = new List<Transform>();
    public GameObject coinPrefab;

    public bool isMoving;//I know this is bad shut up its a game jam -Arda

    private int hammerCount;
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

    private void Start()
    {
        //GameStateManager.InvokeMenuStartedEvent();
        GameStateManager.InvokeMenuStartedEvent();//this is temporary change this to start when startgame is pressed or hook this up to the gamestarted event and invoke it with startgame button
        GameStateManager.GameStarted += GameStart;
    }

    void GameStart()
    {
        // GameStateManager.InvokeGameStartedEvent();

        for (int i = 0; i < startingHammerCount; i++)
        {
            SpawnHammer();
        }

        for (var i = 0; i < holes.Length; i++)
        {
            var hole = holes[i];
            holeCodes.Add(hole.keyCode, hole);
            holePosDict.Add(hole.holePosition, hole);
            usableKeys.Add(hole.keyCode);
        }

        StartCoroutine("BringHoles");

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

    private void SpawnHammer()
    {
        HammerController hammer = Instantiate(hammerPrefabs[Random.Range(0, hammerPrefabs.Count)], hammerRestingPoints[hammerCount % hammerRestingPoints.Count]);
        hammersInGame.Add(hammer);
        hammer.bpm = StartingBPM;
        hammerCount++;
    }

    public void RampUpHammerBpm()
    {
        StartingBPM += BPMRampUpAmount;
        foreach (var hammer in hammersInGame)
        {
            hammer.bpm += BPMRampUpAmount;
        }
    }

    private IEnumerator BringHoles()
    {
        int i = 0;
        while (true)
        {
            if (i == holes.Length) break;
            yield return new WaitForSeconds(.1f);
            var hole = holes[i];
            hole.MoveTo(holeWorldPositions[i].transform.position);
            i++;
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
        holeCodes[keyCode].occupyingMole.WakeUp();
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
        return holePosDict[holePos];
    }

    private void Update()
    {
        coinTick += Time.deltaTime;
        addHammerTick += Time.deltaTime;
        BPMRampUpTick += Time.deltaTime;

        if (coinTick >= coinTimer)
        {
            coinTick = 0;
            //spawn coin on a free tile
        }

        if (addHammerTick >= addHammerTimer)
        {
            addHammerTick = 0;
            SpawnHammer();
        }

        if (BPMRampUpTick > BPMRampUpTimer)
        {
            BPMRampUpTick = 0;
            RampUpHammerBpm();
        }

        isMoving = false;
        foreach (var mole in molesInGame)
        {
            if (mole.isMoving) isMoving = true;
        }

    }
}
