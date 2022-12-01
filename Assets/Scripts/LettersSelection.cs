using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LettersSelection : MonoBehaviour
{
    //private const string letterSprite = "letters_v3_";
    private const float LETTER_SPACING = 1.25f;
    private const float LETTER_MOVE_POINT = 0f;

    private KeyCode[] allKeys;

    private List<LetterPosition> typedPositions = new();

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
    private List<LetterObj> letterObjs = new();


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
            ShuffleDisplay();
        }

        if (Input.anyKeyDown)
        {
            foreach (var key in validKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    MoveToNextAvailablePosition(TypedLetter(key));
                }
            }
        }

        UpdateDisplay();
    }

    private void MoveToNextAvailablePosition(char typed)
    {
        LetterPosition letPos = GetNextAvailablePosition();

        LetterObj letterObj = letterObjs.FirstOrDefault(f => f.GetChar() == typed && !f.isTyped);

        letPos.SetObject(letterObj);
    }

    private LetterPosition GetNextAvailablePosition()
    {
        var pos = typedPositions.FirstOrDefault(p => !p.isOccupied);
        
        if (pos != null)
        {
            pos.isOccupied = true;
            return pos;
        }

        return typedPositions.Last();
    }

    private char TypedLetter(KeyCode key)
    {
        char typedKey = key.ToString().ToLower()[0];

        char letterChar = displayedLetters.FirstOrDefault(c => c == typedKey);

        if (letterChar != char.MinValue)
        {
            displayedLetters.RemoveAt(displayedLetters.IndexOf(letterChar));
            typedLetters.Add(letterChar);
        }

        return typedKey;
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

            LetterPosition charPos = GetLastCharTyped();
            charPos?.ClearObject();
        }
    }

    private void UpdateDisplay()
    {
        foreach (var obj in letterObjs)
        {
            if (obj.transform.localPosition != obj.displayPosition && !obj.isTyped)
            {
                obj.transform.localPosition = Vector3.MoveTowards(obj.transform.localPosition, obj.displayPosition, 2f);
            }
        }
    }

    private LetterPosition GetLastCharTyped()
    {
        if (typedPositions.Any())
        {
            return typedPositions.LastOrDefault(f => f.letterObj != null);
        }

        return null;
    }

    public void UpdateLetters(string letters)
    {
        validKeys.Clear();
        ClearGiven();

        givenLetters = letters;

        displayedLetters = ShuffleLetters(givenLetters.ToCharArray());

        validKeys.AddRange(displayedLetters.Distinct().Select(l => Enum.Parse<KeyCode>($"{l}", true)));

        for (int i = 0; i < displayedLetters.Count; i++)
        {
            int indexNumber = LetterToIndex(displayedLetters[i]);

            LetterObj letter = Instantiate(letterObj, transform);
            letter.GetComponent<SpriteRenderer>().sprite = letterSprites[indexNumber];
            letter.transform.localPosition = new Vector2(i * LETTER_SPACING, 0);

            typedPositions.Add(new LetterPosition(new Vector2(i * LETTER_SPACING, -1.25f)));

            KeyCode key = Enum.Parse<KeyCode>($"{displayedLetters[i]}", true);
            letter.keyCode = key;

            // Show Letters in the display
            letterObjs.Add(letter);
        }

        transform.position = new Vector2((letters.Length - 1) * -LETTER_SPACING / 2, 0);

    }

    private void ClearGiven()
    {
        foreach (var item in letterObjs)
        {
            Destroy(item.gameObject);
        }

        letterObjs.Clear();
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

    internal void ShuffleDisplay()
    {
        List<Vector3> coordinates = letterObjs.Select(o => o.displayPosition).ToList();

        letterObjs = letterObjs.OrderBy(l => Guid.NewGuid()).ToList();

        for (int i = 0; i < letterObjs.Count; i++)
        {
            letterObjs[i].displayPosition = coordinates[i];
        }
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
        displayedLetters.AddRange(typedLetters);
        typedLetters.Clear();

        foreach (var item in typedPositions)
        {
            item.ClearObject();
        }
    }
}
