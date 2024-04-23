using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject skip;
    public GameObject slow;
    public GameObject med;
    public GameObject fast;

    private Button slowButton;
    private Button medButton;
    private Button fastButton;
    private Button skipButton;
    private GameTime gameTime;

    private void Start()
    {
        slowButton = slow.GetComponent<Button>();
        medButton = med.GetComponent<Button>();
        fastButton = fast.GetComponent<Button>();
        skipButton = skip.GetComponent<Button>();
        gameTime = Camera.main.GetComponent<GameTime>();
    }

    // doesn't seem very efficient
    void Update()
    {
        if (gameTime.GetState() != GameTime.GameState.PLAYING)
        {
            slowButton.interactable = false;
            medButton.interactable = false;
            fastButton.interactable = false;
            skipButton.interactable = false;

            return;
        }

        switch (gameTime.speed)
        {
            case GameTime.Speed.SLOW:
                slowButton.interactable = false;
                medButton.interactable = true;
                fastButton.interactable = true;
                skipButton.interactable = true;

                break;
            case GameTime.Speed.MED:
                medButton.interactable = false;
                slowButton.interactable = true;
                fastButton.interactable = true;
                skipButton.interactable = true;

                break;
            case GameTime.Speed.FAST:
                fastButton.interactable = false;
                slowButton.interactable = true;
                medButton.interactable = true;
                skipButton.interactable = true;

                break;
            case GameTime.Speed.SKIP:
                slowButton.interactable = true;
                medButton.interactable = true;
                fastButton.interactable = true;
                skipButton.interactable = false;

                break;
        }
    }
}
