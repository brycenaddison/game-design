using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private readonly List<AssetOwner> owners = new List<AssetOwner>();
    private int month;
    private float strength;

    void Start()
    {
        strength = StaticProperties.StrengthOfAI;

        GameTime gt = gameObject.AddComponent<GameTime>();

        foreach (AssetOwner owner in owners)
        {
            gt.RegisterOnMonth(1, () => owner.MaxRates(), -500);
            gt.RegisterOnMonth(8, () => owner.Undercut(), -500);
            gt.RegisterOnMonth(11, () => owner.Defend(), -500);
        }
    }
}
