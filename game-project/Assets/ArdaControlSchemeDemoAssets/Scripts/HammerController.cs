using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using DG.Tweening;

public class HammerController : MonoBehaviour
{
    public AudioClip Smash;
    
    public Vector3 _restingPos;
    public GameObject shadow;
    public int bpm;

    [FormerlySerializedAs("difficulty")] public int playerWeight; //determines how possible it is that the hammer will attack an occupied hole
    public int coinWeight; //determines how possible it is that the hammer will attack a coin hole

    public float ticker;
    private void Awake()
    {
        shadow.transform.parent = null;
        _restingPos = transform.position;
    }

    void Update()
    {
        ticker += Time.deltaTime;

        if (ticker >= 60f/bpm)
        {
            ticker -= 60f/bpm;// recalibrates every beat(maybe bad idea?)
            ChooseHole();
        }
    }

    private void ChooseHole()
    {
        this.transform.DOKill();
        List<Vector2> holeList = new List<Vector2>();

        foreach (var hole in GameManager.Instance.holes)
        {
            if (hole.occupationState == Hole.Occupation.Unusable)
            {
                continue;
            }
            holeList.Add(hole.holePosition);
            if (hole.occupationState == Hole.Occupation.Full)//if its full, add it "difficulty" more times so there is more chance to hit that one
            {
                for (int i = 0; i < playerWeight; i++)
                {
                    holeList.Add(hole.holePosition);
                }
            }

            if (hole.occupationState == Hole.Occupation.Coin)
            {
                for (int i = 0; i < coinWeight; i++)
                {
                    holeList.Add(hole.holePosition);
                }
            }
        }

        var chosenHole = GameManager.Instance.GetHoleFromHolePos(holeList[Random.Range(0, holeList.Count)]);
        
        HammerGoDown(chosenHole);
        
       
    }

    private void HammerGoDown(Hole hole)
    {
        shadow.GetComponent<SpriteRenderer>().color = Color.white;
        shadow.transform.DOMove(hole.transform.position, 30f/bpm).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMoveX(hole.transform.position.x,  10f/bpm).SetEase(Ease.InQuad);
            transform.DOMoveY(hole.transform.position.y, 10f/bpm).SetEase(Ease.OutQuad).OnComplete(() =>
                CheckHit(hole)); 
        });
    }
    
    private void HammerGoUp()
    {
        shadow.transform.DOMove(_restingPos, 20f/bpm).SetEase(Ease.Linear);
        transform.DOMove(_restingPos, 20f/bpm).SetEase(Ease.Linear);
    }

    private void CheckHit(Hole hole)
    {
        SoundManager.Instance.PlayEffect(Smash);
        hole.Unpicked();
        GameManager.Instance.ShakeAround(hole);
        shadow.GetComponent<SpriteRenderer>().color = Color.clear;
        if (hole.occupationState == Hole.Occupation.Full)
        {
            hole.occupyingMole.Hit();
        }

        HammerGoUp();
    }
}
