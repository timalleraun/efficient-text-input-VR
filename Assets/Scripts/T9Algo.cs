using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class T9Algo : MonoBehaviour
{
    public TMP_InputField inputField; // Texteingabe von Unity verlinken
    public string decodableText;
    public TMP_InputField outputField; // 2tes Texteingabefeld von Unity verlinken
    public TextAsset woerterbuch;

    private T9Encoder encoder;
    private List<KeyValuePair<string, string>> dictionary = new List<KeyValuePair<string, string>>();

    private int x = 0;

    void Start()
    {
        FillWords();
    }

    /// <summary>
    /// Returns last typed words from input field.
    /// </summary>
    public string GetDecodedText()
    {
        string[] eingegebeneWoerter = inputField.text.Split(' ');
        return eingegebeneWoerter[eingegebeneWoerter.Length-1];
    }

    /// <summary>
    /// Fills the dictionary with words from a textfile.
    /// </summary>
    public void FillWords()
    {
        this.encoder = new T9Encoder();
        string[] lines = woerterbuch.text.Split(',');

        for (int i=0; i<lines.Length ; i++)
        {
            this.AddWord(lines[i]);
        }

        this.encoder.Dictionary = this.dictionary;
    }

    /// <summary>
    /// Adds a word to the T9 dictionary.
    /// </summary>
    private void AddWord(string word)
    {
        this.dictionary.Add(new KeyValuePair<string, string>(this.encoder.EncodeString(word), word));
    }

    /// <summary>
    /// Displays entered text as T9.
    /// </summary>
    public void DisplayDecoded()
    {
        string[] eingegebeneWoerter = outputField.text.Split(' '); // split displayed text into words
        outputField.text = outputField.text.Substring(0, outputField.text.Length - eingegebeneWoerter[eingegebeneWoerter.Length - 1].Length); // remove last typed word from text display 

        string[] liste = encoder.Predict(GetDecodedText()).ToArray(); // stack matching words into a list

        // reset the cycle-counter, if all words of the list have been cycled
        if (x >= liste.Length)
        {
            x = 0;
        }

        if (liste[x] != "!")
        {
            outputField.text += liste[x]; // display the xth word of list
        }
    }
    
    public void CyclePressed()
    {
        x++;
    }

    /// <summary>
    /// Resets the cycle-counter to always display the first macthing word.
    /// </summary>
    public void SpacePressed()
    {
        x = 0;
    }

    public void DeletePressed()
    {
        x=0;
    }

    public void CharPressed()
    {
        x = 0;
    }
}

public class T9Encoder
{
    private List<KeyValuePair<string, string>> dictionary = new List<KeyValuePair<string, string>>();

    /// <summary>
    /// Gets or sets the words dictionary.
    /// </summary>
    public List<KeyValuePair<string, string>> Dictionary
    {
        get
        {
            return this.dictionary;
        }

        set
        {
            this.dictionary = value;
        }
    }

    /// <summary>
    /// Encodes a string to the T9 format.
    /// </summary>
    public string EncodeString(string clearText)
    {
        // Normalize and convert to lowercase.
        string result = this.RemoveDiacritics(clearText).ToLower();

        // Remove digits.
        result = Regex.Replace(result, "[2-9]", string.Empty);

        // Translate to SMS.
        result = Regex.Replace(result, "[abc]", "2");
        result = Regex.Replace(result, "[def]", "3");
        result = Regex.Replace(result, "[ghi]", "4");
        result = Regex.Replace(result, "[jkl]", "5");
        result = Regex.Replace(result, "[mno]", "6");
        result = Regex.Replace(result, "[pqrs]", "7");
        result = Regex.Replace(result, "[tuv]", "8");
        result = Regex.Replace(result, "[wxyz]", "9");

        // Replace remaining non-SMS characters by word boundary.
        result = Regex.Replace(result, "[^2-9]", " ");

        return result;
    }

    /// <summary>
    /// Decodes a T9 string to a word in the dictionary.
    /// </summary>
    public List<string> DecodeString(string t9Text)
    {
        return (from w in this.dictionary
                where w.Key == t9Text
                select w.Value).ToList();
    }

    /// <summary>
    /// Predicts a T9 decoding, based on a prefix.
    /// </summary>
    public List<string> Predict(string prefix)
    {
        return (from w in this.dictionary      
        where w.Key.StartsWith(prefix)
        select w.Value).ToList();

    }

    /// <summary>
    /// Normalizes a string and removes diacritics.
    /// </summary>
    public string RemoveDiacritics(string clearText)
    {
        string normalizedText = clearText.Normalize(NormalizationForm.FormD);

        StringBuilder sb = new StringBuilder();
        foreach (char ch in normalizedText)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}