using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoleController : MonoBehaviour
{
    //Movement, Stun, Revive, Position

    public Animator animator;
    
    public Hole currentHole;
    
    public Vector2 molePosition;

    public bool isMoving;
    
    [FormerlySerializedAs("isDead")] public bool isStunned;

    private void Start()
    {
        animator.SetBool("isSleeping", true);
        //MoveTo(GameManager.Instance.holes[1]);
    }

    public void MoveTo(Hole holeToMoveTo)
    {
        if (isStunned)
        {
            return;
        }

        if (isMoving)
        {
            return;
        }
        
        if (holeToMoveTo.occupationState == Hole.Occupation.Full || holeToMoveTo.occupationState == Hole.Occupation.Unusable)
        {
            Debug.Log("the " + holeToMoveTo + " is full");
            return;
        }

        isMoving = true;
        holeToMoveTo.occupationState = Hole.Occupation.Full;
        currentHole.occupationState = Hole.Occupation.Free;
        StartCoroutine(GoDown(holeToMoveTo.transform.position));
        holeToMoveTo.occupyingMole = this;
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

            transform.position -= new Vector3(0,3f * Time.deltaTime,0);
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

            transform.position += new Vector3(0,3f * Time.deltaTime,0);
            yield return null;
        }

        Sleep();
        isMoving = false;
    }

    public void Hit()
    {
        animator.SetTrigger("isStunned");
        animator.SetBool("isSleeping", true);
        currentHole.occupationState = Hole.Occupation.Unusable;
        currentHole.Unpicked();
        isStunned = true;
    }


    public void WakeUp()
    {
        animator.SetBool("isSleeping", false);
    }

    public void Sleep()
    {
        animator.SetBool("isSleeping", true);
    }
}
