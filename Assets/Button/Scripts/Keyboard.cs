using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_InputField outputField;
    public GameObject normalButtons;
    public GameObject capsButtons;
    public GameObject symbolsButtons;
    TextMeshProUGUI buttonText;

    private bool caps;
    private bool symbols;
    private T9Algo algo;

    void Start()
    {
        caps = false;
        symbols = false;
    }

    public void InsertChar(string c)
    {
        inputField.text += c;
    }

    public void DeleteChar()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }
    public void InsertSpace()
    {
        inputField.text += " ";
        if (outputField != null)
        {
            outputField.text += " ";
        }
    }

    public void CapsPressed()
    {
        if (!caps && !symbols)
        {
            normalButtons.SetActive(false);
            capsButtons.SetActive(true);
            caps = true;
        }
        else if (caps && !symbols)
        {
            BackToNormal();
        }
    }

    public void BackToNormal()
    {
        capsButtons.SetActive(false);
        normalButtons.SetActive(true);
        caps = false;
    }

    public void SymbolsPressed()
    {
        if (caps && !symbols)
        {
            capsButtons.SetActive(false);
            symbolsButtons.SetActive(true);
            symbols = true;
        }
        else if (!caps && !symbols)
        {
            normalButtons.SetActive(false);
            symbolsButtons.SetActive(true);
            symbols = true;
        }
        else
        {
            symbolsButtons.SetActive(false);
            normalButtons.SetActive(true);
            symbols = false;
            caps = false;
        }
    }

    public void EnterPressed()
    {

    }
}
