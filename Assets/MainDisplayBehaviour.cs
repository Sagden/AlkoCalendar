using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDisplayBehaviour : MonoBehaviour
{
    public MouseEventsController mouseEvents;

    private Vector2 startMouseCoordinate;

    private Vector2 mainDisplayStartCoordinate;
    private Vector2 mainDisplayNextPosition;

    private void Start()
    {
        mouseEvents.OnPressedY += ChangeYCoordinate;
        mouseEvents.OnDisplayChange += ChangeDisplay;

        mainDisplayStartCoordinate = transform.localPosition;
        mainDisplayNextPosition = transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            startMouseCoordinate = transform.localPosition;
    }

    private void ChangeYCoordinate(float offset)
    {
        transform.localPosition = new Vector3( transform.localPosition.x, startMouseCoordinate.y + offset, 0 );
    }

    private void ChangeDisplay(ChangeDisplaySide state)
    {
        switch(state)
        {
            case ChangeDisplaySide.Down: mainDisplayNextPosition = new Vector2(mainDisplayNextPosition.x, mainDisplayNextPosition.y - 1440); break;
            case ChangeDisplaySide.Up: mainDisplayNextPosition = new Vector2(mainDisplayNextPosition.x, mainDisplayNextPosition.y + 1440); break;
        }

        transform.DOLocalMove(mainDisplayNextPosition, 0.7f);
    }

    private void OnDestroy()
    {
        mouseEvents.OnPressedY -= ChangeYCoordinate;
    }
}
