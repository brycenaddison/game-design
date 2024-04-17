using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public bool isGameOver;
    private bool previousState;
    private GameTime gameTime;

    void Start()
    {
        gameTime = Camera.main.GetComponent<GameTime>();
        previousState = Visible();
        SetVisible(previousState);
    }

    private bool Visible()
    {
        return isGameOver ? gameTime.IsGameOver() : gameTime.GetPaused();

    }

    private void SetVisible(bool paused)
    {
        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(paused);
        }
    }

    void Update()
    {
        bool newState = Visible();
        if (newState != previousState)
        {

            SetVisible(newState);
            previousState = newState;
        }
    }
}
