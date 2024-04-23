using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    void Start()
    {
        GameTime gt = Camera.main.GetComponent<GameTime>();

        AssetOwner owner = GetComponent<AssetOwner>();

        gt.RegisterOnMonth(1, () => owner.MaxRates(), -500);
        gt.RegisterOnMonth(8, () => owner.Undercut(), -500);
        gt.RegisterOnMonth(11, () => owner.Defend(), -500);
    }
}
