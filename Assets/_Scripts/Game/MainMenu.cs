using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public void GotoStory()
    {
        SceneManager.LoadScene("Story");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public GameObject Instructions;

    public void CloseInstrutions()
    {
        gameObject.SetActive(false);
    }

    public void OpenInstrutions()
    {
        gameObject.SetActive(true);
    }
}
