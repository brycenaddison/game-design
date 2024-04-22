using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HelperForAI : MonoBehaviour
{
    public static HelperForAI Instance { get; private set; }
    public List<string> names;
    public int id = 2;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        names = new List<string>(File.ReadAllLines("Assets/Names.txt"));
        Debug.Log(names);
    }

    public string GetRandName() {
        if (names == null || names.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, names.Count);
        string name = names[index];
        names.RemoveAt(index);
        return name;
    }

    public int GetNextID() {
        return id++;
    }
}
