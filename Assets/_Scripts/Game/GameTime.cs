using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public enum Speed
    {
        SLOW,
        MED,
        FAST
    }
    
    public float slowHoursPerSecond = 2f;
    public float medHoursPerSecond = 0.5f;
    public float fastHoursPerSecond = 0.1f;
    public Speed speed = Speed.MED;
    public static DateTime startDate = new DateTime(2000, 1, 1, 0, 0, 0);
    public static int minuteInterval = 15;

    [Header("Read Only")]
    public float hour;
    private Boolean paused;
    private DailyEvents[] events;

    private class DailyEvents
    {
        private SortedList<int, Action> events;

        public DailyEvents()
        {
            events = new SortedList<int, Action>();
        }

        public void AddEvent(Action callback, int priority)
        {
            events.Add(priority, callback);
        }

        public void ExecuteEvents()
        {
            foreach (Action callback in events.Values)
            {
                callback();
            }
        }
    }

    void Start()
    {
        hour = 0;
        paused = false;
        events = new DailyEvents[24];
        for (int i = 0; i < 24; i++)
        {
            events[i] = new DailyEvents();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
        }

        if (paused)
        {
            return;
        }

        float lastHour = Mathf.Floor(hour);
        
        switch (speed)
        {
            case Speed.SLOW:
                hour += slowHoursPerSecond * Time.deltaTime;
                break;
            case Speed.MED:
                hour += medHoursPerSecond * Time.deltaTime;
                break;
            case Speed.FAST:
                hour += fastHoursPerSecond * Time.deltaTime;
                break;
        }

        float thisHour = Mathf.Floor(hour);

        if (thisHour != lastHour)
        {
            events[(int) thisHour % 24].ExecuteEvents();
        }
    }

    public bool GetPaused()
    {
        return paused;
    }

    public void RegisterOnHour(int hour, Action callback, int priority)
    {
        events[hour % 24].AddEvent(callback, priority);
    }

    private DateTime CurrentDate => startDate.AddHours(hour);
    public int GetHour()
    {
        return CurrentDate.Hour % 12;
    }

    public string GetAmPm()
    {
        return CurrentDate.Hour < 12 ? "AM" : "PM";
    }

    public int GetMinute()
    {
        return (int) Mathf.Floor(CurrentDate.Minute / minuteInterval) * minuteInterval;
    }

    public String GetDateString()
    {
        return CurrentDate.ToShortDateString();
    }

    public void SetFast()
    {
        speed = Speed.FAST;
    }

    public void SetSlow()
    {
        speed = Speed.SLOW;
    }

    public void SetMed()
    {
        speed = Speed.MED;
    }
}
