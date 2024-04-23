using System.Collections.Generic;
using UnityEngine;

public interface IAssetMap
{
    List<Asset> GetAdjacentAssets(Asset asset);
}
