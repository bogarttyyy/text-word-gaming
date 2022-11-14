using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static event Action<List<string>> OnCorrectGuess;
    public static event Action<List<string>> OnGenerateWords;
    public static event Action OnIncorrectGuess;
    public static event Action OnOutOfLives;
    public static event Action OnClearGuessList;
    public static event Action OnRoundComplete;

    public static event Action OnTextInput;
    public static event Action OnTextDelete;

    public static event Action OnDebugEvent;

    private void Update()
    {
        HandleOnDebugEvent();
    }

    private void HandleOnDebugEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDebugEvent?.Invoke();
        }
    }

    public static void GenerateWordsEvent(List<string> words)
    {
        OnGenerateWords?.Invoke(words);
    }

    internal static void CorrectGuessEvent(List<string> correctGuesses)
    {
        OnCorrectGuess?.Invoke(correctGuesses);
    }

    internal static void IncorrectGuessEvent()
    {
        OnIncorrectGuess?.Invoke();
    }

    internal static void OutOfLivesEvent()
    {
        OnOutOfLives?.Invoke();
    }

    internal static void ClearEvent()
    {
        OnClearGuessList?.Invoke();
    }

    internal static void RoundComplete()
    {
        OnRoundComplete?.Invoke();
    }

    internal static void TextInputEvent()
    {
        OnTextInput?.Invoke();
    }

    internal static void TextDeleteEvent()
    {
        OnTextDelete?.Invoke();
    }
}
