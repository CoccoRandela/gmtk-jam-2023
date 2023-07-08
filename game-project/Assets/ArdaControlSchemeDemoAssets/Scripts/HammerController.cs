using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HammerController : MonoBehaviour
{
    private Vector3 _restingPos;
    public GameObject shadow;

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
        List<Vector2> HoleList = new List<Vector2>();

        foreach (var hole in GameManager.Instance.holes)
        {
            if (hole.occupationState == Hole.Occupation.Unusable)
            {
                continue;
            }
            HoleList.Add(hole.holePosition);
            if (hole.occupationState == Hole.Occupation.Full)//if its full, add it 3 more times so there is more chance to hit that one
            {
                HoleList.Add(hole.holePosition);
                HoleList.Add(hole.holePosition);
                HoleList.Add(hole.holePosition);
            }
        }

        var chosenHole = GameManager.Instance.GetHoleFromHolePos(HoleList[Random.Range(0, HoleList.Count)]);
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
