
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public NewDataPanel newDataPanel;
    public DataList dataList;

    public event Action OnNewDataAdded;
    public static event Action OnDeleteData;

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
        if (LoadData() == null)
        {
            dataList.datas.Add(new Data() { date = new List<Year>(), nameCategory = "Категория по умолчанию" });
            SaveData();
        }
        else
        {
            dataList.datas = LoadData().datas;
        }
    }

    //Добавление новой категории
    private void AddNewData(string nameCategory, Sprite sprite)
    {
        dataList.datas.Add(new Data() { nameCategory = nameCategory, icon = sprite, date = new List<Year>() });
        SaveData();
        ModeController.SetMode(dataList.datas.Count - 1);

        OnNewDataAdded?.Invoke();
    }

    //Удаление категории
    public void DeleteData(int index)
    {

        dataList.datas.RemoveAt(index);
        SaveData();

        if (ModeController.currentModeIndex == index)
            ModeController.SetMode(0);

        OnDeleteData.Invoke();

    }


    public void SaveData()
    {

        string dataJson = JsonUtility.ToJson(dataList);

        File.WriteAllText(Application.persistentDataPath + "/data.save", dataJson);

    }

    private DataList LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/data.save"))
        {
            DataList newData = JsonUtility.FromJson<DataList>(File.ReadAllText(Application.persistentDataPath + "/data.save"));
            return newData;
        }
        return null;
    }
}
