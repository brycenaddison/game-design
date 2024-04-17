using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NamePrompt : MonoBehaviour
{
    public GameObject text;

    // TODO: protect input
    public void OnEnter()
    {
        StaticProperties.Name = text.GetComponent<Text>().text;
        SceneLoader.LoadGame();
    }
}
