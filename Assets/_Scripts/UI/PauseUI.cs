using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private bool previousState;
    private GameTime gameTime;

    void Start()
    {
        gameTime = Camera.main.GetComponent<GameTime>();
        previousState = gameTime.GetPaused();
        SetVisible(previousState);
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
        bool newState = gameTime.GetPaused();
        if (newState != previousState)
        {

            SetVisible(newState);
            previousState = newState;
        }
    }
}
