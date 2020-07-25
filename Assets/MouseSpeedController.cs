using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class MouseSpeedController : MonoBehaviour
{
    public RectTransform allDisplays;

    public float swipeSpeed = 0.7f;

    public event Action<ChangeDisplaySide> OnDisplayChange;

    private float delta;

    private Vector3 allDisplaysNextCoordinate;
    private float startMouseCoordinateY;

    private float allDisplaysStartCoordinate;
    private Vector3 lastpos = Vector3.zero;

    private void Start()
    {
        allDisplaysNextCoordinate = allDisplays.localPosition;
        allDisplaysStartCoordinate = allDisplays.localPosition.y;
    }

    void LateUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Android_RectTransformBehaviour();
        }
        else
        {
            Windows_RectTransformBehaviour();
        }
    }

    private void Windows_RectTransformBehaviour()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DOTween.KillAll();
            lastpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            allDisplaysStartCoordinate = allDisplays.localPosition.y;
            startMouseCoordinateY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        }

        if (Input.GetMouseButton(0))
        {
            delta = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - lastpos.y;

            allDisplays.localPosition = new Vector3(
                allDisplays.localPosition.x,
                allDisplaysStartCoordinate + ((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinateY) * 130), 0);

            lastpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (delta < -0.1f)
            {
                allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y - 1440, allDisplaysNextCoordinate.z);
                OnDisplayChange.Invoke(ChangeDisplaySide.Down);
            }
            else
            if (delta > 0.1f)
            {
                allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y + 1440, allDisplaysNextCoordinate.z);
                OnDisplayChange.Invoke(ChangeDisplaySide.Up);
            }

            allDisplays.DOLocalMove(allDisplaysNextCoordinate, swipeSpeed);
        }
    }
    private void Android_RectTransformBehaviour()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                DOTween.KillAll();
                lastpos = Input.GetTouch(0).position;

                allDisplaysStartCoordinate = allDisplays.localPosition.y;
                startMouseCoordinateY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                delta = Input.GetTouch(0).position.y - lastpos.y;

                allDisplays.localPosition = new Vector3(
                    allDisplays.localPosition.x,
                    allDisplaysStartCoordinate + ((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinateY) * 130), 0);

                lastpos = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (delta < -10f)
                {
                    allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y - 1440, allDisplaysNextCoordinate.z);
                    delta = 0;
                    OnDisplayChange?.Invoke(ChangeDisplaySide.Down);
                }
                else
                if (delta > 10f)
                {
                    allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y + 1440, allDisplaysNextCoordinate.z);
                    delta = 0;
                    OnDisplayChange?.Invoke(ChangeDisplaySide.Up);
                }
                allDisplays.DOLocalMove(allDisplaysNextCoordinate, swipeSpeed);
            }
        }
    }
}

public enum ChangeDisplaySide
{
    Up,
    Down
}