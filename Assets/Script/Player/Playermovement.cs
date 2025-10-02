using System.Collections;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10f;
    public float jump = 10f;
    public CapsuleCollider2D capsule;
    private SpriteRenderer sr;

    private float HorizontalInput;
    private int lastDirection = 0; // -1 for left, 1 for right

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Variable Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Gravity Settings")]
    public float baseGravity = 4.5f;
    public float fallGravity = 6f;
    public float maxFallSpeed = -20f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing;
    private bool dashAvailable = true; // dash recharges only on ground
    private float dashCooldownTimer;

    [Header("Dash Afterimage")]
    public GameObject afterimagePrefab;
    public float afterimageSpacing = 0.05f; // time between afterimages
    private float afterimageTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDashing)
        {
            HandleAfterimages();
            return; // don’t run normal movement while dashing
        }

        // --- INPUTS (Last Key Priority) ---
        if (Input.GetKeyDown(KeyCode.A)) lastDirection = -1;
        if (Input.GetKeyDown(KeyCode.D)) lastDirection = 1;

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            HorizontalInput = lastDirection;
        else if (Input.GetKey(KeyCode.A))
            HorizontalInput = -1;
        else if (Input.GetKey(KeyCode.D))
            HorizontalInput = 1;
        else
            HorizontalInput = 0;

        // Jump Pressed
        if (Input.GetButtonDown("Jump") && (isGrounded() || coyoteTimeCounter > 0f))
        {
            _Jump();
        }

        // Dash input (works in air too, but only if dashAvailable is true)
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashAvailable)
        {
            StartCoroutine(Dash());
        }

        // Gravity control
        if (rb.velocity.y < 0)
            rb.gravityScale = fallGravity;
        else
            rb.gravityScale = baseGravity;

        // Clamp fall speed
        if (rb.velocity.y < maxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);

        // Variable Jump Height
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

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

        // Recharge dash only when grounded
        if (isGrounded() && !dashAvailable && dashCooldownTimer <= 0f)
        {
            dashAvailable = true;
            sr.color = Color.white; // dash ready
        }

        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (isDashing) return; // don’t override dash velocity

        rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);
    }

    private void _Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
        coyoteTimeCounter = 0f;
    }

    private bool isGrounded()
    {
        Vector2 boxSize = new Vector2(capsule.bounds.size.x * 0.9f, 0.1f);
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

    private IEnumerator Dash()
    {
        isDashing = true;
        dashAvailable = false; // disable until grounded + cooldown
        dashCooldownTimer = dashCooldown;

        sr.color = new Color(0.1216f, 1f, 0f); // Green while dash is recharging
        afterimageTimer = 0f; // reset afterimage timer

        // get dash direction (input or facing direction)
        Vector2 dashDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (dashDir == Vector2.zero)
            dashDir = new Vector2(transform.localScale.x, 0); // default: facing direction

        dashDir.Normalize();

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashTimer = 0f;

        // LOCK velocity during dash (no gravity, no drift)
        while (dashTimer < dashTime)
        {
            rb.velocity = dashDir * dashSpeed;
            dashTimer += Time.deltaTime;

            HandleAfterimages();

            yield return null;
        }

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void HandleAfterimages()
    {
        afterimageTimer -= Time.deltaTime;
        if (afterimageTimer <= 0f)
        {
            SpawnAfterimage();
            afterimageTimer = afterimageSpacing;
        }
    }

    private void SpawnAfterimage()
    {
        GameObject ai = Instantiate(afterimagePrefab, transform.position, transform.rotation);
        SpriteRenderer aiSr = ai.GetComponent<SpriteRenderer>();
        aiSr.sprite = sr.sprite; // copy current sprite
        aiSr.flipX = sr.flipX;   // copy facing direction
        aiSr.color = sr.color;   // match dash color
    }
}
