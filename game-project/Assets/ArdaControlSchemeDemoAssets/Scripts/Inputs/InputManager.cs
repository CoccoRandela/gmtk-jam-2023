using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public List<KeyCode> keyUps = new List<KeyCode>();

    public List<KeyCode> keyDowns = new List<KeyCode>();

    public UnityEvent<KeyCode> keyUpEvent;
    public UnityEvent<KeyCode> keyDownEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this); 
        }
    }


    public List<KeyCode> m_activeInputs = new List<KeyCode>();

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ClearLists();
            return;
        }
        
        List<KeyCode> pressedInput = new List<KeyCode>();

        if (Input.anyKey)
        {
            foreach(KeyCode code in GameManager.Instance.usableKeys)
            {
                if (Input.GetKey(code))
                {
                    pressedInput.Add( code );
                    if ( !keyDowns.Contains(code) )
                    {
                        Debug.Log("code " + code);
                        m_activeInputs.Remove( code ); 
                        m_activeInputs.Add( code ); 
                        
                        AddToKeyDowns(code);
                    }
                }
            }
        }

        List<KeyCode> releasedInput = new List<KeyCode>();

        foreach(KeyCode code in m_activeInputs)
        {
            releasedInput.Add( code );

            if(!pressedInput.Contains(code))
            {
                releasedInput.Remove( code );
                AddToKeyUps(code);
            }
        }

        m_activeInputs = releasedInput;
    }

    private void AddToKeyUps(KeyCode keyCode)
    {
        if (keyUps.Count == 0 || !keyUps.Contains(keyCode))
        {
            Debug.Log("have keyupEvent for " + keyCode);
            keyUps.Add(keyCode);
            keyDowns.Remove(keyCode);
            keyUpEvent.Invoke(keyCode);
        }
    }

    private void AddToKeyDowns(KeyCode keyCode)
    {
        if (keyDowns.Count != 0 && keyDowns.Contains(keyCode)) return;
        //Debug.Log("have keydownevent for " + keyCode);
        keyDowns.Add(keyCode);
        keyUps.Remove(keyCode);
        keyDownEvent.Invoke(keyCode);
    }

    public void ClearLists()
    {
        keyUps.Clear();
        keyDowns.Clear();
    }
}
