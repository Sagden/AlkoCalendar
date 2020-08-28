using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class MouseEventsController : MonoBehaviour
{
    public event Action<ChangeDisplaySide> OnDisplayChange;
    public event Action<float> OnPressedX;
    public event Action<float> OnPressedY;

    private float deltaX;
    private float deltaY;

    private Vector3 startMouseCoordinate;
    private Vector3 mouseLastPos = Vector3.zero;

    private bool canMovingX = false;
    private bool canMovingY = false;

    void LateUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Android();
        }
        else
        {
            Windows();
        }
    }

    private void Windows()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseLastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startMouseCoordinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - mouseLastPos.y;
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - mouseLastPos.x;

            SwipeCalculation();

            if (canMovingX)
            {
                OnPressedX.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130);
            }
            if (canMovingY)
            {
                OnPressedY.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130);
            }

            mouseLastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (canMovingY)
                if (deltaY < -0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Down);
                }
                else
                if (deltaY > 0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Up);
                }
                else
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Stay);
                }

            canMovingX = false;
            canMovingY = false;
        }
    }
    private void Android()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                mouseLastPos = Input.GetTouch(0).position;
                startMouseCoordinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                deltaY = Input.GetTouch(0).position.y - mouseLastPos.y;
                deltaX = Input.GetTouch(0).position.x - mouseLastPos.x;

                SwipeCalculation();

                if (canMovingX)
                {
                    OnPressedX.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130);
                }
                if (canMovingY)
                {
                    OnPressedY.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130);
                }

                mouseLastPos = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (deltaY < -10f)
                {
                    deltaY = 0;
                    OnDisplayChange?.Invoke(ChangeDisplaySide.Down);
                }
                else
                if (deltaY > 10f)
                {
                    deltaY = 0;
                    OnDisplayChange?.Invoke(ChangeDisplaySide.Up);
                }
                else
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Stay);
                }

                canMovingX = false;
                canMovingY = false;
            }
        }
    }

    private void SwipeCalculation()
    {
        if ((Mathf.Abs(deltaX) > 0.1f || Mathf.Abs(deltaY) > 0.1f) && canMovingX == false && canMovingY == false)
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
                canMovingX = true;
            else
                canMovingY = true;
    }
}

public enum ChangeDisplaySide
{
    Up,
    Down,
    Stay
}