/**
 * Scoreboard displaying current competing companies, their balance, and their profit.
 * If a company is bought out or goes bankrupt, then they are removed from the leaderboard.
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public RowUI rowUI;

    private List<AssetOwner> ownerList = new List<AssetOwner>();
    private List<RowUI> rows = new List<RowUI>();

    void Update()
    {
        CheckWon();

        ownerList.Sort((a, b) => b.balance.CompareTo(a.balance));
        for (int i = 0; i < ownerList.Count; i++)
        {
            RowUI row = rows[i];
            AssetOwner owner = ownerList[i];
            row.rank.text = $"{i+1}";
            row.displayName.text = owner.Name;
            row.balance.text = owner.balance.ToString("C", CultureInfo.CurrentCulture);
            row.income.text = owner.Profit.ToString("C", CultureInfo.CurrentCulture);
        }
    }

    private void CheckWon()
    {
        List<AssetOwner> competitors = new List<AssetOwner>(ownerList.Where((owner) => owner.balance > 0));

        if (competitors.Count == 1 && competitors[0] == Camera.main.GetComponent<AssetOwner>())
        {
            Camera.main.GetComponent<GameTime>().TriggerWin();
        }
    }

    public void Register(AssetOwner assetOwner)
    {
        ownerList.Add(assetOwner);
        RowUI newRow = Instantiate(rowUI, transform);
        newRow.gameObject.SetActive(true);
        rows.Add(newRow);
    }

    public void Unregister(AssetOwner assetOwner)
    {
        int index = ownerList.IndexOf(assetOwner);
        ownerList.RemoveAt(index);
        if (rows[index] != null)
        {
            Destroy(rows[index].gameObject);
        }
        rows.RemoveAt(index);
    }
}
