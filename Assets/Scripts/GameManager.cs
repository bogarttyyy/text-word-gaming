using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private WordsetGenerator wordsetGenerator;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TMP_Text scoreDisplay;

    [SerializeField]
    private TMP_Text lettersDisplay;

    [SerializeField]
    private TMP_Text remainingDisplay;

    [SerializeField]
    private TMP_Text livesDisplay;

    [SerializeField]
    private TMP_Text finalScoreDisplay;

    [SerializeField]
    private GameObject victoryScreen;

    [SerializeField]
    private GameObject gameOverScreen;

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
    private int lives;

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

        ResetGame();

        GenerateWords(4);
        UpdateRemaining();
        UpdateLives();

        EventsManager.OnRoundComplete += EventsManager_OnRoundComplete;
        EventsManager.OnOutOfLives += EventsManager_OnOutOfLives;
        EventsManager.OnIncorrectGuess += EventsManager_OnIncorrectGuess;
    }

    private void EventsManager_OnRoundComplete()
    {
        victoryScreen.SetActive(true);
    }

    private void EventsManager_OnOutOfLives()
    {
        finalScoreDisplay.text = $"Final Score: {wordScore}";
        gameOverScreen.SetActive(true);
    }

    private void EventsManager_OnIncorrectGuess()
    {
        UpdateLives();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (victoryScreen.activeSelf)
            {
                NextRound();
            }
            else if (gameOverScreen.activeSelf)
            {
                NewGame();
            }
            else
            {
                EnterGuess();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOverScreen.activeSelf)
            {
                NewGame();
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.anyKeyDown)
        {
            if (!(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)))
            {
                EventsManager.TextInputEvent();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                EventsManager.TextDeleteEvent();
            }
        }
    }

    private void UpdateScore()
    {
        wordScore += 100 * ScoreMultipler(inputField.text);
        scoreDisplay.text = $"Score: {wordScore}";
    }

    private int ScoreMultipler(string word)
    {
        return word.Length switch
        {
            4 => 2,
            5 => 4,
            6 => 8,
            _ => 1,
        };
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
            lives -= 1;
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
            return 4;
        }
        else if (round < 8)
        {
            return 5;
        }

        return 6;
    }

    public void GenerateWords(int length)
    {
        wordsetGenerator.GenerateFromLength(length);
        words = wordsetGenerator.wordSet;
        letters = wordsetGenerator.letters.ToString();
        ShuffleLetters(letters);
        EventsManager.GenerateWordsEvent(words);
    }

    private void RefocusInput()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void ShuffleLetters(string letter)
    {
        if (string.IsNullOrEmpty(letter))
        {
            letter = letters;
        }
        
        lettersDisplay.text = new string(letter.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
    }

    public void ClearField()
    {
        inputField.text = string.Empty;
    }

    public void EnterGuess()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            if (CheckWordGuess(inputField.text))
            {
                UpdateScore();
                UpdateRemaining();

                correctGuesses.Add(inputField.text.ToLower());
                EventsManager.CorrectGuessEvent(correctGuesses);

                if (words.Count() == correctGuesses.Count())
                {
                    EventsManager.RoundComplete();
                }
            }
            else
            {
                EventsManager.IncorrectGuessEvent();

                if (lives < 1)
                {
                    EventsManager.OutOfLivesEvent();
                }
            }
        }
        else
        {
            Debug.Log("Empty Enter");
        }

        inputField.text = string.Empty;
        RefocusInput();
    }

    public void NextRound()
    {
        round += 1;
        RegenerateWordList();
        UpdateRemaining();
        victoryScreen.SetActive(false);
    }

    private void UpdateRemaining()
    {
        remainingDisplay.text = $"Remaining: {words.Count() - correctGuesses.Count()}";
    }

    private void UpdateLives()
    {
        livesDisplay.text = $"Lives: {lives}";
    }

    public void NewGame()
    {
        ResetGame();

        RegenerateWordList();
        UpdateRemaining();
        UpdateLives();

        gameOverScreen.SetActive(false);
    }

    private void ResetGame()
    {
        lives = 3;
        round = 1;
        scoreDisplay.text = "Score: 0";
        inputField.text = string.Empty;
    }

    public void QuitToTitleScreen()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnDestroy()
    {
        EventsManager.OnRoundComplete -= EventsManager_OnRoundComplete;
        EventsManager.OnOutOfLives -= EventsManager_OnOutOfLives;
        EventsManager.OnIncorrectGuess -= EventsManager_OnIncorrectGuess;
    }
}
