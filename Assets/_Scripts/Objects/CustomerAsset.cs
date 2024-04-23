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
                return $"{bidder.ownerName}: {bid.ToString("C", CultureInfo.CurrentCulture)}";
            }
          );
    }

    private SortedList<float, AssetOwner> offers;
    private GameTime gameTime;

    private void Start()
    {
        _payment = InitialPayment;
        offers = new SortedList<float, AssetOwner>
        {
            { Payment, CurrentOwner }
        };
        gameTime = Camera.main.GetComponent<GameTime>();
        gameTime.RegisterOnMonth(1, () => { AcceptBestOffer(); }, 5000);
    }

    public void Offer(AssetOwner owner, float payment)
    {
        if (!owner.CanBidOn(this)) return;

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
            if (newPayment >= MaxPayment) { CurrentOwner?.Unclaim(this); }
            _payment = offers.Keys[0];

            AssetOwner bestOffer = offers.Values[0];
            bestOffer.Claim(this);
        }
        else { CurrentOwner?.Unclaim(this); }
    }

    public bool IsBiddedOnBy(AssetOwner bidder)
    {
        return offers.ContainsValue(bidder);
    }
}
