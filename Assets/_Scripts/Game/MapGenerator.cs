using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int NumPowerStations = StaticProperties.NumAIs + 1;

    private int Size = StaticProperties.MapSize;
    
    public GameObject CityPower;
    public GameObject PowerStation;
    public GameObject SolarField;
    public GameObject House;
    public GameObject Shop;
    public GameObject Building;
    public GameObject VerticalRoad;
    public GameObject HorizontalRoad;
    public GameObject IntersectionRoad;

    private float scale = 0f;
    private float offsetX = 0f;
    private float offsetY = 0f;

    void Start()
    {
        scale = Mathf.Sqrt(Size);
        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);

        GameObject map = new GameObject("Map");
        GenerateMap(map);
    }

    void GenerateMap(GameObject map)
    {   
        GameObject newObj;
        GameObject prefab;
        Quaternion rotation;
        Vector3 position;

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                prefab = GetAsset(x, y);
                rotation = prefab.Equals(VerticalRoad) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
                position = prefab.Equals(IntersectionRoad) ? new Vector3(7 * x, 0.01f, -7 * y) : new Vector3(7 * x, 0, -7 * y);
                
                newObj = Instantiate(prefab, position, rotation);
                newObj.transform.SetParent(map.transform, false);

                Asset asset = newObj.GetComponent<Asset>();

                if (asset != null)
                {
                    CityPower.GetComponent<AssetOwner>().Claim(asset);
                }
            }
        }
    }

    GameObject GetAsset(int x, int y)
    {
        if (x % 3 == 0 && y % 3 == 0)
        {
            return IntersectionRoad;
        } else if (x % 3 == 0)
        {
            return VerticalRoad;
        } else if (y % 3 == 0)
        {
            return HorizontalRoad;
        }

        float perlin = GetPerlin(x, y);

        if (perlin < 0.445f)
        {
            return House;
        } else if (perlin < 0.45f)
        {
            return SolarField;
        } else if (perlin < 0.65f)
        {
            return Shop;
        } else
        {
            return Building;
        }
    }

    float GetPerlin(int x, int y)
    {
        return Mathf.PerlinNoise((float) x / Size * scale + offsetX, (float) y / Size * scale + offsetY);
    }
}
