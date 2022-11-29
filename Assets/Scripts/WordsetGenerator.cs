using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// REIMPLEMENT TO COROUTINES
/// </summary>
public class WordsetGenerator
{
    private static readonly List<string> _dictionary = new List<string>();

    public string letters;

    public List<string> wordSet;

    public WordsetGenerator(string path = null)
    {  
        if (string.IsNullOrEmpty(path))
        {
            ReadCSV(Path.Combine(Application.streamingAssetsPath, "WordAssets/10k-sfw.txt"));
        }
        //else
        //{
        //    ReadCSV(path);
        //}
    }

    private static void ReadCSV(string path)
    {
        string[] textLines;

        #region WebGL Build
        UnityWebRequest uwr = UnityWebRequest.Get(path);

        uwr.SendWebRequest();
        textLines = uwr.downloadHandler.text.Split('\u002C');
        _dictionary.AddRange(textLines);

       

        #endregion

        #region Windows Build

        //StreamReader streamReader = new(path);
        //bool eof = false;

        //while (!eof)
        //{
        //    string dataString = streamReader.ReadLine();
        //    if (dataString == null)
        //    {
        //        //eof = true;
        //        return;
        //    }

        //    string[] values = dataString.Split('\u002C');

        //    _dictionary.AddRange(values);
        //}
        #endregion
    }

    public void GenerateFromLetters(string chars)
    {
        letters = chars;
        var filteredDictionary = _dictionary.Where(s => s.Length <= chars.Length && s.Length >= 3);
        var mainLookup = chars.ToCharArray().ToLookup(c => c);

        wordSet = filteredDictionary.Where(w => w.ToLookup(c => c).All(wc => wc.Count() <= mainLookup[wc.Key].Count())).OrderBy(s => s.Length).ToList();
    }

    public void GenerateFromLength(int length)
    {
        var filteredDictionary = _dictionary.Where(s => s.Length <= length && s.Length >= 3).ToList();
        var candidateWord = filteredDictionary.Where(s => s.Length == length).ToList();

        letters = candidateWord[Random.Range(1, candidateWord.Count)];
        var wordLookup = letters.ToCharArray().ToLookup(c => c);

        wordSet = filteredDictionary.Where(w => w.ToLookup(c => c).All(wc => wc.Count() <= wordLookup[wc.Key].Count())).OrderBy(s => s.Length).ToList();
    }
}
