using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPower : MonoBehaviour
{
    public AssetOwner cityPower;
    public MapGenerator mapGenerator;

    public AssetOwner Get()
    {
        return cityPower;
    }

    public MapGenerator Map()
    {
        return mapGenerator;
    }
}
