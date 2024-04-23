/**
 * GUI text prompt to get the name of player's company. Currently disabled but kept for testing.
 *
 * Author: Michael
 * Date: 4 / 23 / 24
*/

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
        SceneLoader.LoadStory();
    }
}
