using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HammerController : MonoBehaviour
{
    private Vector3 _restingPos;
    public GameObject shadow;

    public int difficulty; //determines how possible it is that the hammer will attack an occupied hole

    private void Awake()
    {
        _restingPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("StartChooseHoleCoroutine",3, 2);
    }

    private void StartChooseHoleCoroutine()
    {
        StartCoroutine(ChooseHole());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ChooseHole()
    {
        shadow.transform.localPosition = Vector3.zero;
        shadow.transform.localScale = new Vector3(3, 3, 3);
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
                for (int i = 0; i < difficulty; i++)
                {
                    holeList.Add(hole.holePosition);
                }
            }
        }

        var chosenHole = GameManager.Instance.GetHoleFromHolePos(holeList[Random.Range(0, holeList.Count)]);
        var shadowStartPos = shadow.transform.position;
        var shadowStartScale = shadow.transform.localScale;

        float lerpT = 0;
        while (true)
        {
            lerpT += 1f * Time.deltaTime;
            shadow.transform.position = Vector3.Lerp(shadowStartPos, chosenHole.transform.position, lerpT);
            shadow.transform.localScale = Vector3.Lerp(shadowStartScale, new Vector3(1, 1, 1), lerpT);
            if (lerpT >= 1)
            {
                break;
            }
            yield return null;
        }

        if (chosenHole.occupationState == Hole.Occupation.Full)
        {
            chosenHole.occupyingMole.Hit();
        }
        
        yield return null;
    }
}
