using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

/// <summary>
/// REIMPLEMENT TO COROUTINES
/// </summary>
public class WordsetGenerator : MonoBehaviour
{
    [SerializeField]
    private List<string> _dictionary = new();

    public string letters;

    public List<string> wordSet;

    [SerializeField]
    private string textPath;

    private UnityWebRequest uwr;

    public bool IsDone { get; internal set; } = false;

    private void Start()
    {
        StartCoroutine(ReadCSV(Path.Combine(Application.streamingAssetsPath, "WordAssets/10k-sfw.txt")));
    }

    private IEnumerator ReadCSV(string path)
    {
        uwr = UnityWebRequest.Get(path);

        yield return uwr.SendWebRequest();
        
        if (uwr.isDone)
        {
            _dictionary.AddRange(uwr.downloadHandler.text.Split("\n"));
            _dictionary = _dictionary.Select(x => x.Trim()).ToList();
            Debug.Log($"Total Words: {_dictionary.Count}");
        }

        Debug.Log("Download Complete!");
        IsDone = uwr.isDone;
    }

    private IEnumerator IsRequestDone(int length)
    {    
        while (!IsDone)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Generating...");

        var filteredDictionary = _dictionary.Where(s => s.Length <= length && s.Length >= 3).ToList();
        var candidateWord = filteredDictionary.Where(s => s.Length == length).ToList();

        letters = candidateWord[UnityEngine.Random.Range(1, candidateWord.Count)];
        var wordLookup = letters.ToCharArray().ToLookup(c => c);

        wordSet = filteredDictionary.Where(w => w.ToLookup(c => c).All(wc => wc.Count() <= wordLookup[wc.Key].Count())).OrderBy(s => s.Length).ToList();
        Debug.Log($"Wordset done: {wordSet.Count}");
    }

    public void GenerateFromLetters(string chars)
    {
        //StartCoroutine(IsRequestDone());

        letters = chars;
        var filteredDictionary = _dictionary.Where(s => s.Length <= chars.Length && s.Length >= 3);
        var mainLookup = chars.ToCharArray().ToLookup(c => c);

        wordSet = filteredDictionary.Where(w => w.ToLookup(c => c).All(wc => wc.Count() <= mainLookup[wc.Key].Count())).OrderBy(s => s.Length).ToList();
    }

    public void GenerateFromLength(int length)
    {
        StartCoroutine(IsRequestDone(length));
    }
}
