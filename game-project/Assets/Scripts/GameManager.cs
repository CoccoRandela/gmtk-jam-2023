using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance;

    public Mole[] characters = new Mole[4];

    public Mole[] activeCharacters = new Mole[4];

    private Mole selectedCharacter;

    public Hole[] holes = new Hole[9];

    void Awake()
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

    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            activeCharacters[i] = Instantiate(characters[i], holes[i].transform.position, holes[i].transform.rotation);
            holes[i].occupied = true;
        }
    }

    void Update()
    {
        CheckIfCharIsSelected();

    }

    void CheckIfCharIsSelected()
    {
        KeyCode[] possibleKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        foreach (KeyCode key in possibleKeys)
        {
            if (Input.GetKeyDown(key))
            {
                SelectCharacter(key);
            }
        }
    }

    void CheckIfCharIsMoved()
    {
        if (selectedCharacter)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                selectedCharacter.MoveUp();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                selectedCharacter.MoveDown();
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                selectedCharacter.MoveLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                selectedCharacter.MoveRight();
        }
    }

    void SelectCharacter(KeyCode keypressed)
    {
        foreach (Mole character in activeCharacters)
        {
            if (character.selectionKeyCode == keypressed)
            {
                selectedCharacter = character;
            }
        }
    }
}
