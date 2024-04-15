using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeInfoManager : MonoBehaviour
{
    public GameObject date;
    public GameObject time;

    private Text dateText;
    private Text timeText;
    private GameTime gameTime;

    void Start()
    {
        dateText = date.GetComponent<Text>();
        timeText = time.GetComponent<Text>();
        gameTime = Camera.main.GetComponent<GameTime>();
    }
    void Update()
    {
        dateText.text = gameTime.GetDateString();
        timeText.text = $"{gameTime.GetHour()}:{gameTime.GetMinute().ToString().PadLeft(2, '0')} {gameTime.GetAmPm()}";
    }
}
