using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private WordsetGenerator wordsetGenerator;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private string letters;

    [SerializeField]
    private List<string> words;

    [SerializeField]
    private List<string> correctGuesses;

    [SerializeField]
    private int round;

    [SerializeField]
    private int wordScore;

    public static GameManager Instance { get; private set; }

    private EventsManager eventsManager;

    private void Awake()
    {
        #region Singleton Code
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion
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
                    UpdateScore();
                    correctGuesses.Add(inputField.text.ToLower());
                    EventsManager.CorrectGuessEvent(correctGuesses);

                    if (words.Count() == correctGuesses.Count())
                    {
                        round += 1;
                        RegenerateWordList();
                        EventsManager.RoundComplete();
                    }
                }
                else
                {
                    EventsManager.IncorrectGuessEvent();
                }
            }
            else
            {
                Debug.Log("Empty Enter");
            }

            inputField.text = string.Empty;
            RefocusInput();
        }
    }

    private void UpdateScore()
    {
        wordScore += 1;
        scoreText.text = $"{wordScore}";
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

    public void RegenerateWordList()
    {
        int wordLength = AdjustDifficulty();
        GenerateWords(wordLength);
        ClearGuesses();
        Debug.Log("Congrats! On to the next round");
    }

    private int AdjustDifficulty()
    {
        if (round < 4)
        {
            return 3;
        }
        else if (round < 8)
        {
            return 4;
        }

        return 5;
    }

    public void GenerateWords()
    {
        wordsetGenerator.GenerateFromLetters(letters);
        words = wordsetGenerator.wordSet;
        EventsManager.GenerateWordsEvent(words);
    }

    public void GenerateWords(int length)
    {
        wordsetGenerator.GenerateFromLength(length);
        words = wordsetGenerator.wordSet;
        letters = wordsetGenerator.letters.ToString();
        EventsManager.GenerateWordsEvent(words);
    }

    private void RefocusInput()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }
}
