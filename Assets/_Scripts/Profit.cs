using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Profit : MonoBehaviour
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
        text.text = "Profit: " + player.Profit;
    }
}
