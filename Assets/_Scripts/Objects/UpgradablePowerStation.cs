using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradablePowerStation : PowerAsset
{
    public enum UpgradeType
    {
        Power,
        Upkeep
    }

    [Serializable]
    public struct Upgrade : IPurchasable
    {
        public string Name;
        public float Value;
        [SerializeField]
        private float _cost;
        public float Cost { get => _cost; set => _cost = value; }
    }

    public List<Upgrade> _PowerUpgrades;
    public List<Upgrade> _UpkeepUpgrades;

    // conversion since inspector doesn't support queues
    private Queue<Upgrade> UpkeepUpgrades;
    private Queue<Upgrade> PowerUpgrades;

    private void Start()
    {
        UpkeepUpgrades = new Queue<Upgrade>(_UpkeepUpgrades);
        PowerUpgrades = new Queue<Upgrade>(_PowerUpgrades);
    }

    void BuyUpgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case (UpgradeType.Power):
                if (PowerUpgrades.Count == 0)
                {
                    return;
                }
                Upgrade powerUpgrade = PowerUpgrades.Dequeue();
                PowerGenerated += powerUpgrade.Value;
                Owner.Purchase(powerUpgrade);
                break;
            case (UpgradeType.Upkeep):
                if (UpkeepUpgrades.Count == 0)
                {
                    return;
                }
                Upgrade upkeepUpgrade = UpkeepUpgrades.Dequeue();
                PowerGenerated += upkeepUpgrade.Value;
                Owner.Purchase(upkeepUpgrade);
                break;
        }
    }
    
    Upgrade? GetUpgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case (UpgradeType.Power):
                if (PowerUpgrades.Count == 0)
                {
                    return null;
                }
                return PowerUpgrades.Peek();
            case (UpgradeType.Upkeep):
                if (UpkeepUpgrades.Count == 0)
                {
                    return null;
                }
                return UpkeepUpgrades.Peek();
            default:
                return null;
        }
    }   
}
