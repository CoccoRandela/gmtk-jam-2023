using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Mole[] characters = new Mole[3];

    public Mole[] activeCharacters = new Mole[3];

    public Map map;

    private Mole selectedCharacter;

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

        int indexOne = 0;
        int indexTwo = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            activeCharacters[i] = Instantiate(characters[i], map.holes[indexOne, indexTwo].transform.position, map.holes[indexOne, indexTwo].transform.rotation);
            activeCharacters[i].currentMatrixIndex = (indexOne, indexTwo);
            Debug.Log(activeCharacters[i].selectionKeyCode + " " + activeCharacters[i].currentMatrixIndex);
            map.holes[indexOne, indexTwo].occupied = true;
            indexTwo++;
            if (indexTwo == map.columns)
            {
                indexTwo = 0;
                indexOne++;
            }
        }
    }

    void Update()
    {
        CheckIfCharIsSelected();
        CheckIfCharIsMoved();

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
                selectedCharacter.MoveBack();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                selectedCharacter.MoveForward();
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
                Debug.Log(selectedCharacter.selectionKeyCode);
            }
        }
    }
}
