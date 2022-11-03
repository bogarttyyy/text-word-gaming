using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private WordsetGenerator wordsetGenerator;

    [SerializeField]
    private string letters;

    [SerializeField]
    private List<string> words;
    public static GameManager Instance { get; private set; }


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
    }

    public void GenerateWords()
    {
        wordsetGenerator.Generate(letters);

        words = wordsetGenerator.wordSet;
    }


}
