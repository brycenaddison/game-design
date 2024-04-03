using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public Color highlightColor;
    public Color selectionColor;
    public Color baseColor;

    public GameObject Highlighted
    {
        get { return _highlighted; }
        private set
        {
            SetColor(_highlighted, baseColor);
            SetColor(value, highlightColor);
            _highlighted = value;
        }
    }

    public GameObject Selected
    {
        get { return _selected; }
        private set
        {
            if (value == Highlighted && value != null)
            {
                Highlighted = null;
            }
            SetColor(_selected, baseColor);
            SetColor(value, selectionColor);
            _selected = value;

        }
    }

    [SerializeField]
    private GameObject _highlighted;

    [SerializeField]
    private GameObject _selected;

    private void SetColor(GameObject go, Color color)
    {
        if (go == null) return;

        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.SetColor("_Color", color);
        }

    }

    void Update()
    {
        if (Highlighted != null)
        {
            Highlighted = null;
        }

        RaycastHit hit;
        GameObject hovered;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            hovered = hit.collider.gameObject;

            if (hovered.CompareTag("Selectable") && hovered != Selected)
            {
                Highlighted = hovered;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Selected = Highlighted;
        }
    }
}