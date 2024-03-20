using UnityEngine;

public class GuideFollower : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject target;
    public float initialDistance = 10.0f;
    public float angle = 45.0f;
    public float minDistance = 5.0f;
    public float maxDistance = 20.0f;
    public float zoomSpeed = 10.0f;
    public float zoomSensitivity = 10.0f;

    // public float smoothTime = 0.05f;

    [Header("Dynamic")]
    public float distance;
    public float zoomLevel;

    void Start()
    {
        zoomLevel = initialDistance;
    }

    void Update()
    {
        zoomLevel -= Input.mouseScrollDelta.y * zoomSensitivity;
        zoomLevel = Mathf.Clamp(zoomLevel, minDistance, maxDistance);

        distance = Mathf.MoveTowards(distance, zoomLevel, zoomSpeed * Time.deltaTime);
    }

    void LateUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector3 offset = Quaternion.Euler(angle, 0, 0) * Vector3.back * distance;
        Vector3 newPosition = new Vector3(target.transform.position.x, 0, target.transform.position.z) + offset;

        // TODO: revisit interpolation
        //
        // transform.position = Vector3.SmoothDamp(
        //     transform.position,
        //     newPosition,
        //     ref currentVelocity,
        //     smoothTime
        // );

        transform.position = newPosition;
    }
}
