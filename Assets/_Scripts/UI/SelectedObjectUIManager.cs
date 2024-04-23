using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
    public GameObject upkeepButton;
    public GameObject powerButton;
    public GameObject bidding;
    public GameObject drop;
    public GameObject bidInput;
    public GameObject bidPlaceholder;
    public GameObject bidText;
    public Button biddingButton;


    private Text nameText;
    private Text typeText;
    private Text ownerText;
    private Text powerText;
    private Text moneyText;

    private GameObject selected;


    private bool visible = false;

    // Start is called before the first frame update
    void Start()
    {
        nameText = assetName.GetComponent<Text>();
        typeText = type.GetComponent<Text>();
        ownerText = owner.GetComponent<Text>();
        powerText = power.GetComponent<Text>();
        moneyText = money.GetComponent<Text>();
        upkeepButton.SetActive(false);
        powerButton.SetActive(false);
        SetBidding(false);
        drop.SetActive(false);
        UpdateVisibility();
    }

    void SetBidding(bool visible)
    {
        bidding.SetActive(visible);
        bidInput.GetComponent<Image>().enabled = visible;
        bidPlaceholder.SetActive(visible);
        bidText.SetActive(visible);
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

        selected = Camera.main.GetComponent<Selector>().Selected;

        if (selected)
        {
            visible = true;
            Asset asset = selected.GetComponent<Asset>();

            SetUpgradeButtons(asset);
            SetBidding(false);

            if (asset is PowerAsset powerAsset)
            {
                SetPowerText(powerAsset);
            }
            else if (asset is CustomerAsset customerAsset)
            {
                SetCustomerText(customerAsset);
                SetBidding(true);
                bidding.GetComponent<CustomerBidding>().SetAsset(customerAsset);
                if (Camera.main.GetComponent<AssetOwner>().CanBidOn(customerAsset) != biddingButton.interactable)
                {
                    biddingButton.interactable = Camera.main.GetComponent<AssetOwner>().CanBidOn(customerAsset);
                }

                drop.SetActive(customerAsset.IsBiddedOnBy(Camera.main.GetComponent<AssetOwner>()));
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

    void SetUpgradeButtons(Asset asset)
    {
        if (asset is UpgradablePowerStation upgradablePowerStation && asset.Owner == Camera.main.GetComponent<AssetOwner>())
        {
            UpgradablePowerStation.PowerStationUpgrade? powerUpgrade = upgradablePowerStation.GetUpgrade(UpgradablePowerStation.UpgradeType.Power);
            UpgradablePowerStation.PowerStationUpgrade? upkeepUpgrade = upgradablePowerStation.GetUpgrade(UpgradablePowerStation.UpgradeType.Upkeep);

            if (powerUpgrade is UpgradablePowerStation.PowerStationUpgrade power)
            {
                powerButton.SetActive(true);
                powerButton.GetComponent<UpgradeButton>().SetUpgrade(power);
            }
            else
            {
                powerButton.SetActive(false);
            }

            if (upkeepUpgrade is UpgradablePowerStation.PowerStationUpgrade upkeep)
            {
                upkeepButton.SetActive(true);
                upkeepButton.GetComponent<UpgradeButton>().SetUpgrade(upkeep);
            }
            else
            {
                upkeepButton.SetActive(false);
            }

        }
        else
        {
            upkeepButton.SetActive(false);
            powerButton.SetActive(false);
        }
    }

    void SetPowerText(PowerAsset asset)
    {
        nameText.text = asset.assetName;
        typeText.text = "Power Generator";
        typeText.color = powerAssetColor;
        ownerText.text = "Owner: " + asset.Owner.Name;
        moneyText.text = "Expense per day: " + asset.Upkeep.ToString("C", CultureInfo.CurrentCulture);
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
            ownerText.text = "Supplied by: " + asset.Owner.Name;
            moneyText.text = "Currently paying: " + asset.Payment.ToString("C", CultureInfo.CurrentCulture);
            powerText.text = "Power draw: " + asset.Draw + " units";
        }
    }

    public void BuyPowerUpgrade()
    {
        selected.GetComponent<UpgradablePowerStation>().BuyUpgrade(UpgradablePowerStation.UpgradeType.Power);
    }

    public void BuyUpkeepUpgrade()
    {
        selected.GetComponent<UpgradablePowerStation>().BuyUpgrade(UpgradablePowerStation.UpgradeType.Upkeep);
    }

    public void DropCustomer()
    {
        selected.GetComponent<CustomerAsset>().RemoveOffer(Camera.main.GetComponent<AssetOwner>());
    }

    public void Bid()
    {
        bidding.GetComponent<CustomerBidding>().Bid(selected.GetComponent<CustomerAsset>());
    }
}
