using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    //Movement, Stun, Revive, Position

    public Hole currentHole;
    
    public Vector2 molePosition;

    private void Start()
    {
        //MoveTo(GameManager.Instance.holes[1]);
    }

    public void MoveTo(Hole holeToMoveTo)
    {
        if (holeToMoveTo.occupationState == Hole.Occupation.Full)
        {
            Debug.Log("the " + holeToMoveTo + " is full");
            return;
        }
        currentHole.occupationState = Hole.Occupation.Free;
        StartCoroutine(GoDown(holeToMoveTo.transform.position));
        holeToMoveTo.occupyingMole = this;
        holeToMoveTo.occupationState = Hole.Occupation.Full;
        currentHole = holeToMoveTo;

    }
    

    private IEnumerator GoDown(Vector3 nextPos)
    {
        Vector3 startPos = transform.position;
        while (true)
        {
            if (Vector2.Distance(startPos, transform.position) > .5)
            {
                break;
            }

            transform.position -= new Vector3(0,.5f,0);
            yield return null;
        }

        transform.position = nextPos - new Vector3(0,.5f, 0);
        StartCoroutine(GoUp());
    }
    
    private IEnumerator GoUp()
    {
        Vector2 startPos = transform.position;
        while (true)
        {
            if (Vector2.Distance(startPos, transform.position) > .5)
            {
                break;
            }

            transform.position += new Vector3(0,.5f ,0);
            yield return null;
        }

        Debug.Log("finito");
        
    }
}
