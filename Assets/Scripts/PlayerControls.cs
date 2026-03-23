using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    [Space(10)]
    [SerializeField] float acceleration;
    [SerializeField] float deacceleration;
    [SerializeField] float moveSpeed;
    [SerializeField] float velPower;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce = 30.0f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] bool isDashing = false;
    [SerializeField] bool grounded;
    [SerializeField] float frictionAmount = 0.2f;
    [SerializeField] bool facingRight = true;
    [SerializeField] bool isInAir = true;

    [Space(30)]
    [SerializeField] LayerMask groundLayerMask;
    [Space(10)]
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTime;
    public float coyoteTimer; public float jumpBufferTimer;
    public float jumpCutMultiplier = 0.5f;

    Rigidbody2D rb;
    inputManager input;
    void Start()
    {
        input = gameObject.GetComponent<inputManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        input.JumpPressed += HandleJumpPressed; // Subscribe to the JumpPressed event
        input.DashPressed += Dash;
    }

    void Update()
    {
        // Prevent jumping and state changes while dashing
        if (isDashing) return;

        if (grounded)
        {
            coyoteTimer = coyoteTime;
            isInAir = false;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        jumpHandler();
        Aircheck();
    }

    void OnDestroy()
    {
        input.JumpPressed -= HandleJumpPressed;
        input.DashPressed -= Dash;
    }

    void FixedUpdate()
    {
        // Prevent normal movement and flipping while dashing
        if (isDashing) return;

        movement();
        flipHandle();
        if (grounded && Mathf.Abs(input.horizontal) < 0.01f)
        {
            friction();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(Convert.ToString(groundLayerMask, 2).PadLeft(32, '0'));
        if ((groundLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Vector3 normalvect = other.GetContact(0).normal;
            if (normalvect == Vector3.up)
            {
                grounded = true;
            }
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        //Debug.Log(Convert.ToString(groundLayerMask,2).PadLeft(32,'0'));
        if ((groundLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            grounded = false;
        }
    }

    // This method is now called by the JumpPressed event
    void HandleJumpPressed()
    {
        jumpBufferTimer = jumpBufferTime; // Set jump buffer when jump is pressed

        // Coyote jump logic: if in air and within coyote time, perform jump immediately
        if (coyoteTimer > 0 && isInAir)
        {
            jump();
            jumpBufferTimer = 0; // Consume the buffer immediately if coyote jump happens
        }
    }

    void jumpHandler()
    {
        jumpBufferTimer -= Time.deltaTime; // Always decrement the buffer timer

        // Buffered jump logic: if buffer is active and player is grounded, perform jump
        if (jumpBufferTimer > 0 && !isInAir)
        {
            jump();
            jumpBufferTimer = 0; // Consume the buffer
        }
        jumpCut();
    }
    void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    void jumpCut()
    {
        if (input.Jump.action.WasReleasedThisFrame() && rb.velocity.y > 0 && isInAir)
        {
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * (1-jumpCutMultiplier));
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }
    void Aircheck()
    {
        if (rb.velocity.y != 0 && !grounded)
        {
            isInAir = true;
        }
    }
    void movement()
    {
        float targetSpeed = input.horizontal * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deacceleration;
        float force = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
        rb.AddForce(force * Vector2.right);
    }

    async void Dash()
    {
        // Prevent multiple dashes from overlapping
        if (isDashing) return;

        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Turn off gravity

        // Apply dash velocity in the direction the player is facing, with 0 vertical velocity
        float dashDirection = facingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashForce, 0f);

        // Wait for the duration of the dash
        await Task.Delay(TimeSpan.FromSeconds(dashDuration));

        // If the object was destroyed during the delay, exit early to prevent errors
        if (this == null) return;

        rb.gravityScale = originalGravity; // Restore gravity
        isDashing = false;
    }
    void friction()
    {
        float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Math.Abs(frictionAmount));
        amount *= Mathf.Sign(rb.velocity.x);
        rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
    }
    void flipHandle()
    {
        if (input.horizontal > 0 && !facingRight)
        {
            flip();
        }
        if (input.horizontal < 0 && facingRight)
        {
            flip();
        }
    }
    void flip()
    {
        Vector3 scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
        facingRight = !facingRight;
    }
}
