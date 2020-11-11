using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIBehaviour uiBehaviour;
    public AnimatedPanel newCategoryButton;

    private void Awake()
    {
        uiBehaviour.OnChangeMode += AddNewCategoryButtonVisible;
    }

    private void AddNewCategoryButtonVisible(UIMode state)
    {

        if (state == UIMode.Menu)
        {
            newCategoryButton.Show();
        }
        if (state == UIMode.Main)
        {
            newCategoryButton.Hide();
        }

    }

    private void OnDestroy()
    {
        uiBehaviour.OnChangeMode -= AddNewCategoryButtonVisible;
    }
}
