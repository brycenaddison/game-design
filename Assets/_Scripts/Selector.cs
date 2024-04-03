using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public Color highlightColor;
    public Color selectionColor;

    public GameObject Highlighted
    {
        get { return _highlighted; }
        private set
        {
            _highlighted = value;
        }
    }

    public GameObject Selected
    {
        get { return _selected; }
        private set
        {
            _selected = value;
        }
    }

    [SerializeField]
    private GameObject _highlighted;

    [SerializeField]
    private GameObject _selected;

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
            Highlighted = null;
        }
    }
}