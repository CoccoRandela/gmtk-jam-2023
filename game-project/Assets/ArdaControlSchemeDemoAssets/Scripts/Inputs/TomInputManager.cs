using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomInputManager : MonoBehaviour
{
    public Vector3 mouseStartPos;
    public Vector3 mouseEndPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseEndPos = Input.mousePosition;
            GameManager.Instance.MoveMoles(GetNormalizedMouseDrag(mouseStartPos, mouseEndPos));
        }
    }

    private Vector2 GetNormalizedMouseDrag(Vector3 start, Vector3 end)
    {
        var drag = end - start;

        var angle = Mathf.Rad2Deg * Mathf.Atan2(drag.y, drag.x);

        //primary directions 60 degrees/ secondary directions 30 degrees
        if (angle > -30 && angle < 30)
        {
            drag = new Vector3(1, 0, 0);
            Debug.Log("right");
        }
        else if (angle > -60 && angle < -30)
        {
            drag = new Vector3(1, -1, 0);

            Debug.Log("right bottom");

        }
        else if (angle > -120 && angle < -60)
        {
            drag = new Vector3(0, -1, 0);

            Debug.Log("bottom");

        }
        else if (angle > -150 && angle < -120)
        {
            drag = new Vector3(-1, -1, 0);

            Debug.Log("left bottom");

        }
        else if (angle > 150 || angle < -150)
        {
            drag = new Vector3(-1, 0, 0);

            Debug.Log("left");

        }
        else if (angle > 120 && angle < 150)
        {
            drag = new Vector3(-1, 1, 0);

            Debug.Log("left top");
            
        }
        else if (angle > 60 && angle < 120)
        {
            drag = new Vector3(0, 1, 0);

            Debug.Log("top");

        }
        else if (angle > 30 && angle < 60)
        {
            drag = new Vector3(1, 1, 0);

            Debug.Log("right top");
        }
        
        return drag;
    }
}