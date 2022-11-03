using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WordsList : MonoBehaviour
{
    private TextMeshProUGUI wordList;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.OnGenerateWords += EventsManager_OnGenerateWords;
        EventsManager.OnClearGuessList += EventsManager_OnClearGuessList;
        wordList = GetComponent<TextMeshProUGUI>();
    }

    private void EventsManager_OnGenerateWords(List<string> words)
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
        EventsManager.OnGenerateWords -= EventsManager_OnGenerateWords;
    }
}
