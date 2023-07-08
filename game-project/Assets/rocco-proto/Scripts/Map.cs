using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int rows = 3;
    public int columns = 3;

    public Hole holePrefab;
    public Hole[,] holes;
    // Start is called before the first frame update

    void Awake()
    {
        holes = new Hole[rows, columns];

        float xPos = -3f;
        float yPos = 3f;

        for (int i = 0; i < rows; i++)
        {
            for (int y = 0; y < columns; y++)
            {
                holes[i, y] = Instantiate(holePrefab, new Vector2(xPos, yPos), this.gameObject.transform.rotation, this.gameObject.transform);
                xPos += 3f;
            }
            xPos = -3f;
            yPos -= 3f;
        }
    }
}
