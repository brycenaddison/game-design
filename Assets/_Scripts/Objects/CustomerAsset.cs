/**
 * Handles the bidding logic. Customers always accept the lowest offer once bidding is closed.
 * If the lowest offer is from a different company, they will switch to that new company.
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class CustomerAsset : Asset
{
    public float Draw;
    public float InitialPayment;
    public float MaxPayment;

    private float _payment;
    public float Payment { get => _payment; }
    public IEnumerable<string> Offers
    {
        get => GetSortedOffers().Select(
            offer => $"{offer.Key.Name}: {offer.Value.ToString("C", CultureInfo.CurrentCulture)}");
    }

    private Dictionary<AssetOwner, float> offers;
    private GameTime gameTime;
    private CityPower _cityPower;

    private void Start()
    {
        _payment = InitialPayment;
        offers = new Dictionary<AssetOwner, float>
        {
            { Owner, Payment }
        };
        gameTime = Camera.main.GetComponent<GameTime>();
        gameTime.RegisterOnMonth(1, () => { AcceptBestOffer(); }, 5000);
    }


    private CityPower GetCityPower()
    {
        if (_cityPower == null)
        {
            _cityPower = Camera.main.GetComponent<CityPower>();
        }
        return _cityPower;
    }

    public void Offer(AssetOwner owner, float payment)
    {
        Debug.Log("offer made!");

        if (!owner.CanBidOn(this)) return;

        RemoveOffer(owner);

        offers.Add(owner, payment);
    }

    public void RemoveOffer(AssetOwner owner)
    {
        offers?.Remove(owner);
    }

    private void FilterInvalidOffers()
    {
        if (offers == null || offers.Count == 0) return;

        List<KeyValuePair<AssetOwner, float>> invalidOffers = new List<KeyValuePair<AssetOwner, float>>(offers.Where(
            pair => !HasAdjacentOwner(pair.Key) || pair.Value > MaxPayment
        ));

        foreach (KeyValuePair<AssetOwner, float> offer in invalidOffers)
        {
            offers.Remove(offer.Key);
        }
    }

    private List<KeyValuePair<AssetOwner, float>> GetSortedOffers()
    {
        if (offers == null || offers.Count == 0) return new List<KeyValuePair<AssetOwner, float>>();

        List<KeyValuePair<AssetOwner, float>> sortedOffers = new List<KeyValuePair<AssetOwner, float>>(offers);

        sortedOffers.Sort((a, b) => a.Value.CompareTo(b.Value));

        return sortedOffers;
    }

    public KeyValuePair<AssetOwner, float> BestOffer()
    {
        FilterInvalidOffers();

        List<KeyValuePair<AssetOwner, float>> validOffers = GetSortedOffers();

        KeyValuePair<AssetOwner, float> bestOffer = validOffers[0];

        return bestOffer;
    }

    public void AcceptBestOffer()
    {
        FilterInvalidOffers();

        List<KeyValuePair<AssetOwner, float>> validOffers = GetSortedOffers();

        if (validOffers.Count > 0)
        {
            KeyValuePair<AssetOwner, float> bestOffer = validOffers[0];

            _payment = BestOffer().Value;

            bestOffer.Key.Claim(this);
        }
        else
        {
            if (Owner != GetCityPower().Get())
            {
                Owner.Unclaim(this);
            }
        }
    }

    public bool IsBiddedOnBy(AssetOwner bidder)
    {
        return offers.ContainsKey(bidder);
    }

    public bool HasAdjacentOwner(AssetOwner owner)
    {
        IAssetMap map = GetCityPower().Map();

        // pathfinding to ensure connected to owned power source
        List<Asset> visited = new List<Asset>();
        Queue<Asset> queue = new Queue<Asset>();

        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            Asset visitingAsset = queue.Dequeue();
            visited.Add(visitingAsset);

            foreach (Asset asset in map.GetAdjacentAssets(visitingAsset))
            {
                if (visited.Contains(asset) || asset.Owner != owner) continue;

                if (asset is PowerAsset) return true;

                queue.Enqueue(asset);
            }
        }

        return false;
    }
}
