using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject prefab0;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    //public GameObject prefab4;
    //public GameObject prefab5;
    //public GameObject prefab6;
    public GameObject prefab7;
    public GameObject prefab8;
    public GameObject prefab9;
    
    public AssetOwner[] owners;

    private Dictionary<char, GameObject> mapDict;

    void Awake() {
        mapDict = new Dictionary<char, GameObject>(){
            {'0', prefab0},
            {'1', prefab1},
            {'2', prefab2},
            {'3', prefab3},
            //{'4', prefab4},
            //{'5', prefab5},
            //{'6', prefab6},
            {'7', prefab7},
            {'8', prefab8},
            {'9', prefab9}
        };
    }

    void Start()
    {
        GameObject map = new GameObject("Map");
        LoadMap(System.IO.File.ReadAllLines("Assets/Maps/map1.txt"), map);
    }

    void LoadMap(string[] lines, GameObject map)
    {
        int width = lines[0].Length;
        int height = lines.Length;
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                char c = lines[y][x];
                GameObject prefab = mapDict[c];
                Quaternion rotation = (c == '7') ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
                Vector3 position = (c == '9') ? new Vector3(-40 + 7 * x, 0.01f, 60 + -7 * y) : new Vector3(-40 + 7 * x, 0, 60 + -7 * y);
                GameObject newObj = Instantiate(prefab, position, rotation);
                newObj.transform.SetParent(map.transform, false);

                Asset asset = newObj.GetComponent<Asset>();

                if (asset != null) {
                    int ownerIndex = getOwnerIndex(x < halfWidth, y < halfHeight);
                    owners[ownerIndex].Claim(asset);
                }
            }
        }
    }

    int getOwnerIndex(bool isLeft, bool isTop) {
        if (isLeft && isTop) return 0;
        if (!isLeft && isTop) return 1;
        if (!isLeft && !isTop) return 2;
        if (isLeft && !isTop) return 3;     // player (camera)
        return -1;
    }
}
