using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class MouseEventsController : MonoBehaviour
{
    public RectTransform allDisplays;
    public RectTransform menu;

    public float swipeSpeed = 0.7f;

    public event Action<ChangeDisplaySide> OnDisplayChange;
    public event Action<float> OnPressedX;
    public event Action<float> OnPressedY;

    private float deltaX;
    private float deltaY;

    private Vector3 allDisplaysNextCoordinate;
    private Vector3 startMouseCoordinate;

    private Vector3 allDisplaysStartCoordinate;
    private Vector3 menuStartCoordinate;
    private Vector3 lastpos = Vector3.zero;

    private bool canMovingX = false;
    private bool canMovingY = false;

    private void Start()
    {
        allDisplaysNextCoordinate = allDisplays.localPosition;
        allDisplaysStartCoordinate = allDisplays.localPosition;
        menuStartCoordinate = menu.localPosition;
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

            allDisplaysStartCoordinate = allDisplays.localPosition;
            menuStartCoordinate = menu.localPosition;
            startMouseCoordinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - lastpos.y;
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - lastpos.x;

            if ((Mathf.Abs(deltaX) > 0.1f || Mathf.Abs(deltaY) > 0.1f) && canMovingX == false && canMovingY == false)
                if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
                    canMovingX = true;
                else
                    canMovingY = true;


                if (canMovingX)
                {
                OnPressedX.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130);
                    //allDisplays.localPosition = new Vector3(
                    //   allDisplaysStartCoordinate.x + ((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130),
                    //   allDisplays.localPosition.y, 0);
                    //menu.localPosition = new Vector3(
                    //   menuStartCoordinate.x + ((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130),
                    //   menu.localPosition.y, 0);
                }
                if (canMovingY)
                {
                OnPressedY.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130);
                //allDisplays.localPosition = new Vector3(
                //    allDisplays.localPosition.x,
                //    allDisplaysStartCoordinate.y + ((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130), 0);
                //menu.localPosition = new Vector3(
                //    menu.localPosition.x,
                //    menuStartCoordinate.y + ((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130), 0);

            }

            lastpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (canMovingY)
                if (deltaY < -0.1f)
                {
                    //allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y - 1440, allDisplaysNextCoordinate.z);
                    OnDisplayChange.Invoke(ChangeDisplaySide.Down);
                }
                else
                if (deltaY > 0.1f)
                {
                    //allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y + 1440, allDisplaysNextCoordinate.z);
                    OnDisplayChange.Invoke(ChangeDisplaySide.Up);
                }

            canMovingX = false;
            canMovingY = false;
            //allDisplays.DOLocalMove(allDisplaysNextCoordinate, swipeSpeed);
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

                allDisplaysStartCoordinate = allDisplays.localPosition;
                startMouseCoordinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                deltaY = Input.GetTouch(0).position.y - lastpos.y;

                allDisplays.localPosition = new Vector3(
                    allDisplays.localPosition.x,
                    allDisplaysStartCoordinate.y + ((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130), 0);

                lastpos = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (deltaY < -10f)
                {
                    allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y - 1440, allDisplaysNextCoordinate.z);
                    deltaY = 0;
                    OnDisplayChange?.Invoke(ChangeDisplaySide.Down);
                }
                else
                if (deltaY > 10f)
                {
                    allDisplaysNextCoordinate = new Vector3(allDisplaysNextCoordinate.x, allDisplaysNextCoordinate.y + 1440, allDisplaysNextCoordinate.z);
                    deltaY = 0;
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