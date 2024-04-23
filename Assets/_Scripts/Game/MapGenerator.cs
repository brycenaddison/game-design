/**
 * Generates a random map using Perlin Noise. Populates the map with companies, and
 * immediately gives those companies some starting assets.
 *
 * Author: Jack, Brycen
 * Date: 4 / 23 / 24
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject Player;
    public GameObject CityPower;
    public GameObject AI;
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
    private List<AssetOwner> Owners = new List<AssetOwner>();
    private Dictionary<Asset, List<Asset>> adjList;

    private readonly int NumPowerStations = StaticProperties.NumAIs + 1;
    private readonly int Size = StaticProperties.MapSize;

    void Start()
    {
        adjList = new Dictionary<Asset, List<Asset>>();

        scale = Mathf.Sqrt(Size);
        offsetX = UnityEngine.Random.Range(0f, 99999f);
        offsetY = UnityEngine.Random.Range(0f, 99999f);

        Owners.Add(Player.GetComponent<AssetOwner>());

        GameObject map = new GameObject("Map");
        GenerateMap(map);

        GameObject AIs = new GameObject("AIs");
        GenerateAIs(AIs);

        PopulateAIs(map);

        ManifestDestiny();
    }

    void GenerateMap(GameObject map)
    {
        GameObject newObj;
        GameObject prefab;
        Quaternion rotation;
        Vector3 position;

        List<List<Asset>> assetGrid = new List<List<Asset>>();

        for (int x = 0; x < Size; x++)
        {
            List<Asset> row = new List<Asset>();

            for (int z = 0; z < Size; z++)
            {
                prefab = GetAsset(x, z);
                rotation = prefab.Equals(VerticalRoad) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
                position = prefab.Equals(IntersectionRoad) ? new Vector3(7 * x, 0.01f, -7 * z) : new Vector3(7 * x, 0, -7 * z);

                newObj = Instantiate(prefab, position, rotation);
                newObj.transform.SetParent(map.transform, false);

                Asset asset = newObj.GetComponent<Asset>();

                if (asset != null)
                {
                    CityPower.GetComponent<AssetOwner>().Claim(asset);

                    List<Asset> adjacentAssets = new List<Asset>();

                    row.Add(asset);
                    adjList.Add(asset, adjacentAssets);
                }

                if (row.Count != 0)
                {
                    assetGrid.Add(row);
                }
            }
        }
    }

    public List<Asset> GetAdjacentAssets(Asset asset)
    {
        return adjList[asset];
    }

    GameObject GetAsset(int x, int y)
    {
        if (x % 3 == 0 && y % 3 == 0)
        {
            return IntersectionRoad;
        }
        else if (x % 3 == 0)
        {
            return VerticalRoad;
        }
        else if (y % 3 == 0)
        {
            return HorizontalRoad;
        }

        float perlin = GetPerlin(x, y);

        if (perlin < 0.445f)
        {
            return House;
        }
        else if (perlin < 0.45f)
        {
            return SolarField;
        }
        else if (perlin < 0.65f)
        {
            return Shop;
        }
        else
        {
            return Building;
        }
    }

    float GetPerlin(int x, int y)
    {
        return Mathf.PerlinNoise((float)x / Size * scale + offsetX, (float)y / Size * scale + offsetY);
    }

    void GenerateAIs(GameObject AIs)
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 position = new Vector3(0, 0, 0);
        GameObject newObj;

        for (int i = 0; i < NumPowerStations - 1; i++)
        {
            newObj = Instantiate(AI, position, rotation);
            newObj.transform.SetParent(AIs.transform, false);

            AssetOwner assetOwner = newObj.GetComponent<AssetOwner>();
            Owners.Add(assetOwner);
        }
    }

    void PopulateAIs(GameObject map)
    {
        GameObject prefab = PowerStation;
        Quaternion rotation = Quaternion.identity;
        Vector3 position;
        GameObject newObj;

        System.Random random = new System.Random();

        int randX;
        int randY;

        for (int i = 0; i < NumPowerStations; i++)
        {
            randX = random.Next(1, Size);
            randY = random.Next(1, Size);
            position = new Vector3(7 * randX, 0, -7 * randY);

            while (GetAsset(randX, randY) != House || !ValidPosition(position, i))
            {
                randX = random.Next(1, Size);
                randY = random.Next(1, Size);
                position = new Vector3(7 * randX, 0, -7 * randY);
            }

            DeleteObjectAtPosition(position, 1f);

            newObj = Instantiate(prefab, position, rotation);
            newObj.transform.SetParent(map.transform, false);

            Asset asset = newObj.GetComponent<Asset>();

            Owners[i].HQ = newObj;
            Owners[i].Claim(asset);
        }
    }

    private bool ValidPosition(Vector3 position, int OwnerIndex)
    {
        int borderDistance = 30;
        int betweenDistance = 65;

        if (position.x < borderDistance || position.x > Size * 7 - borderDistance) return false;
        if (position.z > -borderDistance || position.z < Size * -7 + borderDistance) return false;
        if (OwnerIndex == 0) return true;

        List<float> distances = new List<float>();
        distances = DistancesToOwners(distances, position, OwnerIndex);

        return distances.Min() > betweenDistance;
    }

    private List<float> DistancesToOwners(List<float> distances, Vector3 position, int OwnerIndex)
    {
        Vector3 otherPosition;

        for (int i = 0; i < OwnerIndex; i++)
        {
            otherPosition = Owners[i].HQ.transform.position;
            distances.Add(Vector3.Distance(position, otherPosition));
        }

        return distances;
    }

    void DeleteObjectAtPosition(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);

        foreach (Collider hit in hits)
        {
            Destroy(hit);
        }
    }

    void ManifestDestiny()
    {
        int steps = 10;
        int offsetX = steps / 2 * 7;
        int offsetZ = steps / 2 * -7;

        Vector3 position;

        foreach (AssetOwner owner in Owners)
        {
            position = owner.HQ.transform.position;

            position.x -= offsetX;
            position.z -= offsetZ;

            for (int i = 0; i <= steps; i++)
            {
                for (int j = 0; j <= steps; j++)
                {
                    ClaimObjectAtPosition(position, 1f, owner);
                    position.z -= 7;
                }

                position.x += 7;
                position.z += (steps + 1) * 7;
            }
        }
    }

    void ClaimObjectAtPosition(Vector3 position, float radius, AssetOwner owner)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);
        Asset asset;

        foreach (Collider hit in hits)
        {
            asset = hit.GetComponent<Asset>();

            if (asset != null) owner.Claim(asset);
        }
    }
}
