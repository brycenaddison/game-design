/**
 * GUI menu to display information about a building - owner, revenue, power draw, etc.
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsMenu : MonoBehaviour
{
    public GameObject panel;
    public void Open()
    {
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
