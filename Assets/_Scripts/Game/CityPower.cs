using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPower : MonoBehaviour
{
    public AssetOwner cityPower;
    public GameObject mapGenerator;

    private IAssetMap assetMap;

    public AssetOwner Get()
    {
        return cityPower;
    }

    public IAssetMap Map()
    {
        if (assetMap == null)
        {
            MapGenerator generator = this.mapGenerator.GetComponent<MapGenerator>();
            if (generator == null)
            {
                MapLoader loader = this.mapGenerator.GetComponent<MapLoader>();
                assetMap = loader;
            }
            else
            {
                assetMap = generator;
            }
        }

        return assetMap;
    }
}
