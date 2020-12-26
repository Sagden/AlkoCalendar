
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public NewDataPanel newDataPanel;
    [Space][SerializeField]
    public List<Data> datas;
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
        for (int i = 0; i < 100; i++)
        {
            if (i == 0)
            {
                if (LoadGame(0) == null)
                {
                    dataList.datas.Add(new Data() { date = new List<Year>(), nameCategory = "Категория по умолчанию" });
                    datas.Add(new Data() { date = new List<Year>(), nameCategory = "Категория по умолчанию" });

                    return;
                }
            }
            if (LoadGame(i) != null)
            {
                dataList.datas.Add(LoadGame(i));
                datas.Add(LoadGame(i));
            }
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

    //Удаление категории
    public static void DeleteData(int index)
    {

        if (File.Exists(Application.persistentDataPath + "/" + index.ToString() + ".save"))
        {
            File.Delete(Application.persistentDataPath + "/" + index.ToString() + ".save");

            //Попытка сделать смещение категорий
            for (int i = index + 1; i < 200; i++)
            {

                if (File.Exists(Application.persistentDataPath + "/" + i.ToString() + ".save"))
                {
                    File.Move(Application.persistentDataPath + "/" + i.ToString() + ".save", Application.persistentDataPath + "/" + (i - 1).ToString() + ".save");
                }
                else
                {
                    break;
                }
            }

            OnDeleteData.Invoke();

        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SaveData();
        }
    }

    private void SaveData()
    {

        string dataJson = JsonUtility.ToJson(dataList);

        File.WriteAllText(Application.persistentDataPath + "/data.save", dataJson);

    }
}
