using UnityEngine;

public class GuideFollower : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject target;
    public float distance = 10.0f;
    public float angle = 45.0f;

    public float smoothTime = 0.05f;

    [Header("Dynamic")]
    public Vector3 currentVelocity;
    void LateUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector3 offset = Quaternion.Euler(angle, 0, 0) * Vector3.back * distance;
        Vector3 newPosition = new Vector3(target.transform.position.x, 0, target.transform.position.z) + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            newPosition,
            ref currentVelocity,
            smoothTime
        );
    }
}
