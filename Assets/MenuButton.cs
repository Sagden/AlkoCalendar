using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text text;
    public Image image;
    public int index;

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //ModeController.SetMode(index);
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        ModeController.SetMode(index);
    }
}
