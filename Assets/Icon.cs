using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Icon : MonoBehaviour, IPointerClickHandler
{

    public Image image;
    public int id;
    [Space]
    public GameObject checkmark;

    public event Action<Icon> OnClick;


    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(this);
    }

}
