/**
 * GUI component indicating the player's free power. Free power is
 * the total amount of power a company produces less the total amount
 * of power its consumers use.
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FreePower : MonoBehaviour
{
    private AssetOwner player;
    private Text text;


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();                                      // d
        player = Camera.main.GetComponent<AssetOwner>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Excess Power: " + player.FreePower + " MWh";
    }
}
