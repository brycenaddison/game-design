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
    public struct PowerStationUpgrade : IPurchasable
    {
        public string Name;
        public float PowerAdded;
        public float UpkeepAdded;
        [SerializeField]
        private float _cost;
        public float Cost { get => _cost; set => _cost = value; }
    }

    public List<PowerStationUpgrade> _PowerUpgrades;
    public List<PowerStationUpgrade> _UpkeepUpgrades;
 
    // conversion since inspector doesn't support queues
    private Queue<PowerStationUpgrade> UpkeepUpgrades;
    private Queue<PowerStationUpgrade> PowerUpgrades;

    private void Start()
    {
        UpkeepUpgrades = new Queue<PowerStationUpgrade>(_UpkeepUpgrades);
        PowerUpgrades = new Queue<PowerStationUpgrade>(_PowerUpgrades);
    }

    public void BuyUpgrade(UpgradeType upgradeType)
    {
        PowerStationUpgrade upgrade;

        if (UpgradeType.Upkeep == upgradeType)
        {
            if (UpkeepUpgrades.Count == 0)
            {
                return;
            }
            upgrade = UpkeepUpgrades.Dequeue();
        }
        else
        {
            if (PowerUpgrades.Count == 0)
            {
                return;
            }
            upgrade = PowerUpgrades.Dequeue();
        }

        PowerGenerated += upgrade.PowerAdded;
        Upkeep += upgrade.UpkeepAdded;

        Owner.Purchase(upgrade);
    }

    public PowerStationUpgrade? GetUpgrade(UpgradeType upgradeType)
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
