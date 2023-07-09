using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Vector2 holePosition;

    public enum Occupation
    {
        Free,
        Full,
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
}
