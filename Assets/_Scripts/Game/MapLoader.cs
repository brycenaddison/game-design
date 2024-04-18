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
    
    private Dictionary<char, GameObject> map;

    void Awake() {
        map = new Dictionary<char, GameObject>(){
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
        LoadMap(System.IO.File.ReadAllLines("Assets/Maps/map1.txt"));
    }

    void InstantiateRoad(GameObject prefab, Vector3 position, float yRotation) {
        Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
        Instantiate(prefab, position, rotation);
    }

    void LoadMap(string[] lines)
    {
        int width = lines[0].Length;
        int height = lines.Length;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                char c = lines[y][x];
                GameObject prefab = map[c];
                Quaternion rotation = (c == '7') ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
                Vector3 position = (c == '9') ? new Vector3(6 * x, 0.01f, -6 * y) : new Vector3(6 * x, 0, -6 * y);
                Instantiate(prefab, position, rotation);
            }
        }
    }
}
