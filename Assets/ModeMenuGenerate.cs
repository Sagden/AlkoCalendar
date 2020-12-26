using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeMenuGenerate : MonoBehaviour
{
    public DataLoader dataLoader;

    [Space]

    public MenuButton menuButtonPrefab;
    public Transform buttonsContentContainer;

    private List<MenuButton> menuButtonObjects = new List<MenuButton>();

    private void Start()
    {
        dataLoader.OnNewDataAdded += Generate;
        DataLoader.OnDeleteData += Generate;

        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < menuButtonObjects.Count; i++)
        {
            Destroy(menuButtonObjects[i].gameObject);
        }

        menuButtonObjects.Clear();

        for ( int i = 0; i < 50; i++ )
        {
            if (dataLoader.LoadGame(i) == null) return;

            MenuButton newButton = Instantiate(menuButtonPrefab, buttonsContentContainer);
            newButton.index = i;
            newButton.text.text = dataLoader.LoadGame(i).nameCategory;
            if (dataLoader.LoadGame(i).icon != null) newButton.image.sprite = dataLoader.LoadGame(i).icon;
            menuButtonObjects.Add(newButton);
        }
    }

    private void OnDestroy()
    {
        dataLoader.OnNewDataAdded -= Generate;
        DataLoader.OnDeleteData -= Generate;
    }
}
