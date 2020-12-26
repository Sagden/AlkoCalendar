using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[System.Serializable]
public class CalendarGenerate : MonoBehaviour
{
    public int month;
    public int year;
    public string categoryName;
    public Sprite categoryIcon;
    public List<bool> isFullDay = new List<bool>();
    public List<SwitchWine> switchWines = new List<SwitchWine>();
    public event Action<CalendarGenerate> OnSaveChange;
    [Space]
    [SerializeField] private GameObject horizontalGridPrefab;
    [SerializeField] private GameObject wineGlass;
    [SerializeField] private GameObject emptyObject;
    [SerializeField] private GameObject verticalGridContainer;
    [SerializeField] private TMP_Text monthTextContainer;
    [SerializeField] private TMP_Text yearTextContainer;
    [SerializeField] private TMP_Text categoryTextContainer;
    [Space]
    public List<GameObject> weeklyViewPrefs = new List<GameObject>();

    private int horizontalPrefsCount;
    private List<GameObject> horizontalPrefs = new List<GameObject>();

    private void Start()
    {
        DateTime date = new DateTime(year, month, 1);
        horizontalPrefsCount = GetDaysInMonth(date) + 1;
        HorizontalGridObjectsGenerate();
        ContentGenerate(date);
        MonthTextGenerate(month);

        yearTextContainer.text = year.ToString();
        categoryTextContainer.text = categoryName;
    }

    //Расчет количества строк в месяце
    private int GetDaysInMonth(DateTime dateTime)
    {
        int daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        switch(daysInMonth)
        {
            case 28:
                if (dateTime.DayOfWeek.ToString() == "Monday")
                    return 4;
                else
                    return 5;

            case 29:
                return 5;

            case 30:
                if (dateTime.DayOfWeek.ToString() == "Sunday")
                    return 6;
                else
                    return 5;

            case 31:
                if (dateTime.DayOfWeek.ToString() == "Sunday" || dateTime.DayOfWeek.ToString() == "Saturday")
                    return 6;
                else
                    return 5;

            default: return 0;
        }
    }
    private void HorizontalGridObjectsGenerate()
    {
        for (int i = 0; i < horizontalPrefsCount; i++)
        {
            horizontalPrefs.Add(Instantiate(horizontalGridPrefab, verticalGridContainer.transform));
        }
    }
    private void ContentGenerate(DateTime dateTime)
    {
        int count = 0;

        for (int i = 1; i <= 7; i++)
        {
            if (i >= GetDayOfWeek(dateTime))
            {
                if (isFullDay.Count == DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
                {
                    var switchWine = Instantiate(wineGlass, horizontalPrefs[0].transform).GetComponent<SwitchWine>();
                    switchWine.Index = count;
                    switchWine.image.sprite = categoryIcon;
                    switchWines.Add(switchWine);

                    switchWines[count].OnSwitchOn += ChangeSwitchWineState;

                    if (isFullDay[count])
                        switchWine.SwitchOn();

                    count++;
                }
                else
                {
                    var switchWine = Instantiate(wineGlass, horizontalPrefs[0].transform).GetComponent<SwitchWine>();
                    switchWine.Index = count;
                    switchWine.image.sprite = categoryIcon;
                    switchWines.Add(switchWine);
                    isFullDay.Add(false);
                    switchWines[count].OnSwitchOn += ChangeSwitchWineState;

                    count++;
                }
                
            }
            else
                Instantiate(emptyObject, horizontalPrefs[0].transform);
        } // Заполняем первую полосу

        for (int i = 1; i < horizontalPrefsCount - 1; i++)
        {
            for (int ii = 1; ii <= 7; ii++)
            {
                if (count < DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
                {
                    if (isFullDay.Count == DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
                    {
                        var switchWine = Instantiate(wineGlass, horizontalPrefs[i].transform).GetComponent<SwitchWine>();
                        switchWine.Index = count;
                        switchWine.image.sprite = categoryIcon;
                        switchWines.Add(switchWine);

                        switchWines[count].OnSwitchOn += ChangeSwitchWineState;

                        if (isFullDay[count])
                            switchWine.GetComponent<SwitchWine>().SwitchOn();

                        count++;
                    }
                    else
                    {
                        var switchWine = Instantiate(wineGlass, horizontalPrefs[i].transform).GetComponent<SwitchWine>();
                        switchWine.Index = count;
                        switchWine.image.sprite = categoryIcon;
                        switchWines.Add(switchWine);
                        isFullDay.Add(false);
                        switchWines[count].OnSwitchOn += ChangeSwitchWineState;
                        
                        count++;
                    }
                }
                else
                {
                    Instantiate(emptyObject, horizontalPrefs[i].transform);
                }
            }
        } //Заполняем остальные строки (кроме последней)

        for (int i = 0; i <= 6; i++)
        {
            Instantiate(weeklyViewPrefs[i], horizontalPrefs[horizontalPrefsCount - 1].transform);
        }
    }

    private void ChangeSwitchWineState(int index, bool state)
    {
        isFullDay[index] = state;

        OnSaveChange.Invoke(this);
    }
    private void MonthTextGenerate(int month)
    {
        string monthName;

        switch(month)
        {
            case 1: monthName = "Январь"; break;
            case 2: monthName = "Февраль"; break;
            case 3: monthName = "Март"; break;
            case 4: monthName = "Апрель"; break;
            case 5: monthName = "Май"; break;
            case 6: monthName = "Июнь"; break;
            case 7: monthName = "Июль"; break;
            case 8: monthName = "Август"; break;
            case 9: monthName = "Сентябрь"; break;
            case 10: monthName = "Октябрь"; break;
            case 11: monthName = "Ноябрь"; break;
            case 12: monthName = "Декабрь"; break;
            default: monthName = "Ошибка"; break;
        }

        monthTextContainer.text = monthName;
    }
    private int GetDayOfWeek(DateTime dateTime)
    {
        if ((int)dateTime.DayOfWeek == 0)
            return 7;
        else
            return (int)dateTime.DayOfWeek;
    }
}