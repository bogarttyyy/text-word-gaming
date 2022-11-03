using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private WordsetGenerator wordsetGenerator;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private string letters;

    [SerializeField]
    private List<string> words;

    [SerializeField]
    private List<string> correctGuesses;

    public static GameManager Instance { get; private set; }

    private EventsManager eventsManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        wordsetGenerator = new WordsetGenerator();
        eventsManager = GetComponent<EventsManager>();
        correctGuesses = new List<string>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(inputField.text))
            {
                if (CheckWordGuess(inputField.text))
                {
                    correctGuesses.Add(inputField.text.ToLower());
                    EventsManager.CorrectGuessEvent(correctGuesses);
                }
                else
                {
                    EventsManager.IncorrectGuessEvent();
                }

                inputField.text = string.Empty;
                inputField.Select();
            }
            else
            {
                Debug.Log("Empty Enter");
            }
        }
    }

    public void GenerateWords()
    {
        wordsetGenerator.Generate(letters);

        words = wordsetGenerator.wordSet;

        EventsManager.GenerateWordsEvent(words);
    }

    private bool CheckWordGuess(string wordGuess)
    {
        string cleanString = wordGuess.Trim().ToLower();
        if (words != null && words.Any() && words.Contains(cleanString))
        {
            if (!correctGuesses.Contains(cleanString))
            {
                return true;
            }
            else
            {
                Debug.Log($"{cleanString} has already been guessed");
            }
        }
        else
        {
            Debug.Log($"Try again");
        }

        return false;
    }

    public void ClearGuesses()
    {
        correctGuesses.Clear();
        EventsManager.ClearEvent();
    }
}
