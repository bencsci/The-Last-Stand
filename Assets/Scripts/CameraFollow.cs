using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    // Reference to the target Transform
    [SerializeField]
    public static Transform target;

    // Smoothing time for camera movement
    public float smoothTime = 0.1f;

    // Offset from the target position
    public Vector3 offset;

    // Velocity used for smooth dampening
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Make Camera follow the player with given offset
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
