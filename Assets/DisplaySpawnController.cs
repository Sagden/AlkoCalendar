using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySpawnController : MonoBehaviour
{
    public DataLoader dataLoader;
    public GameObject displaysContainer;
    public MouseSpeedController mouseSpeedController;
    [Space]
    public CalendarGenerate displayPref;
    
    private int currentDisplayIndex = 1;
    public int CurrentDisplayIndex
    {
        get => currentDisplayIndex;
        set
        {
            currentDisplayIndex = value;
            CheckCreateCalendar();
        }
    }

    private List<CalendarGenerate> displaysList = new List<CalendarGenerate>();

    private void Start()
    {
        mouseSpeedController.OnDisplayChange += DisplayChanged;

        DisplaysInit();
    }
    private void OnDestroy()
    {
        mouseSpeedController.OnDisplayChange -= DisplayChanged;
    }

    private void DisplaysInit()
    {
        displaysList.Add(CreateCalendar(new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1), 1440));
        displaysList.Add(CreateCalendar(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), 0));
        displaysList.Add(CreateCalendar(new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1), -1440));

        CurrentDisplayIndex = 1;
    }

    private CalendarGenerate CreateCalendar(DateTime dateTime, float yPosition)
    {
        displayPref.month = dateTime.Month;
        displayPref.year = dateTime.Year;

        CalendarGenerate newCalendar = Instantiate(displayPref, displaysContainer.transform);

        newCalendar.GetComponent<RectTransform>().localPosition = new Vector3(0, yPosition, 0);
        newCalendar.OnSaveChange += SaveDays;

        if (CheckDateInPool(dateTime) != null)
        {
            newCalendar.isFullDay = CheckDateInPool(dateTime);
        }

        return newCalendar;
    }

    private void SaveDays(CalendarGenerate calendarGenerate)
    {
        Year containsYear = ContainsYear(calendarGenerate.year);

        if (containsYear != null) //Есть ли такой год в списке?
        {
            if (ContainsMonth(containsYear, calendarGenerate.month) == null) //Есть ли такой месяц в списке?
            {
                containsYear.months.Add(new Month { month = calendarGenerate.month, isFullDay = calendarGenerate.isFullDay });
            }
        }
        else
        {
            dataLoader.data.date.Add(new Year { year = calendarGenerate.year, months = new List<Month>() });
            ContainsYear(calendarGenerate.year).months.Add(new Month { month = calendarGenerate.month, isFullDay = calendarGenerate.isFullDay });
            Debug.Log("SaveDays else");
        }

        dataLoader.SaveGame();
    }

    //Проверка есть ли в хранилище месяцы этого года?
    private Year ContainsYear(int year)
    {
        foreach (Year yr in dataLoader.data.date)
        {
            if (yr.year == year)
            {
                return yr;
            }
        }

        return null;
    }

    private Month ContainsMonth(Year year, int month)
    {
        foreach (Month mnth in year.months)
        {
            if (mnth.month == month)
            {
                return mnth;
            }
        }

        return null;
    }

    private List<bool> CheckDateInPool(DateTime dateTime)
    {
        foreach(Year year in dataLoader.data.date)
        {
            if (year.year == dateTime.Year)
            {
                foreach(Month month in year.months)
                {
                    if (month.month == dateTime.Month)
                    {
                        return month.isFullDay;
                    }
                }
            }
        }

        return null;
    }

    private void DisplayChanged(ChangeDisplaySide displaySide)
    {
        if (displaySide == ChangeDisplaySide.Up)
            CurrentDisplayIndex++;
        else
            CurrentDisplayIndex--;
    }

    ///<summary> Проверка, может мы у края отображенных месяцев и пора рисовать новый? </summary>
    private void CheckCreateCalendar()
    {
        if (CurrentDisplayIndex == 0)
        {
            displaysList.Insert(0, CreateCalendar(GetPreviousMonthDate(), displaysList[0].transform.localPosition.y + 1440));

            CurrentDisplayIndex = 1;
        }

        if (CurrentDisplayIndex == displaysList.Count - 1)
        {
            displaysList.Add(CreateCalendar(GetNextMonthDate(), displaysList[displaysList.Count - 1].transform.localPosition.y - 1440));
        }
    }

    //Получить предыдущий месяц в displaysList
    private DateTime GetPreviousMonthDate()
    {
        if(displaysList[0].month != 1)
            return new DateTime(displaysList[0].year, displaysList[0].month - 1, 1);
        else
        if (displaysList[0].month == 1)
            return new DateTime(displaysList[0].year - 1, 12, 1);

        return new DateTime(0, 0, 0);
    }

    //Получить следующий месяц в displaysList
    private DateTime GetNextMonthDate()
    {
        if (displaysList[displaysList.Count - 1].month != 12)
            return new DateTime(displaysList[displaysList.Count - 1].year, displaysList[displaysList.Count - 1].month + 1, 1);
        else
        if (displaysList[displaysList.Count - 1].month == 12)
            return new DateTime(displaysList[displaysList.Count - 1].year + 1, 1, 1);

        return new DateTime(0, 0, 0);
    }
}
