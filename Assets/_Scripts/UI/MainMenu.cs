/**
 * Loads the main menu scene on start-up.
 *
 * Author: Michael, Jack
 * Date: 4 / 23 / 24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject startGameButton;
    //public GameObject namePrompt;

    public void Start()
    {
        //namePrompt.SetActive(false);
    }

    public void NewGame()
    {
        //namePrompt.SetActive(true);
        startGameButton.GetComponent<Button>().interactable = false;
    }

    public static void LoadStory()
    {
        Debug.Log("Loading story");
        SceneManager.LoadScene("Story");
        Debug.Log("Loaded story");
    }
}
