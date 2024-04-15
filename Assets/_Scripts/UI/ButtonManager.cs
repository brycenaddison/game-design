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

    private void Start()
    {
        slowButton = slow.GetComponent<Button>();
        medButton = med.GetComponent<Button>();
        fastButton = fast.GetComponent<Button>();
    }

    // doesn't seem very efficient
    void Update()
    {
        switch (Camera.main.GetComponent<GameTime>().speed)
        {
            case GameTime.Speed.SLOW:
                slowButton.interactable = false;
                medButton.interactable = true;
                fastButton.interactable = true;
                break;
            case GameTime.Speed.MED:
                medButton.interactable = false;
                slowButton.interactable = true;
                fastButton.interactable = true;
                break;
            case GameTime.Speed.FAST:
                fastButton.interactable = false;
                slowButton.interactable = true;
                medButton.interactable = true;
                break;

        }
    }
}
