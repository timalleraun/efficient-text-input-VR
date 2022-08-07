using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class name : MonoBehaviour
{
    string word = null;
    int wordIndex = 0;
    string alpha;
    public Text myName = null;

    public void alphabetFunction (string alphabet)
    {
        wordIndex++;
        word = word + alphabet;
        myName.text = word;
    }
}
