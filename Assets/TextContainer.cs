using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextContainer : MonoBehaviour
{
    [SerializeField]
    private string letter;

    private TMP_Text tmpText;

    // Start is called before the first frame update
    void Start()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(letter))
        {
            tmpText.text = $"{letter}";
        }
        else
        {
            tmpText.text = $"_";
        }
    }

    public void SetLetter(string letter)
    {
        if (letter.Length == 1)
        {
            this.letter = letter;
        }
    }
}
