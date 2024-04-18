using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public GameObject upgradeName;
    public GameObject cost;
    public GameObject modifier;

    private Text nameText;
    private Text costText;
    private Text modifierText;

    void Start()
    {
        nameText = upgradeName.GetComponent<Text>();
        costText = cost.GetComponent<Text>();
        modifierText = modifier.GetComponent<Text>();
    }

    public void SetUpgrade(UpgradablePowerStation.PowerStationUpgrade upgrade)
    {
        nameText.text = upgrade.Name;
        GetComponent<Button>().interactable = Camera.main.GetComponent<AssetOwner>().CanAfford(upgrade);
        costText.text = upgrade.Cost.ToString("C", CultureInfo.CurrentCulture);
        List<string> modifiers = new List<string>();
        if (upgrade.UpkeepAdded != 0)
        {
            modifiers.Add((upgrade.UpkeepAdded > 0 ? "+$" : "-$") + Mathf.Abs(upgrade.UpkeepAdded) + " Upkeep");
        }
        if (upgrade.PowerAdded != 0)
        {
            modifiers.Add((upgrade.PowerAdded > 0 ? "+" : "") + upgrade.PowerAdded + " Power");
        }
        modifierText.text = string.Join("\n", modifiers);
    }
}
