using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public AudioClip gameMusic;
    
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

    public bool hasGameEnded;
    public bool hasGameStarted;

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
        GameStateManager.InvokeMenuStartedEvent();

        GameStateManager.GameEnded += OnGameEnded;
        GameStateManager.GameStarted += GameStart;
    }

    void OnGameEnded()
    {
        hasGameEnded = true;
        foreach (HammerController hammer in hammersInGame)
        {
            hammer.transform.DOKill();
            GameObject.Destroy(hammer.gameObject);
            hammerCount--;
        }

        holeCodes.Clear();
        usableKeys.Clear();
        holePosDict.Clear();

        foreach (MoleController mole in molesInGame)
        {
            GameObject.Destroy(mole.gameObject);
        }

        molesInGame.Clear();
        StartCoroutine("TakeAwayHoles");
    }

    void GameStart()
    {
        // GameStateManager.InvokeGameStartedEvent();
        
        SoundManager.Instance.PlayLoopingSound(gameMusic);

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

        for (int i = 0; i < moleCount; i++)
        {
            molesInGame.Add(Instantiate(molePrefabs[i], holes[i].transform.position, Quaternion.identity));
            holes[i].occupyingMole = molesInGame[i];
            molesInGame[i].currentHole = holes[i];
            holes[i].occupationState = Hole.Occupation.Full;
            molesInGame[i].transform.parent = holes[i].transform;
        }

        hasGameStarted = true;
    }

    private void SpawnHammer()
    {
        HammerController hammer = Instantiate(hammerPrefabs[hammerCount % hammerPrefabs.Count], hammerRestingPoints[hammerCount % hammerRestingPoints.Count]);
        hammersInGame.Add(hammer);
        hammer.bpm = StartingBPM;
        hammer.ticker = 0;
        hammerCount++;
    }

    public void RampUpHammerBpm()
    {
        StartingBPM += BPMRampUpAmount;
        foreach (var hammer in hammersInGame)
        {
            hammer.bpm += BPMRampUpAmount;
        }

        SoundManager.Instance._audioSource.pitch += 0.1f;
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

    private IEnumerator TakeAwayHoles()
    {
        int i = 0;
        while (true)
        {
            if (i == holes.Length) break;
            yield return new WaitForSeconds(.1f);
            var hole = holes[i];
            hole.MoveTo(new Vector3(holeWorldPositions[i].transform.position.x, 1000));
            i++;
        }
        GameStateManager.InvokeMenuEndedEvent();
    }

    public void OnKeyUpEvent(KeyCode keyCode)
    {
        if (holeCodes[keyCode].occupationState != Hole.Occupation.Full)
        {
            InputManager.Instance.keyUps.Remove(keyCode);
            return;
        }
        holeCodes[keyCode].Picked();
        holeCodes[keyCode].occupyingMole.WakeUp();
    }


    public void OnKeyDownEvent(KeyCode keyCode)
    {

        for (int i = 0; i < InputManager.Instance.keyUps.Count; i++)
        {
            KeyCode holeToMoveFrom = InputManager.Instance.keyUps[i];
            KeyCode holeToMoveTo = InputManager.Instance.keyDowns[i];
            holeCodes[holeToMoveFrom].occupyingMole.MoveTo(holeCodes[holeToMoveTo]);

            holeCodes[holeToMoveFrom].Unpicked();
            holeCodes[holeToMoveTo].Picked();

        }

        InputManager.Instance.ClearLists();
    }


    public Hole GetHoleFromHolePos(Vector2 holePos)
    {
        return holePosDict[holePos];
    }

    private void Update()
    {
        if (hasGameEnded) return;
        coinTick += Time.deltaTime;
        addHammerTick += Time.deltaTime;
        BPMRampUpTick += Time.deltaTime;

        if (coinTick >= coinTimer)
        {
            coinTick = 0;
            SpawnCoin();
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

        if (hasGameStarted)
        {
            foreach (MoleController mole in molesInGame)
            {
                if (!mole.isStunned)
                {
                    return;
                }
            }

            GameStateManager.InvokeGameEndedEvent();
        }
    }

    public void SpawnCoin()
    {
        foreach (var hole in holes)
        {
            if (hole.occupationState == Hole.Occupation.Free && Random.Range(0,5) == 2)
            {
                hole.coin = Instantiate(coinPrefab, hole.transform);
                hole.occupationState = Hole.Occupation.Coin;
                return;
            }
        }
    }

    public void ShakeAround(Hole hole)
    {
        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(-1, 0)))
        {
            holePosDict[hole.holePosition + new Vector2(-1, 0)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(-1, 0)]?.transform.DOShakePosition(0.3f,0.5f);
        }

        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(-1, -1)))
        {
            holePosDict[hole.holePosition + new Vector2(-1, -1)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(-1, -1)]?.transform.DOShakePosition(0.3f, 0.3f);
        }

        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(-1, 1)))

        {
            holePosDict[hole.holePosition + new Vector2(-1, 1)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(-1, 1)]?.transform.DOShakePosition(0.3f, 0.3f);
        }

        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(1, -1)))
        {
            holePosDict[hole.holePosition + new Vector2(1, -1)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(1, -1)]?.transform.DOShakePosition(0.3f, 0.3f); 
        }
        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(1, 1)))
        {
            holePosDict[hole.holePosition + new Vector2(1, 1)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(1, 1)]?.transform.DOShakePosition(0.3f, 0.3f); 
        }

        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(0, 0)))
        {
            holePosDict[hole.holePosition + new Vector2(0, 0)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(0, 0)]?.transform.DOShakePosition(0.3f, 0.5f);
        }

        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(0, -1)))
        {
            holePosDict[hole.holePosition + new Vector2(0, -1)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(0, -1)]?.transform.DOShakePosition(0.3f, 0.5f);
        }

        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(1, 0)))
        {
            holePosDict[hole.holePosition + new Vector2(1, 0)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(1, 0)]?.transform.DOShakePosition(0.3f, 0.5f);
        }
        if (holePosDict.ContainsKey(hole.holePosition + new Vector2(0, 1)))
        {
            holePosDict[hole.holePosition + new Vector2(0, 1)]?.transform.DORestart();
            holePosDict[hole.holePosition + new Vector2(0, 1)]?.transform.DOShakePosition(0.3f, 0.5f);
        }
    }
}
