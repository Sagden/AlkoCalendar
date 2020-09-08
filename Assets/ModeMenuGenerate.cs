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
            menuButtonObjects.Add(newButton);
        }
    }

    private void OnDestroy()
    {
        dataLoader.OnNewDataAdded -= Generate;
    }
}
