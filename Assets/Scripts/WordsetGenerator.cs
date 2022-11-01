using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WordsetGenerator
{
    private static readonly List<string> _dictionary = new List<string>();

    public char[] letters;

    public List<string> wordSet;

    public WordsetGenerator(string path = null)
    {  
        if (string.IsNullOrEmpty(path))
        {
            ReadCSV("Assets/WordAssets/10k-sfw.txt");
        }
        else
        {
            ReadCSV(path);
        }
    }

    private static void ReadCSV(string path)
    {
        StreamReader streamReader = new StreamReader(path);
        bool eof = false;

        while (!eof)
        {
            string dataString = streamReader.ReadLine();
            if (dataString == null)
            {
                //eof = true;
                return;
            }

            string[] values = dataString.Split('\u002C');

            _dictionary.AddRange(values);
        }
    }

    public void Generate(string chars)
    {
        var filteredDictionary = _dictionary.Where(s => s.Length <= chars.Length && s.Length >= 3);

        var mainLookup = chars.ToCharArray().ToLookup(c => c);

        wordSet = filteredDictionary.Where(w => w.ToLookup(c => c).All(wc => wc.Count() <= mainLookup[wc.Key].Count())).ToList();
    }
}
