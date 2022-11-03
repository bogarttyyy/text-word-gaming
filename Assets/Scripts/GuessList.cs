using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuessList : MonoBehaviour
{
    private TextMeshProUGUI wordList;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.OnCorrectGuess += EventsManager_OnCorrectGuess;
        EventsManager.OnClearGuessList += EventsManager_OnClearGuessList;
        wordList = GetComponent<TextMeshProUGUI>();
    }

    private void EventsManager_OnCorrectGuess(List<string> words)
    {
        string wordsString = string.Empty;

        foreach (var word in words)
        {
            wordsString += $"{word}\n";
        }

        wordList.text = wordsString;
    }

    private void EventsManager_OnClearGuessList()
    {
        wordList.text = string.Empty;
    }

    private void OnDestroy()
    {
        EventsManager.OnCorrectGuess -= EventsManager_OnCorrectGuess;
        EventsManager.OnClearGuessList -= EventsManager_OnClearGuessList;
    }
}
