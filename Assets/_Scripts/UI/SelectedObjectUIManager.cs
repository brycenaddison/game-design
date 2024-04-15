using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectUIManager : MonoBehaviour
{
    public int OnScreenX;
    public int OffScreenX;
    public Color powerAssetColor;
    public Color customerAssetColor;

    public GameObject assetName;
    public GameObject type;
    public GameObject owner;
    public GameObject power;
    public GameObject money;

    private Text nameText;
    private Text typeText;
    private Text ownerText;
    private Text powerText;
    private Text moneyText;

    private bool visible = false;

    // Start is called before the first frame update
    void Start()
    {
        nameText = assetName.GetComponent<Text>();
        typeText = type.GetComponent<Text>();
        ownerText = owner.GetComponent<Text>();
        powerText = power.GetComponent<Text>();
        moneyText = money.GetComponent<Text>();
        UpdateVisibility();
    }

    void UpdateVisibility()
    {
        RectTransform rectPosition = GetComponent<RectTransform>();
        rectPosition.anchoredPosition = new Vector3(visible ? OnScreenX : OffScreenX, rectPosition.anchoredPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        bool oldVisible = visible;

        GameObject selected = Camera.main.GetComponent<Selector>().Selected;
        if (selected)
        {
            visible = true;
            Asset asset = selected.GetComponent<Asset>();

            if (asset is PowerAsset powerAsset)
            {
                SetPowerText(powerAsset);

            }
            else if (asset is CustomerAsset customerAsset)
            {
                SetCustomerText(customerAsset);
            }
        }
        else
        {
            visible = false;
        }

        if (visible != oldVisible)
        {
            UpdateVisibility();
        }
    }

    void SetPowerText(PowerAsset asset)
    {
        nameText.text = asset.assetName;
        typeText.text = "Power Generator";
        typeText.color = powerAssetColor;
        ownerText.text = "Owner: " + asset.Owner.ownerName;
        moneyText.text = "Expense per day: $" + asset.Upkeep;
        powerText.text = "Power generated: " + asset.PowerGenerated + " units";
    }

    void SetCustomerText(CustomerAsset asset)
    {
        nameText.text = asset.assetName;
        typeText.text = "Customer";
        typeText.color = customerAssetColor;
        if (asset.Owner == null)
        {
            ownerText.text = "No power supplied";
            moneyText.text = "";
            powerText.text = "";
        }
        else
        {
            ownerText.text = "Supplied by: " + asset.Owner.ownerName;
            moneyText.text = "Currently paying: $" + asset.Payment;
            powerText.text = "Power draw: " + asset.Draw + " units";
        }
    }
}
