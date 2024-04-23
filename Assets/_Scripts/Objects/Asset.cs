/**
 * Assets are the basic unit of the game. Primarily, they have an owner.
 * Several prefabs are given the Asset component, where they specify power draw
 * and revenue.
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset : MonoBehaviour
{
    public AssetOwner Owner { get; set; }
    public string assetName;
}
