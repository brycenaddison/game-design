/**
 * GUI element to display the current month and year to the player.
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeInfoManager : MonoBehaviour
{
    private GameTime gameTime;
    private Text dateText;

    void Start()
    {
        dateText = gameObject.GetComponent<Text>();
        gameTime = Camera.main.GetComponent<GameTime>();
    }
    void Update()
    {
        dateText.text = gameTime.GetDateString();
    }
}
