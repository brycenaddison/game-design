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

    // doesn't seem very efficient
    void Update()
    {
        Button slowButton = slow.GetComponent<Button>();
        Button medButton = med.GetComponent<Button>();
        Button fastButton = fast.GetComponent<Button>();

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
