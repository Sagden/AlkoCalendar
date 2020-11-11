using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwitchWine : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMPro.TMP_Text dateTextContainer;
    public Color colorOff;
    public Color colorOn;
    [Space]
    public Sprite spriteEmpty, spriteFull;
    public Image image;

    private bool wineGlassFull = false;
    public bool WineGlassFull { get => wineGlassFull; }
    

    public bool pressed = false;

    public event Action<int, bool> OnSwitchOn;

    private int index;
    public int Index {
        set
        {
            index = value;
            dateTextContainer.text = (value + 1).ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!WineGlassFull)
        {
            pressed = true;
        }
        else
        {
            Invoke("Press", 0.4f);
            pressed = true;
        }
    }

    void Press()
    {
        if (pressed)
        {
            SwitchOff();
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                pressed = false;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!WineGlassFull && pressed)
        {
            SwitchOn();
        }

        pressed = false;
        CancelInvoke("Press");
    }

    public void SwitchOn()
    {
        OnSwitchOn.Invoke(index, true);
        wineGlassFull = true;

        dateTextContainer.color = colorOn;

        GetComponent<Outline>().enabled = true;
        //image.sprite = spriteFull;
    }

    public void SwitchOff()
    {
        OnSwitchOn.Invoke(index, false);

        dateTextContainer.color = colorOff;

        GetComponent<Outline>().enabled = false;

        //image.sprite = spriteEmpty;
        wineGlassFull = false;
        pressed = false;
    }
}
