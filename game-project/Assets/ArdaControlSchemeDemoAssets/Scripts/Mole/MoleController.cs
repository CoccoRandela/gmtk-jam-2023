using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    //Movement, Stun, Revive, Position

    public Hole currentHole;
    
    public Vector2 molePosition;

    public bool isDead;

    private void Start()
    {
        //MoveTo(GameManager.Instance.holes[1]);
    }

    public void MoveTo(Hole holeToMoveTo)
    {
        if (isDead)
        {
            Debug.Log("this man dedd");
            return;
        }
        
        if (holeToMoveTo.occupationState == Hole.Occupation.Full || holeToMoveTo.occupationState == Hole.Occupation.Unusable
            )
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

            transform.position -= new Vector3(0,5f * Time.deltaTime,0);
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

            transform.position += new Vector3(0,5f * Time.deltaTime,0);
            yield return null;
        }
    }

    public void Hit()
    {
        Debug.Log(name + " got hit");
        currentHole.occupationState = Hole.Occupation.Unusable;
        isDead = true;
        GetComponent<SpriteRenderer>().color = Color.black;
    }
}
