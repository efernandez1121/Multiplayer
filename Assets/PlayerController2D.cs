using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        // Only the owning client can control this player
        if (!IsOwner)
        {
            rb.simulated = false; // prevents non-owners from fighting physics locally
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        float move = Input.GetAxisRaw("Horizontal");

        // Move
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}