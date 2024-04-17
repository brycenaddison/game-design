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
        FAST,
        SKIP,
    }

    public float slowHoursPerSecond = 0.1f;
    public float medHoursPerSecond = 0.5f;
    public float fastHoursPerSecond = 2f;
    public float skipHoursPerSecond = 8f;

    public Speed speed = Speed.MED;
    public static DateTime startDate = new DateTime(2000, 1, 1, 0, 0, 0);
    public static int minuteInterval = 15;

    [Header("Read Only")]
    public float hour;
    private Boolean paused;
    private Boolean gameOver;
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
            events.Add(-1 * priority, callback);
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
        gameOver = false;
        events = new DailyEvents[24];
        for (int i = 0; i < 24; i++)
        {
            events[i] = new DailyEvents();
        }
        RegisterOnHour(
            0,
            () =>
            {
                if (speed == Speed.SKIP)
                {
                    speed = Speed.MED;
                }
            },
            1000
        );
    }

    void Update()
    {
        if (gameOver) return;

        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
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
            case Speed.SKIP:
                hour += skipHoursPerSecond * Time.deltaTime;
                break;
        }

        float thisHour = Mathf.Floor(hour);

        if (thisHour != lastHour)
        {
            events[(int)thisHour % 24].ExecuteEvents();
        }
    }

    public bool GetPaused()
    {
        return paused;
    }

    public void TriggerGameOver()
    {
        gameOver = true;
        paused = true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void RegisterOnHour(int hour, Action callback, int priority)
    {
        events[hour % 24].AddEvent(callback, priority);
    }

    private DateTime CurrentDate => startDate.AddHours(hour);
    public int GetHour()
    {
        return CurrentDate.Hour % 12 == 0 ? 12 : CurrentDate.Hour % 12;
    }

    public string GetAmPm()
    {
        return CurrentDate.Hour < 12 ? "AM" : "PM";
    }

    public int GetMinute()
    {
        return (int)Mathf.Floor(CurrentDate.Minute / minuteInterval) * minuteInterval;
    }

    public string GetDateString()
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

    public void Skip()
    {
        speed = Speed.SKIP;
    }
}
