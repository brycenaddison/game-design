/**
 * Changes the scene when certain buttons are pressed. In general scenes will follow
 * this order: Title Screen -> Story -> Settings -> Game
 *
 * Author: Brycen, Michael, Jack
 * Date: 4 / 23 / 24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// monobehaviour just to attach/reference if needed
public class SceneLoader : MonoBehaviour
{
    public static void LoadGame()
    {
        Debug.Log("Loading game");
        SceneManager.LoadScene("PowerStruggle");
        Debug.Log("Loaded game");
    }

    public static void LoadTitle()
    {
        Debug.Log("Loading title");
        SceneManager.LoadScene("Title Screen");
        Debug.Log("Loaded title");
    }

    public static void LoadStory()
    {
        Debug.Log("Loading story");
        SceneManager.LoadScene("Story");
        Debug.Log("Loaded story");
    }
        
    public static void LoadSettings()
    {
        Debug.Log("Loading settings");
        SceneManager.LoadScene("Settings");
        Debug.Log("Loaded settings");
    }
}
