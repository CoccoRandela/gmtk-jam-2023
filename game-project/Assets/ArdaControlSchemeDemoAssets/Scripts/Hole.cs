using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Vector2 holePosition;

    public enum Occupation
    {
        Free,
        Full,
        Coin
    }

    public Occupation occupationState;

    public KeyCode keyCode;

    public MoleController occupyingMole;
}
