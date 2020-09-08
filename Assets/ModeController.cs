using System;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    public static int currentModeIndex = 0;

    public static event Action OnChangeMode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentModeIndex++;
            OnChangeMode?.Invoke();
        }
    }

    public static void SetMode(int index)
    {
        currentModeIndex = index;
        OnChangeMode?.Invoke();
    }
}
