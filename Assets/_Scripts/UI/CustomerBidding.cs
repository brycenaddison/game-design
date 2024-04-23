/**
 * Text input for a custom bid on a neighboring asset.
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBidding : MonoBehaviour
{
    public Text inputText;
    public Text outputText;

    public void SetAsset(CustomerAsset asset)
    {
        outputText.text = String.Join("\n", asset.Offers);
    }

    public void Bid(CustomerAsset asset)
    {
        try
        {
            float value = float.Parse(inputText.text, CultureInfo.InvariantCulture);
            asset.Offer(Camera.main.GetComponent<AssetOwner>(), Mathf.Floor(value * 100) / 100);
        }
        catch (FormatException)
        {
            Debug.Log($"bid invalid: \"{inputText.text}\"");
        }
    }
}
