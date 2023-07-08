using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int rows = 3;
    public int columns = 3;

    public Hole holePrefab;
    public Hole[] holes;
    // Start is called before the first frame update

    void Awake()
    {
        for (int i = 0; i < rows * columns; i++)
        {
            holes[i] = Instantiate(holePrefab, this.gameObject.transform);
        }
    }
}
