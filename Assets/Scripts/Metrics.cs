using System;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

public class Metrics : MonoBehaviour
{

    public TMP_InputField inputField;
    public TMP_InputField preselectedPhrase;
    public TMP_InputField logField; // temporary text field to display metrics
    
    bool startedTyping = false;
    double elapsedTime;
    int phraseLength;
    string path = @"C:\Users\Tim\Desktop\BA Efficient text input in Virtual Reality";
    StringDistance stringDistance = new StringDistance();
    Stopwatch stopwatch = new Stopwatch();

    /// <summary>
    /// Calculate words per minute after the text has been entered.
    /// </summary>
    public void CalculateWPM()
    {
        logField.text += "";
        StopStopwatch();
        CalculatePhraseLength();
        double wpm = (phraseLength - 1) / elapsedTime * 60 * 0.2; // calculate words per minute
        logField.text += "Phrase length: " + phraseLength + ", time in seconds: " + elapsedTime + "\n" + "WORDS PER MINUTE: " + Math.Round(wpm, 2) + "\n\n"; // display words per minute in temporary text field
    }

    /// <summary>
    /// Calculate the length of the entered phrase.
    /// </summary>
    public void CalculatePhraseLength()
    {
        phraseLength = inputField.text.Length;
    }

    /// <summary>
    /// Start the stopwatch once the user enters the first character.
    /// </summary>
    public void StartStopwatch()
    {
        if (startedTyping == false)
        {
            startedTyping = true;
            stopwatch.Restart();
        }
    }

    /// <summary>
    /// Stop the stopwatch once the user hits the "done" button.
    /// </summary>
    public void StopStopwatch()
    {
        stopwatch.Stop();
        elapsedTime = (int)stopwatch.ElapsedMilliseconds/1000;
    }

    /// <summary>
    /// Calculate the error rate of the entered text.
    /// </summary>
    public void CalculateER()
    {
        double inf = stringDistance.LevenshteinDistance(inputField.text, preselectedPhrase.text);
        double er = inf / phraseLength * 100;
        if (er > 100)
        {
            er = 100;
        }
        logField.text += "Incorrect characters: " + inf + "\n" + "Total characters: " + phraseLength + "\n" + "ERROR RATE: " + Math.Round(er, 2) + "%";
    }
}

public class StringDistance
{
    /// <summary>
    /// Compute the distance between two strings.
    /// </summary>
    public int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Step 1
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Step 2
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Step 3
        for (int i = 1; i <= n; i++)
        {
            //Step 4
            for (int j = 1; j <= m; j++)
            {
                // Step 5
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                // Step 6
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        // Step 7
        return d[n, m];
    }
}

