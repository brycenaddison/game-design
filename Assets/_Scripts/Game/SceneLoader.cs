using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// monobehaviour just to attach/reference if needed
public class SceneLoader : MonoBehaviour
{
    public static void LoadGame()
    {
        SceneManager.LoadScene("PowerStruggle");
    }

    public static void LoadTitle()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
