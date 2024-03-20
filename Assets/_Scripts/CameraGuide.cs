using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraGuide : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 5.0f;
    public float xMin = -8.5f;
    public float xMax = 8.5f;
    public float zMin = -4.5f;
    public float zMax = 4.5f;

    private Vector3 boundPosition(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.z = Mathf.Clamp(pos.z, zMin, zMax);
        return pos;
    }

    // Update is called once per frame
    void Update()
    {
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
