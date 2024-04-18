using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBidding : MonoBehaviour
{
    public GameObject inputTextBox;
    public GameObject outputTextBox;

    private Text inputText;
    private Text outputText;

    // Start is called before the first frame update
    void Start()
    {
        inputText = inputTextBox.GetComponent<Text>();
        outputText = outputTextBox.GetComponent<Text>();
    }

    // Update is called once per frame
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
            // TODO: idk better handling
            Debug.Log($"bid invalid: \"{inputText.text}\"");
        }
    }
}
