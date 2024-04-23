using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

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

    public void Offer(AssetOwner owner, float payment)
    {
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

    public void AcceptBestOffer()
    {
        FilterInvalidOffers();

        List<KeyValuePair<AssetOwner, float>> validOffers = GetSortedOffers();

        if (validOffers.Count > 0)
        {
            KeyValuePair<AssetOwner, float> bestOffer = validOffers[0];

            _payment = bestOffer.Value;

            bestOffer.Key.Claim(this);
        }
        else {
            if (Owner != Camera.main.GetComponent<CityPower>().Get())
            {
                Owner.Unclaim(this);

                MapGenerator generator = Camera.main.GetComponent<CityPower>().Map();

                // propogate unclaim to possibly unclaim unconnected tiles
                foreach (Asset asset in generator.GetAdjacentAssets(this))
                {
                    if (asset is CustomerAsset customerAsset)
                    {
                        customerAsset.AcceptBestOffer();
                    }
                }
            }
        }
    }

    public bool IsBiddedOnBy(AssetOwner bidder)
    {
        return offers.ContainsKey(bidder);
    }

    public bool HasAdjacentOwner(AssetOwner owner)
    {
        return true;

        MapGenerator generator = Camera.main.GetComponent<CityPower>().Map();

        // pathfinding to ensure connected to owned power source
        List<Asset> visited = new List<Asset>();
        Queue<Asset> queue = new Queue<Asset>();

        queue.Enqueue(this);

        while (queue.Count > 0) {
            Asset visitingAsset = queue.Dequeue();
            visited.Add(visitingAsset);

            foreach (Asset asset in generator.GetAdjacentAssets(visitingAsset))
            {
                if (visited.Contains(asset) || asset.Owner != owner) continue;

                if (asset is PowerAsset) return true;

                queue.Enqueue(asset);
            }
        }

        return false;
    }
}
