using System;
using UnityEngine;
using UnityEngine.UI;

public class NewDataPanel : MonoBehaviour
{
    public Text categoryName;
    public Button newCategoryButton;

    public event Action<string> OnNewCategoryButtonClick;

    private void Start()
    {
        newCategoryButton.onClick.AddListener(NewCategoryButtonClick);
    }

    public void Switch()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void NewCategoryButtonClick()
    {
        if (categoryName.text != "")
        {
            OnNewCategoryButtonClick.Invoke(categoryName.text);
        }
    }

    private void OnDestroy()
    {
        newCategoryButton.onClick.RemoveListener(NewCategoryButtonClick);
    }
}
