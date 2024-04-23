using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public RowUI rowUI;

    private List<AssetOwner> ownerList = new List<AssetOwner>();
    private List<RowUI> rows = new List<RowUI>();

    void Update()
    {
        ownerList.Sort((a, b) => b.balance.CompareTo(a.balance));
        for (int i = 0; i < ownerList.Count; i++)
        {
            RowUI row = rows[i];
            AssetOwner owner = ownerList[i];
            row.rank.text = $"{i+1}";
            row.displayName.text = owner.ownerName;
            row.balance.text = $"{owner.balance.ToString("C", CultureInfo.CurrentCulture)} ({owner.Profit.ToString("C", CultureInfo.CurrentCulture)})";
        }
    }

    public void Register(AssetOwner assetOwner)
    {
        ownerList.Add(assetOwner);
        RowUI newRow = Instantiate(rowUI, transform);
        newRow.gameObject.SetActive(true);
        rows.Add(newRow);
    }
}
