using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetOwner : MonoBehaviour
{
    public List<Asset> assets;
    public string ownerName;
    public float initialBalanace = 10000;

    [Header("Read Only")]
    public float balance;

    void Start()
    {
        foreach (Asset asset in assets)
        {
            Claim(asset);
        }
        balance = initialBalanace;
        Camera.main.GetComponent<GameTime>().RegisterOnHour(0, () => balance += Profit, 0);
    }

    public float PowerTotal
    {
        get
        {
            float totalPower = 0;
            foreach (var asset in assets)
            {
                if (asset is PowerAsset powerAsset)
                {
                    totalPower += powerAsset.Upkeep;
                }
            }
            return totalPower;
        }
    }

    public float PowerUsed
    {
        get
        {
            float powerUsed = 0;
            foreach (var asset in assets)
            {
                if (asset is CustomerAsset customerAsset)
                {
                    powerUsed += customerAsset.Draw;
                }
            }
            return powerUsed;
        }
    }

    public float FreePower
    {
        get
        {
            return PowerTotal - PowerUsed;
        }
    }

    public float Revenue
    {
        get
        {
            float revenue = 0;
            foreach (var asset in assets)
            {
                if (asset is CustomerAsset customerAsset)
                {
                    revenue += customerAsset.Payment;
                }
            }
            return revenue;
        }
    }

    public float Expenses
    {
        get
        {
            float expenses = 0;
            foreach (var asset in assets)
            {
                if (asset is PowerAsset powerAsset)
                {
                    expenses += powerAsset.Upkeep;
                }
            }
            return expenses;
        }
    }

    public float Profit
    {
        get
        {
            return Revenue - Expenses;
        }
    }

    public void Claim(Asset asset)
    {
        AssetOwner oldOwner = asset.Owner;
        if (oldOwner == this) return;
        if (oldOwner != null)
        {
            oldOwner.Unclaim(asset);
        }

        if (!assets.Contains(asset))
        {
            assets.Add(asset);
        }
        asset.Owner = this;
    }

    public void Unclaim(Asset asset)
    {
        asset.Owner = null;
        assets.Remove(asset);
    }
}
