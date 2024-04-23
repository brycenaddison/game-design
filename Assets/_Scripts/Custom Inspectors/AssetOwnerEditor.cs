/**
 * Displays relevant info about companies to the GUI
 *
 * Author: Brycen
 * Date: 4 / 23 / 24
*/

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AssetOwner))]
public class AssetOwnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AssetOwner assetOwner = (AssetOwner)target;

        EditorGUILayout.LabelField("Profit: " + assetOwner.Profit);
        EditorGUILayout.LabelField("Revenue (from customers): " + assetOwner.Revenue);
        EditorGUILayout.LabelField("Expenses (from power sources): " + assetOwner.Expenses);

        EditorGUILayout.LabelField("Free Power: " + assetOwner.FreePower);
        EditorGUILayout.LabelField("Consumption (from customers): " + assetOwner.PowerUsed);
        EditorGUILayout.LabelField("Generation (from power sources): " + assetOwner.PowerTotal);
    }
}