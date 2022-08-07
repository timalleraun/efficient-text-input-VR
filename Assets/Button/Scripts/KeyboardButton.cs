using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardButton : MonoBehaviour
{
    Keyboard keyboard;
    TextMeshProUGUI buttonText;

    void Start()
    {
        keyboard = GetComponentInParent<Keyboard>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText.text.Length == 1)
        {
            NameToButtonText();
            GetComponentInChildren<ButtonVR>().onPress.AddListener(delegate { keyboard.InsertChar(buttonText.text); });
            GetComponentInChildren<ButtonVR>().onPress.AddListener(delegate { keyboard.BackToNormal(); });
        }
    }

    // wenn UI Button gedrückt wird: keyboard.InsertChart(button.text);

    public void NameToButtonText()
    {
        buttonText.text = gameObject.name;
    }

}
