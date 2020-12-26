using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldSettings : MonoBehaviour
{

    public InputField.InputType inputType;
    public TouchScreenKeyboardType keyboardType;

    private void Start()
    {

        InputField inputField = GetComponent<InputField>();

        inputField.inputType = inputType;
        inputField.keyboardType = keyboardType;

    }
}
