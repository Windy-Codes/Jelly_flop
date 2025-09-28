using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10f;
    public float jump = 10f;
    public CapsuleCollider2D capsule;

    [Header("Layer Settings")]
    public LayerMask excludeLayers; // Layers the player should NOT stand on

    private float HorizontalInput;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Variable Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Gravity Settings")]
    public float baseGravity = 4.5f;   // upward / default gravity
    public float fallGravity = 6f;     // gravity when falling
    public float maxFallSpeed = -20f;  // terminal velocity like Hollow Knight

    void Update()
    {
        // --- INPUTS ---
        HorizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump Pressed
        if (Input.GetButtonDown("Jump")&&(isGrounded () && coyoteTimeCounter > 0f))
        {
            _Jump();
        }

        // Apply gravity scale depending on state
        if (rb.velocity.y < 0) // falling
        {
            rb.gravityScale = fallGravity;
        }
        else // going up / idle
        {
            rb.gravityScale = baseGravity;
        }

        // Clamp fall speed (terminal velocity)
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }

        // Variable Jump Height
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Sprite flip
        if (HorizontalInput > 0.01f)
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        else if (HorizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);

        // Update coyote time
        if (isGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        // --- MOVEMENT ---
        rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);
    }

    private void _Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
        coyoteTimeCounter = 0f;
    }

    private bool isGrounded()
    {
        // shrink the box a bit so it doesn't hit the player's own collider
        Vector2 boxSize = new Vector2(capsule.bounds.size.x * 0.9f, 0.1f);

        // position just below the collider
        Vector2 boxCenter = (Vector2)capsule.bounds.center + Vector2.down * (capsule.bounds.extents.y + 0.05f);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCenter,
            boxSize,
            0f,
            Vector2.down,
            0f,
            Physics2D.AllLayers
        );

        return hit.collider != null && !hit.collider.isTrigger;
    }
}
