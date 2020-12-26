using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon : MonoBehaviour, IPointerClickHandler
{

    public Image image;

    public event Action<Sprite> OnClick;


    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(image.sprite);
    }

}
