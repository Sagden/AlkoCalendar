﻿
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public Data data;
    public string fileName = "/gamesave.save";

    private void Awake()
    {
        if (LoadGame() != null)
        {
            data = LoadGame();
        }
        else
        {
            data = new Data() { date = new List<Year>() };
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    //Сохранение данных в файл
    public void SaveGame()
    {
        string dataJson = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + fileName, dataJson);
    }

    //Загрузка данных из файла
    public Data LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            Data newData = JsonUtility.FromJson<Data>(File.ReadAllText(Application.persistentDataPath + fileName));
            return newData;
        }
        return null;
    }
}
