using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Vector2 holePosition;
    public SpriteRenderer PickIndicator;
    public GameObject coin;

    public enum Occupation
    {
        Free,
        Full,
        Coin,
        Unusable
    }

    public Occupation occupationState;

    public KeyCode keyCode;

    public MoleController occupyingMole;

    public AnimationCurve animationCurve;
    public void MoveTo(Vector3 pos)
    {
        transform.DOMove(pos, 2).SetEase(animationCurve);
    }

    public void Picked()
    {
        PickIndicator.enabled = true;
    }

    public void Unpicked()
    {
        PickIndicator.enabled = false;
    }
}
