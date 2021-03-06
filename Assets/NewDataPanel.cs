﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class NewDataPanel : MonoBehaviour
{
    [Header("Данные новой категории:")]
    public InputField inputField;
    public Sprite categoryIcon;

    [Header("Панели создания новой категории")]
    public AnimatedPanel namePanel;
    public AnimatedPanel iconPanel;

    [Header("Ссылка на кнопку завершающую создание категории")]
    public Button newCategoryButton;

    public event Action<string, Sprite> OnNewCategoryButtonClick;

    private void Start()
    {
        newCategoryButton.onClick.AddListener(NewCategoryButtonClick);
    }


    private void NewCategoryButtonClick()
    {
        if (inputField.text != "")
        {

            OnNewCategoryButtonClick.Invoke(inputField.text, categoryIcon);

            inputField.text = "";
            gameObject.GetComponent<AnimatedPanel>().Hide();

            namePanel.Show();
            iconPanel.Hide();
        }
    }

    private void OnDestroy()
    {
        newCategoryButton.onClick.RemoveListener(NewCategoryButtonClick);
    }
}
