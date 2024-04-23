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

    public enum GameState
    {
        PLAYING,
        PAUSED,
        LOST,
        WON,
    }

    public float slowMonthsPerSecond = 0.1f;
    public float medMonthsPerSecond = 0.5f;
    public float fastMonthsPerSecond = 2f;
    public float skipMonthsPerSecond = 8f;

    public Speed speed = Speed.MED;
    public static DateTime startDate = new DateTime(2000, 1, 1, 0, 0, 0);

    [Header("Read Only")]
    public float month;
    private GameState state;
    private DailyEvents[] events;

    private class DailyEvents
    {
        private List<(int, Action)> events;

        public DailyEvents()
        {
            events = new List<(int, Action)>();
        }

        public void AddEvent(Action callback, int priority)
        {
            events.Add((-1 * priority, callback));
        }

        public void RemoveEvent(Action callback)
        {
            events.RemoveAll((tuple) => tuple.Item2 == callback);
        }

        public void ExecuteEvents()
        {
            events.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            foreach ((int, Action) tuple in events)
            {
                Action callback = tuple.Item2;
                callback();
            }
        }
    }

    void Start()
    {
        state = GameState.PLAYING;
        month = 0;
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
        if (state == GameState.WON || state == GameState.LOST) return;

        if (Input.GetButtonDown("Cancel"))
        {
            state = state == GameState.PAUSED ? GameState.PLAYING : GameState.PAUSED;
            Time.timeScale = state == GameState.PAUSED ? 0f : 1f;
        }

        if (state == GameState.PAUSED)
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

    public GameState GetState()
    {
        return state;
    }

    public void TriggerLose()
    {
        state = GameState.LOST;
    }

    public void TriggerWin()
    {
        state = GameState.WON;
    }

    public Action RegisterOnMonth(int month, Action callback, int priority)
    {
        events[(month - 1) % 12].AddEvent(callback, priority);

        return () => events[(month - 1) % 12].RemoveEvent(callback);
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

    public int GetMonth()
    {
        return Mathf.FloorToInt(month) % 12 + 1;
    }   
}
