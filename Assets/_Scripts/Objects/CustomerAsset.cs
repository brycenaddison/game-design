using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
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
        get => Enumerable.Zip(
            offers.Keys,
            offers.Values,
            (float bid, AssetOwner bidder) =>
            {
                return $"{bidder.Name}: {bid.ToString("C", CultureInfo.CurrentCulture)}";
            }
          );
    }

    private SortedList<float, AssetOwner> offers;

    private void Start()
    {
        _payment = InitialPayment;
        offers = new SortedList<float, AssetOwner>
        {
            { Payment, Owner }
        };
        Camera.main.GetComponent<GameTime>().RegisterOnMonth(11, () => { AcceptBestOffer(); }, 0);
    }

    public void Offer(AssetOwner owner, float payment)
    {
        RemoveOffer(owner);

        offers.Add(payment, owner);
    }

    public void RemoveOffer(AssetOwner owner)
    {
        int ownerIndex = offers.IndexOfValue(owner);

        if (ownerIndex != -1)
        {
            offers.RemoveAt(ownerIndex);
        }
    }

    public void AcceptBestOffer()
    {
        if (offers.Count > 0)
        {
            float newPayment = offers.Keys[0];
            if (newPayment >= MaxPayment) { Owner?.Unclaim(this); }
            _payment = offers.Keys[0];

            AssetOwner bestOffer = offers.Values[0];
            bestOffer.Claim(this);
        }
        else { Owner?.Unclaim(this); }
    }

    public bool IsBiddedOnBy(AssetOwner bidder)
    {
        return offers.ContainsValue(bidder);
    }
}
