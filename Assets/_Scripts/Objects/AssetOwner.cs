/**
 * AssetOwners are any GameObject that can own assets: The player, AIs, and City Power.
 * Large gameplay functionality implemented here, such as winning and losing, bidding
 * and claiming, and profit.
 *
 * Author: Brycen, Jack
 * Date: 4 / 23 / 24
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetOwner : MonoBehaviour
{
    public List<Asset> assets;

    [SerializeField]
    private string _name;
    [SerializeField]
    private int _id;
    [SerializeField]
    private bool _isPlayable;

    public string Name { get => _name; set => _name = value; }
    public int Id { get => _id; set => _id = value; }
    public Color Color { get; set; }
    public bool IsPlayable { get => _isPlayable; set => _isPlayable = value; }
    public GameObject HQ { get; set; }

    public float initialBalance = 1000;
    public Scoreboard scoreboard;

    public static int BidWindowStart = 7;
    public static int BidWindowEnd = 9;

    [Header("Read Only")]
    public float balance;

    private Action cleanup;
    private CityPower cityPower;

    void Start()
    {
        foreach (Asset asset in assets)
        {
            Claim(asset);
        }

        scoreboard?.Register(this);

        GameTime gameTime = Camera.main.GetComponent<GameTime>();
        cityPower = Camera.main.GetComponent<CityPower>();

        if (IsPlayable)
        {
            Name = StaticProperties.Name;
            Color = StaticProperties.Color;
            Id = 0;
        } else if (cityPower.Get() == this)
        {
            Name = "City Power";
            Color = Color.grey;
            Id = 1;
        } else
        {
            List<string> Names = new List<string>(File.ReadAllLines("Assets/Names.txt"));
            Name = Names[UnityEngine.Random.Range(0, Names.Count)];

            int red = UnityEngine.Random.Range(0, 255);
            int green = UnityEngine.Random.Range(0, 255);
            int blue = UnityEngine.Random.Range(0, 255);
            Color = new Color(red / 255f, green / 255f, blue / 255f);

            Id = UnityEngine.Random.Range(2, 100000000); // crashes if this happens to be the same as another asset owner lmao 🙏
        }

        balance = initialBalance;

        cleanup = gameTime.RegisterOnMonth(1, () =>
        {
            balance += Profit;
            if (balance < 0)
            {
                if (IsPlayable)
                {
                    gameTime.TriggerLose();
                }
                else
                {
                    DeclareBankruptcy();
                }
            }
        }, Id);
    }

    private void OnDestroy()
    {
        scoreboard?.Unregister(this);
        if (cleanup != null) cleanup();
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
            }
            else
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
        if (asset == null) return;

        asset.Owner = cityPower.Get();
        assets.Remove(asset);

        if (asset is CustomerAsset customerAsset)
        {
            customerAsset.RemoveOffer(this);
            customerAsset.AcceptBestOffer();
        }
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

        balance += other.balance - 5000;
        other.balance = 0;
    }

    public void DeclareBankruptcy()
    {
        foreach (Asset asset in new List<Asset>(assets))
        {
            cityPower.Get().Claim(asset);
        }

        balance = 0;
    }

    public bool CanBidOn(CustomerAsset customerAsset)
    {
        if (!customerAsset.HasAdjacentOwner(this)) return false;

        int month = Camera.main.GetComponent<GameTime>().GetMonth();

        if (customerAsset.Owner == this) return month > BidWindowEnd || month < BidWindowStart;
        return month >= BidWindowStart && month <= BidWindowEnd;
    }
}
