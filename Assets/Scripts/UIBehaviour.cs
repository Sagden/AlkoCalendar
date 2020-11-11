using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    public MouseEventsController mouseEventsController;

    [Space]

    public RectTransform mainDisplay;
    private Vector2 mainDisplayNextPosition;
    private Vector2 mainDisplayStartPosition;

    [Space]

    public RectTransform menuDisplay;
    public ScrollRect menuScrollRect;
    private Vector2 menuDisplayNextPosition;
    private Vector2 menuDisplayStartPosition;

    public event Action<UIMode> OnChangeMode;
    public event Action<ChangeDisplaySide> OnChangeDisplayY;

    private UIMode currentMode = UIMode.Main;
    public UIMode CurrentMode { get => currentMode; set => currentMode = value; }

    private MovedMove movedMode = MovedMove.Null;

    private void Start()
    {
        mouseEventsController.OnDisplayChange += DisplayChange;
        mouseEventsController.OnPressedX += canvasMovedX;
        mouseEventsController.OnPressedY += canvasMovedY;

        ModeController.OnChangeMode += () => DisplayChangeX(ChangeDisplaySide.Right);
        ModeController.OnChangeMode += ResetPosition;

        mainDisplayNextPosition = mainDisplay.transform.localPosition;
        menuDisplayNextPosition = menuDisplay.transform.localPosition;
    }

    private void ResetPosition()
    {
        mainDisplayNextPosition = new Vector2(0, 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DOTween.KillAll();

            mainDisplayStartPosition = mainDisplay.transform.localPosition;
            menuDisplayStartPosition = menuDisplay.transform.localPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            menuScrollRect.enabled = true;

            mainDisplay.transform.DOLocalMove(mainDisplayNextPosition, 0.7f).OnComplete(() => movedMode = MovedMove.Null);
            menuDisplay.transform.DOLocalMove(menuDisplayNextPosition, 0.7f).OnComplete(() => movedMode = MovedMove.Null);
        }
    }
    //TODO: Сделать для Android

    private void canvasMovedX(float offsetX, float offsetY)
    {
        menuScrollRect.enabled = false;

        if (movedMode == MovedMove.Null)
            movedMode = MovedMove.xMode;

        if (movedMode == MovedMove.xMode)
        {
            mainDisplay.transform.localPosition = new Vector3(mainDisplayStartPosition.x + offsetX, mainDisplay.transform.localPosition.y, 0);
            menuDisplay.transform.localPosition = new Vector3(menuDisplayStartPosition.x + offsetX, menuDisplay.transform.localPosition.y, 0);
        }
        else
        {
            mainDisplay.transform.localPosition = new Vector3(mainDisplay.transform.localPosition.x, mainDisplayStartPosition.y + offsetY, 0);
            menuDisplay.transform.localPosition = new Vector3(menuDisplay.transform.localPosition.x, menuDisplayStartPosition.y + offsetY, 0);
        }
    }
    private void canvasMovedY(float offsetY, float offsetX)
    {
        if (currentMode == UIMode.Menu) return;

        if (movedMode == MovedMove.Null)
            movedMode = MovedMove.yMode;

        if (movedMode == MovedMove.yMode)
        {
            mainDisplay.transform.localPosition = new Vector3(mainDisplay.transform.localPosition.x, mainDisplayStartPosition.y + offsetY, 0);
            menuDisplay.transform.localPosition = new Vector3(menuDisplay.transform.localPosition.x, menuDisplayStartPosition.y + offsetY, 0);
        }
        else
        {
            mainDisplay.transform.localPosition = new Vector3(mainDisplayStartPosition.x + offsetX, mainDisplay.transform.localPosition.y, 0);
            menuDisplay.transform.localPosition = new Vector3(menuDisplayStartPosition.x + offsetX, menuDisplay.transform.localPosition.y, 0);
        }
    }

    private void DisplayChange(ChangeDisplaySide state)
    {

        if (movedMode == MovedMove.xMode)
        {
            DisplayChangeX(state);
            return;
        }

        if (state == ChangeDisplaySide.Down || state == ChangeDisplaySide.Up)
        {
            if (movedMode == MovedMove.Null || movedMode == MovedMove.yMode)
            {
                DisplayChangeY(state);
                movedMode = MovedMove.yMode;
            }
            else
            {
                DisplayChangeX(state);
            }
        }
        else
        if (state == ChangeDisplaySide.Left || state == ChangeDisplaySide.Right)
        {
            if (movedMode == MovedMove.Null || movedMode == MovedMove.xMode)
            {
                DisplayChangeX(state);
                movedMode = MovedMove.xMode;
            }
            else
            {
                DisplayChangeY(state);
            }
        }

    }

    private void DisplayChangeY(ChangeDisplaySide state)
    {

        if (currentMode != UIMode.Main) return;

            switch (state)
            {
                case ChangeDisplaySide.Down: mainDisplayNextPosition = new Vector2(mainDisplayNextPosition.x, mainDisplayNextPosition.y - 1440); break;
                case ChangeDisplaySide.Up: mainDisplayNextPosition = new Vector2(mainDisplayNextPosition.x, mainDisplayNextPosition.y + 1440); break;
            }

        mainDisplay.transform.DOLocalMove(mainDisplayNextPosition, 0.7f);
        OnChangeDisplayY.Invoke(state);

    }

    private void DisplayChangeX(ChangeDisplaySide state)
    {

        if (currentMode == UIMode.Main && state == ChangeDisplaySide.Left)
        {
            currentMode = UIMode.Menu;

            OnChangeMode.Invoke(currentMode);

            mainDisplayNextPosition = new Vector2(mainDisplayNextPosition.x + 864, mainDisplayNextPosition.y);
            menuDisplayNextPosition = new Vector2(menuDisplayNextPosition.x + 864, menuDisplayNextPosition.y);
        }
        else
        if (currentMode == UIMode.Menu && state == ChangeDisplaySide.Right)
        {
            currentMode = UIMode.Main;

            OnChangeMode.Invoke(currentMode);

            mainDisplayNextPosition = new Vector2(mainDisplayNextPosition.x - 864, mainDisplayNextPosition.y);
            menuDisplayNextPosition = new Vector2(menuDisplayNextPosition.x - 864, menuDisplayNextPosition.y);
        }

        mainDisplay.transform.DOLocalMove(mainDisplayNextPosition, 0.7f).OnComplete(() => movedMode = MovedMove.Null);
        menuDisplay.transform.DOLocalMove(menuDisplayNextPosition, 0.7f).OnComplete(() => movedMode = MovedMove.Null);

    }
}

public enum MovedMove
{
    yMode,
    xMode,
    Null
}

public enum UIMode
{
    Main,
    Menu
}
