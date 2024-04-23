using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAsset : Asset, IPurchasable
{
    public float Upkeep;
    public float PowerGenerated;
    [SerializeField]
    private float _Cost = 10000;
    public float Cost { get { return _Cost; } }

    public bool IsBuyable(AssetOwner player)
    {
        if (Owner != Camera.main.GetComponent<CityPower>().Get())
        {
            return false;
        }

        MapGenerator loader = Camera.main.GetComponent<CityPower>().Map();

        foreach (Asset asset in loader.GetAdjacentAssets(this))
        {
            if (asset.Owner == player)
            {
                return true;
            }
        }

        return false;
    }

    public void Buy(AssetOwner player)
    {
        player.Purchase(this);
        player.Claim(this);
    }
}
