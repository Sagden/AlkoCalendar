using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerDownHandler
{
    public TMP_Text text;
    public int index;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("cChange;");
        ModeController.SetMode(index);
    }
}
