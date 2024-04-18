using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject startGameButton;
    public GameObject namePrompt;

    public void Start()
    {
        namePrompt.SetActive(false);
    }

    public void NewGame()
    {
        namePrompt.SetActive(true);
        startGameButton.GetComponent<Button>().interactable = false;
    }
}
