using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetOwner : MonoBehaviour
{
    public List<Asset> assets;
    public float initialBalanace = 1000;
    public GameObject CityPower;

    public string Name { get; set; }
    public int Id { get; set; }
    public Color Color { get; set; }
    public bool IsPlayable { get; set; }
    public GameObject HQ { get; set; }

    [Header("Read Only")]
    public float balance;

    void Start()
    {
        foreach (Asset asset in assets)
        {
            Claim(asset);
        }

        GameTime gameTime = Camera.main.GetComponent<GameTime>();

        if (IsPlayable)
        {
            Name = StaticProperties.Name;
            Color = StaticProperties.Color;
            Id = 0;
        } else
        {

        }

        balance = initialBalanace;

        gameTime.RegisterOnMonth(1, () =>
        {
            balance += Profit;
            if (balance < 0)
            {
                if (IsPlayable)
                {
                    gameTime.TriggerGameOver();
                } else
                {
                    DeclareBankruptcy();
                }
            }
        }, Id);
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
                    totalPower += powerAsset.PowerGenerated;
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
            if (asset == oldOwner.HQ)
            {
                BuyOut(oldOwner);
            } else
            {
                oldOwner.Unclaim(asset);
            }
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

    public void Purchase(IPurchasable p)
    {
        balance -= p.Cost;
    }

    public bool CanAfford(IPurchasable p)
    {
        return balance >= p.Cost;
    }

    public void BuyOut(AssetOwner other)
    {
        List<Asset> otherAssets = other.assets;

        foreach (Asset asset in otherAssets)
        {
            Claim(asset);
        }

        balance += other.balance;
        other.balance = 0;
    }

    public void DeclareBankruptcy()
    {
        foreach (Asset asset in assets)
        {
            CityPower.GetComponent<AssetOwner>().Claim(asset);
        }

        balance = 0;
    }
}
