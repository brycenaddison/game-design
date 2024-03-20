using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraGuide : MonoBehaviour
{
    [Header("Inscribed")]
    public bool enableMouseControl = true;
    public float speed = 5.0f;
    public float xMin = -8.5f;
    public float xMax = 8.5f;
    public float zMin = -4.5f;
    public float zMax = 4.5f;
    Plane plane = new Plane(Vector3.down, Vector3.zero);

    [Header("Dynamic")]
    private Vector2 dragOrigin;
    private bool cameraDragging = false;
    private float cameraDistance;

    private Vector3 boundPosition(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = 0;
        pos.z = Mathf.Clamp(pos.z, zMin, zMax);
        return pos;
    }

    private void Start()
    {
        cameraDistance = Camera.main.WorldToScreenPoint(transform.position).z;
    }
    // Update is called once per frame
    void Update()
    {
        if (enableMouseControl)
        {

            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                cameraDragging = true;

                Debug.Log("DragOrigin: " + dragOrigin);

                return;
            }

            if (!Input.GetMouseButton(0))
            {
                cameraDragging = false;
            }

            if (cameraDragging)
            {
                Vector2 diff = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - dragOrigin;

                Ray ray = Camera.main.ScreenPointToRay(diff);

                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 move = ray.GetPoint(distance);
                    Debug.Log("hitPoint: " + move);
                }

// Vector3 move = Camera.main.ScreenToWorldPoint(new Vector3(diff.x, diff.y, cameraDistance));

// Debug.Log("move: " + move);

              // transform.position = boundPosition(transform.position - move);

                return;
            }
        }

        // Pull in information from the Input class
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        // Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.z += vAxis * speed * Time.deltaTime;

        transform.position = boundPosition(pos);
    }
}
