using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class MouseEventsController : MonoBehaviour
{
    public event Action<ChangeDisplaySide> OnDisplayChange;
    public event Action<float, float> OnPressedX;
    public event Action<float, float> OnPressedY;

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
            //----------Здесь код-------------------------

            SwipeCalculation();

            //--------------------------------------------
            mouseLastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (canMovingX)
            {
                OnPressedX.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130,
                    (Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130);
            }
            if (canMovingY)
            {
                OnPressedY.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130,
                    (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (canMovingY)
            {
                if (deltaY < -0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Down);
                    
                }
                else
                if (deltaY > 0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Up);
                    Debug.Log(deltaY);
                }
                else
                if (deltaY <= 0.1f && deltaY >= -0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Stay);
                }
            }
            if (canMovingX)
            {
                if (deltaX > 0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Left);
                }
                else
                if (deltaX < -0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Right);
                }
                else
                if (deltaX <= 0.1f && deltaX >= -0.1f)
                {
                    OnDisplayChange.Invoke(ChangeDisplaySide.Stay);
                }
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

                

                mouseLastPos = Input.GetTouch(0).position;

                if (canMovingX)
                {
                    OnPressedX.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130,
                        (Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130);
                }
                if (canMovingY)
                {
                    OnPressedY.Invoke((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - startMouseCoordinate.y) * 130,
                        (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - startMouseCoordinate.x) * 130);
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (canMovingY)
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
                    if (deltaY > -10f && deltaY < 10f)
                    {
                        deltaY = 0;
                        OnDisplayChange?.Invoke(ChangeDisplaySide.Stay);
                    }
                }
                if (canMovingX)
                {
                    if (deltaX < -10f)
                    {
                        deltaX = 0;
                        OnDisplayChange?.Invoke(ChangeDisplaySide.Right);
                    }
                    else
                    if (deltaX > 10f)
                    {
                        deltaX = 0;
                        OnDisplayChange?.Invoke(ChangeDisplaySide.Left);
                    }
                    else
                    if (deltaX > -10f && deltaY < 10f)
                    {
                        deltaX = 0;
                        OnDisplayChange.Invoke(ChangeDisplaySide.Stay);
                    }
                }

                canMovingX = false;
                canMovingY = false;
            }
        }
    }

    private void SwipeCalculation()
    {
        if ((Mathf.Abs(deltaX) > 0.01f || Mathf.Abs(deltaY) > 0.01f) && canMovingX == false && canMovingY == false)
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) - 1)
                canMovingX = true;
            else
                canMovingY = true;
    }
}

public enum ChangeDisplaySide
{
    Right,
    Left,
    Up,
    Down,
    Stay
}