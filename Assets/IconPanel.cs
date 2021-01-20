using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconPanel : MonoBehaviour
{
    [Header("Все иконки (переделать на ScriptableObj)")]
    public List<Sprite> icons;

    [Header("Ссылка на объект который наполняем")]
    public Transform contentObj;

    [Header("Префаб одной горизонтальной линии, объекта иконки и пустой иконки")]
    public Transform horizontalPref;
    public Transform iconPref;
    public Transform iconEmptyPref;

    [Header("Контроллер главный")]
    public NewDataPanel dataPanel;

    private Icon previousIconChoosen;


    private void Start()
    {
        IconListGeneration();
    }

    private void IconListGeneration()
    {
        int id = 0;

        for (int i = 0; i < Mathf.CeilToInt((float)icons.Count / 8); i++)
        {

            Transform horizontalObj = Instantiate(horizontalPref, contentObj);

            for (int o = 0; o < 8; o++)
            {

                if ((i * 8 + o) < icons.Count)
                {

                    Transform iconObj = Instantiate(iconPref, horizontalObj);
                    iconObj.GetComponent<Image>().sprite = icons[i * 8 + o];
                    iconObj.GetComponent<Icon>().id = id;
                    iconObj.GetComponent<Icon>().OnClick += NewIconChoose;

                    id++;
                }
                else
                {

                    Instantiate(iconEmptyPref, horizontalObj);

                }
            }
        }
    }

    private void NewIconChoose(Icon icon)
    {
        previousIconChoosen?.checkmark.SetActive(false);

        dataPanel.categoryIcon = icon.image.sprite;
        icon.checkmark.SetActive(true);

        previousIconChoosen = icon;
    }
}
