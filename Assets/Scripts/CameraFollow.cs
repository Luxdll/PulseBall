using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Collider2D leftWall;
    [SerializeField] private Collider2D rightWall;
    [SerializeField, Min(0.01f)] private float smoothTime = 0.15f;

    private Camera gameCamera;
    private float velocityX;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (target == null || leftWall == null || rightWall == null)
        {
            Debug.LogError("Назначь игрока и границы для CameraFollow.", this);
            enabled = false;
            return;
        }

        GetLimits(out float minX, out float maxX);

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(target.position.x, minX, maxX);
        transform.position = position;
    }

    private void LateUpdate()
    {
        GetLimits(out float minX, out float maxX);

        float targetX = Mathf.Clamp(target.position.x, minX, maxX);

        Vector3 position = transform.position;
        position.x = Mathf.SmoothDamp(
            position.x, targetX, ref velocityX, smoothTime);

        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
    }

    private void GetLimits(out float minX, out float maxX)
    {
        float halfWidth = gameCamera.orthographicSize * gameCamera.aspect;

        minX = leftWall.bounds.min.x + halfWidth;
        maxX = rightWall.bounds.max.x - halfWidth;

        if (minX > maxX)
        {
            float center = (leftWall.bounds.min.x
                + rightWall.bounds.max.x) * 0.5f;

            minX = center;
            maxX = center;
        }
    }
}