using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public MouseEventsController mouseEvents;
    public AnimatedPanel newCategoryButton;

    private void Awake()
    {
        mouseEvents.OnDisplayChange += AddNewCategoryButton;
    }

    private void AddNewCategoryButton(ChangeDisplaySide state)
    {
        if (state == ChangeDisplaySide.Left)
        {
            newCategoryButton.Show();
        }
        if (state == ChangeDisplaySide.Right)
        {
            newCategoryButton.Hide();
        }
    }
}
