using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveText : MonoBehaviour
{
    public InputField userInputField;
    private static string userInput {get; set;} 

    void Start()
    {
        userInputField.onEndEdit.AddListener(SubmitInput);
    }

    private void SubmitInput(string input)
    {
        userInput = input;
    }

    public static string GetUserInput() {
        return userInput;
    }
}
