using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private List<AssetOwner> owners = new List<AssetOwner>();
    private Dictionary<Asset, List<Asset>> adjList;
    private int month;
    private float strength;
    private MapGenerator mg = new MapGenerator();

    void Start()
    {
        strength = StaticProperties.StrengthOfAI;
    }

    void Update()
    {  
        GameTime gt = new GameTime();
        month = gt.GetMonth();
        owners = mg.GetAIOwners();

        foreach (AssetOwner owner in owners)
        {
            if (month == 1)
            {
                owner.MaxRates();
            } else if (month < 7)
            {
                owner.Undercut(mg);
            } else
            {
                owner.Defend();
            }
        }
    }
}
