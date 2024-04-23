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
    }

    void Update()
    {  
        GameTime gt = gameObject.AddComponent<GameTime>();
        month = gt.GetMonth();

        foreach (AssetOwner owner in owners)
        {
            if (month == 1)
            {
                owner.MaxRates();
            } else if (month < 11)
            {
                owner.Undercut();
            } else
            {
                owner.Defend();
            }
        }
    }
}
