/**
 * Global variables needed for several game functions, such as creating the map,
 * initializing AIs, and displaying colors. Gets these values from the Settings scene.
 *
 * Author: Jack, Brycen
 * Date: 4 / 23 / 24
*/

using System;
using System.Collections;
using UnityEngine;

public class StaticProperties
{
    private static string _name = "Unnamed Player";
    private static Color _color = new Color(0, 0, 0);
    private static int _mapSize = 12;
    private static int _numAIs = 3;
    private static float _strengthOfAI = 0.8f;

    public static string Name
    {
        get => _name;
        set => _name = value;
    }

    public static Color Color
    {
        get => _color;
        set => _color = value;
    }

    public static int MapSize
    {
        get => _mapSize;
        set => _mapSize = value;
    }

    public static int NumAIs
    {
        get => _numAIs;
        set => _numAIs = value;
    }

    public static float StrengthOfAI
    {
        get => _strengthOfAI;
        set => _strengthOfAI = value;
    }
}