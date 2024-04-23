using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Profit : MonoBehaviour
{
    private AssetOwner player;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();                                      // d
        player = Camera.main.GetComponent<AssetOwner>();
    }

    void Update()
    {
        text.text = $"Balance: {player.balance.ToString("C", CultureInfo.CurrentCulture)}    Profit: {player.Profit.ToString("C", CultureInfo.CurrentCulture)}";
    }
}
