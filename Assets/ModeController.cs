using System;
using UnityEngine;


//Хранит текущую выбранную категорию
public class ModeController : MonoBehaviour
{
    public static int currentModeIndex = 0;

    public static event Action OnChangeMode;


    public static void SetMode(int index)
    {
        currentModeIndex = index;
        OnChangeMode?.Invoke();
    }
}
