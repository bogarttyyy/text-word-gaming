using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Accessibility;

public class WordContainer : MonoBehaviour
{
    private const float padding = 0;
    private const float spacing = 30;

    [Range(3,6)]
    [SerializeField]
    private int length;

    [SerializeField]
    private string word;

    [SerializeField]
    private List<TextContainer> textContainers;

    [SerializeField]
    private TextContainer textContainerPrefab;

    void Start()
    {
        if (!string.IsNullOrEmpty(word))
        {
            length = word.Length;
        }

        InitializeWord();
    }

    private void InitializeWord()
    {
        textContainers = new List<TextContainer>();

        for (int i = 0; i < length; i++)
        {
            TextContainer letter = Instantiate(textContainerPrefab, transform, false);
            //TextContainer letter = Instantiate(textContainerPrefab, new Vector3((padding + (i + 1) * spacing), 0), Quaternion.identity, transform);
            letter.SetLetter($"{word.ToCharArray()[i]}");
            letter.transform.localPosition = new Vector3((padding + (i) * spacing), 0);
            textContainers.Add(letter);
        }
    }

    public void SetWord(string word)
    {
        this.word = word;
    }
}
