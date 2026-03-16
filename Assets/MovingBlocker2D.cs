using UnityEngine;

public class MovingBlocker2D : MonoBehaviour
{
    [Header("Positions")]
    public Transform closedPoint;
    public Transform openPoint;

    [Header("Movement")]
    public float moveSpeed = 3f;

    private bool isOpen;

    void Start()
    {
        if (closedPoint != null)
            transform.position = closedPoint.position;
    }

    void Update()
    {
        if (closedPoint == null || openPoint == null) return;

        Transform target = isOpen ? openPoint : closedPoint;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
    }
}