using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class LettersSelection : MonoBehaviour
{
    //private const string letterSprite = "letters_v3_";
    private const float LETTER_SPACING = 1.25f;
    private const float LETTER_MOVE_POINT = 0f;

    private KeyCode[] allKeys;

    public string givenLetters;

    /// <summary>
    /// Displayed letters available for selection, this can be jumbled/shuffled via shuffle command
    /// </summary>
    [SerializeField]
    private List<char> displayedLetters;

    /// <summary>
    /// This are the typed letters from the displayed. Whatever is typed, should be placed here from the displayedLetters array
    /// </summary>
    [SerializeField]
    private List<char> typedLetters;

    /// <summary>
    /// This is the letter object that will be used to contain the sprites.
    /// </summary>
    [SerializeField]
    private LetterObj letterObj;

    /// <summary>
    /// This are the sprites used for the individual letters
    /// </summary>
    [SerializeField]
    private Sprite[] letterSprites;

    /// <summary>
    /// This contains the valid letters (that is available for selection)
    /// </summary>
    private List<KeyCode> validKeys = new();


    [SerializeField]
    private List<LetterObj> letterDisplay = new();


    private void Start()
    {
        // Initial loading of all KeyCodes (Saving to memory)
        allKeys = (KeyCode[])Enum.GetValues(typeof(KeyCode));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShuffleLetters();
        }

        if (Input.anyKeyDown)
        {
            foreach (var key in validKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    TypeLetter(key);
                }
            }
        }

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (var letter in typedLetters)
        {
            LetterObj obj = letterDisplay.FirstOrDefault(l => l.GetChar() == letter && !l.isTyped);

            if (obj != null)
            {
                obj.isTyped = true;
                Vector3.Lerp(obj.transform.position, new Vector3(obj.transform.position.x, obj.transform.position.y - 1f), 1f);
            }


            //Vector3.MoveTowards(obj.transform.position, new Vector3(obj.transform.position.x, obj.transform.position.y - 1f), 0.1f);
        }

        foreach (var letter in displayedLetters)
        {
            LetterObj obj = letterDisplay.Find(l => l.GetChar() == letter && l.isTyped);

            if (obj != null)
            {
                if (obj.transform.position != obj.displayPosition)
                {
                    obj.isTyped = false;
                    Vector3.Lerp(obj.transform.position, obj.displayPosition, 1f);
                }
            }
        }
    }

    private void TypeLetter(KeyCode key)
    {
        char typedKey = key.ToString().ToLower()[0];

        char letterChar = displayedLetters.FirstOrDefault(c => c == typedKey);

        if (letterChar != char.MinValue)
        {
            displayedLetters.RemoveAt(displayedLetters.IndexOf(letterChar));
            typedLetters.Add(letterChar);
        }
    }

    private void DeleteCharacter()
    {
        if (typedLetters.Any())
        {
            char deletedLetter = typedLetters[^1];

            if (deletedLetter != char.MinValue)
            {
                typedLetters.RemoveAt(typedLetters.IndexOf(deletedLetter));
                displayedLetters.Add(deletedLetter);
            }
        }
    }

    public void UpdateLetters(string letters)
    {
        validKeys.Clear();

        givenLetters = letters;

        displayedLetters = ShuffleLetters(givenLetters.ToCharArray());

        for (int i = 0; i < displayedLetters.Count; i++)
        {
            int indexNumber = LetterToIndex(displayedLetters[i]);

            LetterObj letter = Instantiate(letterObj, transform);
            letter.GetComponent<SpriteRenderer>().sprite = letterSprites[indexNumber];
            letter.transform.localPosition = new Vector3(i * LETTER_SPACING, 0, 0);

            KeyCode key = Enum.Parse<KeyCode>($"{displayedLetters[i]}", true);
            letter.keyCode = key;

            validKeys.Add(key);

            // Show Letters in the display
            letterDisplay.Add(letter);
        }
    }

    public List<char> ShuffleLetters(char[] text)
    {
        if (text != null && text.Any())
        {
            return text.OrderBy(x => Guid.NewGuid()).ToList();
        }

        return new List<char>();
    }

    public List<char> ShuffleLetters(string text = null)
    {
        if (!string.IsNullOrEmpty(text))
        {
            return ShuffleLetters(text.ToCharArray());
        }

        return givenLetters.ToCharArray().OrderBy(x => Guid.NewGuid()).ToList();
    }

    private int LetterToIndex(char letter)
    {
        if (Enum.TryParse($"{letter}".ToUpper(), out ELetter value))
        {
            return (int)value;
        }

        return (int)ELetter.Unknown;
    }

    public string GetGuess()
    {
        return new string(typedLetters.ToArray());
    }
    private bool ValidKeyDown()
    {
        if (givenLetters.Contains($"{Event.current.keyCode}"))
        {
            return true;
        }

        return false;
    }

    internal void ClearTyped()
    {
        displayedLetters = new List<char>(typedLetters);
        typedLetters.Clear();
    }
}
