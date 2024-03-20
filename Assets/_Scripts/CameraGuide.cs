using UnityEngine;

public class CameraGuide : MonoBehaviour
{
    public enum MouseControlType
    {
        Drag,
        Off
    }

    [Header("Inscribed")]
    public MouseControlType mouseControl = MouseControlType.Drag;
    public float speed = 5.0f;
    public float xMin = -8.5f;
    public float xMax = 8.5f;
    public float zMin = -4.5f;
    public float zMax = 4.5f;
    public Plane plane = new Plane(Vector3.down, Vector3.zero);

    [Header("Dynamic")]
    private Vector3 dragOrigin;
    private bool cameraDragging = false;

    private Vector3 BoundPosition(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = 0;
        pos.z = Mathf.Clamp(pos.z, zMin, zMax);
        return pos;
    }

    private Vector3 GetPointAtScreenPosition()
    {
        return GetPointAtScreenPosition(Input.mousePosition);
    }

    private Vector3 GetPointAtScreenPosition(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return new Vector3();
    }


    void Update()
    {
        if (mouseControl == MouseControlType.Drag)
        {

            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = GetPointAtScreenPosition();
                cameraDragging = true;

                return;
            }

            if (!Input.GetMouseButton(0))
            {
                cameraDragging = false;
            }

            if (cameraDragging)
            {
                Vector3 diff = GetPointAtScreenPosition() - dragOrigin;

                transform.position = BoundPosition(transform.position - diff);

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

        transform.position = BoundPosition(pos);
    }
}
