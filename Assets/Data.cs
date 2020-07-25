using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Data
{
    public List<Year> date = new List<Year>();
}

[Serializable]
public class Year
{
    public int year;
    public List<Month> months = new List<Month>();
}

[Serializable]
public class Month
{
    public int month;
    public List<bool> isFullDay = new List<bool>();
}