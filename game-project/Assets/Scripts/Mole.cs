using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public KeyCode selectionKeyCode;

    public (int, int) currentMatrixIndex;

    public void MoveBack()
    {
        (int, int) newMatrixIndex = (currentMatrixIndex.Item1 - 1, currentMatrixIndex.Item2);
        if (newMatrixIndex.Item1 < 0 || newMatrixIndex.Item2 < 0) return;
        if (GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied) return;
        gameObject.transform.position = GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].transform.position;
        GameManager.Instance.map.holes[currentMatrixIndex.Item1, currentMatrixIndex.Item2].occupied = false;
        GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied = true;
        currentMatrixIndex = newMatrixIndex;
    }

    public void MoveForward()
    {
        Debug.Log(currentMatrixIndex);
        (int, int) newMatrixIndex = (currentMatrixIndex.Item1 + 1, currentMatrixIndex.Item2);
        if (newMatrixIndex.Item1 < 0 || newMatrixIndex.Item2 < 0) return;
        if (GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied) return;
        gameObject.transform.position = GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].gameObject.transform.position;
        GameManager.Instance.map.holes[currentMatrixIndex.Item1, currentMatrixIndex.Item2].occupied = false;
        GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied = true;
        currentMatrixIndex = newMatrixIndex;
    }
    public void MoveLeft()
    {
        Debug.Log(currentMatrixIndex);
        (int, int) newMatrixIndex = (currentMatrixIndex.Item1, currentMatrixIndex.Item2 - 1);
        if (newMatrixIndex.Item1 < 0 || newMatrixIndex.Item2 < 0) return;
        if (GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied) return;
        gameObject.transform.position = GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].gameObject.transform.position;
        GameManager.Instance.map.holes[currentMatrixIndex.Item1, currentMatrixIndex.Item2].occupied = false;
        GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied = true;
        currentMatrixIndex = newMatrixIndex;
    }
    public void MoveRight()
    {
        Debug.Log(currentMatrixIndex);
        (int, int) newMatrixIndex = (currentMatrixIndex.Item1, currentMatrixIndex.Item2 + 1);
        if (newMatrixIndex.Item1 < 0 || newMatrixIndex.Item2 < 0) return;
        if (GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied) return;
        gameObject.transform.position = GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].gameObject.transform.position;
        GameManager.Instance.map.holes[currentMatrixIndex.Item1, currentMatrixIndex.Item2].occupied = false;
        GameManager.Instance.map.holes[newMatrixIndex.Item1, newMatrixIndex.Item2].occupied = true;
        currentMatrixIndex = newMatrixIndex;
    }



}
