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

    public float slowMonthsPerSecond = 0.1f;
    public float medMonthsPerSecond = 0.5f;
    public float fastMonthsPerSecond = 2f;
    public float skipMonthsPerSecond = 8f;

    public Speed speed = Speed.MED;
    public static DateTime startDate = new DateTime(2000, 1, 1, 0, 0, 0);

    [Header("Read Only")]
    public float month;
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
        month = 0;
        paused = false;
        gameOver = false;
        events = new DailyEvents[12];
        for (int i = 0; i < 12; i++)
        {
            events[i] = new DailyEvents();
        }
        RegisterOnMonth(
            1,
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

        int lastMonth = Mathf.FloorToInt(month);

        switch (speed)
        {
            case Speed.SLOW:
                month += slowMonthsPerSecond * Time.deltaTime;
                break;
            case Speed.MED:
                month += medMonthsPerSecond * Time.deltaTime;
                break;
            case Speed.FAST:
                month += fastMonthsPerSecond * Time.deltaTime;
                break;
            case Speed.SKIP:
                month += skipMonthsPerSecond * Time.deltaTime;
                break;
        }

        int thisMonth = Mathf.FloorToInt(month);

        if (thisMonth != lastMonth)
        {
            events[thisMonth % 12].ExecuteEvents();
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

    public void RegisterOnMonth(int month, Action callback, int priority)
    {
        events[(month - 1) % 12].AddEvent(callback, priority);
    }

    private DateTime CurrentDate => startDate.AddMonths(Mathf.FloorToInt(month));

    public string GetDateString()
    {
        return CurrentDate.ToString("MMMM yyyy");
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
