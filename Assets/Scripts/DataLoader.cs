
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public NewDataPanel newDataPanel;
    [Space][HideInInspector]
    public List<Data> datas;

    public event Action OnNewDataAdded;

    private void Awake()
    {
        DatasInit();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        newDataPanel.OnNewCategoryButtonClick += AddNewData;
    }

    private void DatasInit()
    {
        for (int i = 0; i < 100; i++)
        {
            if (i == 0)
            {
                if (LoadGame(0) == null)
                {
                    datas.Add(new Data() { date = new List<Year>() });

                    return;
                }
            }
            if (LoadGame(i) != null)
                datas.Add(LoadGame(i));
        }
    }

    //Сохранение данных в файл
    public void SaveGame(int index)
    {
        string dataJson = JsonUtility.ToJson(datas[index]);

        File.WriteAllText(Application.persistentDataPath + "/" + index.ToString() + ".save", dataJson);
    }

    //Загрузка данных из файла
    public Data LoadGame(int index)
    {
        if (File.Exists(Application.persistentDataPath + "/" + index.ToString() + ".save"))
        {
            Data newData = JsonUtility.FromJson<Data>(File.ReadAllText(Application.persistentDataPath + "/" + index.ToString() + ".save"));
            return newData;
        }
        return null;
    }

    //Добавление новой категории
    private void AddNewData(string nameCategory, Sprite sprite)
    {
        for (int i = 0; i < 100; i++)
        {
            if(LoadGame(i) == null)
            {
                datas.Add(new Data() { nameCategory = nameCategory, icon = sprite, date = new List<Year>() });
                SaveGame(i);
                ModeController.SetMode(i);

                OnNewDataAdded?.Invoke();

                return;
            }
        }
    }
}
