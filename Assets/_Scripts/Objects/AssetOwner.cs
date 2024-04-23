using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetOwner : MonoBehaviour
{
    public List<Asset> assets;
    public string ownerName;
    public int id;
    public Boolean isPlayable;
    public float initialBalance = 1000;
    public Scoreboard scoreboard;

    public static int BidWindowStart = 7;
    public static int BidWindowEnd = 9;

    [Header("Read Only")]
    public float balance;

    void Start()
    {
        foreach (Asset asset in assets)
        {
            Claim(asset);
        }

        scoreboard?.Register(this);

        GameTime gameTime = Camera.main.GetComponent<GameTime>();

        if (isPlayable)
        {
            ownerName = StaticProperties.Name;
        }

        balance = initialBalance;

        gameTime.RegisterOnMonth(1, () =>
        {
            balance += Profit;
            if (balance < 0)
            {
                if (isPlayable) gameTime.TriggerGameOver(); // need to have AI drop assets
            }
        }, id);
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
        AssetOwner oldOwner = asset.CurrentOwner;
        if (oldOwner == this) return;
        if (oldOwner != null)
        {
            oldOwner.Unclaim(asset);
        }

        if (!assets.Contains(asset))
        {
            assets.Add(asset);
        }
        asset.CurrentOwner = this;
    }

    public void Unclaim(Asset asset)
    {
        asset.CurrentOwner = null;
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

    public bool CanBidOn(CustomerAsset customerAsset)
    {
        int month = Camera.main.GetComponent<GameTime>().GetMonth();

        if (customerAsset.CurrentOwner == this) return month > BidWindowEnd || month < BidWindowStart;
        return month >= BidWindowStart && month <= BidWindowEnd;
    }
}
