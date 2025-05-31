using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    void Update()
    {
        if(grounded){
            coyoteTimer = coyoteTime;
            isInAir = false;
        }else{
            coyoteTimer -= Time.deltaTime;
        }
        jumpHandler();
        Aircheck();
    }

    void FixedUpdate(){
        movement();
        flipHandle();
        if(grounded && Mathf.Abs(input.horizontal) < 0.01f){
            friction();
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        Debug.Log(Convert.ToString(groundLayerMask,2).PadLeft(32,'0'));  // layers are stored in bitmask this prints how ground layer is represented in the bits
        if ((groundLayerMask.value & (1 << other.gameObject.layer)) != 0){
            Vector3 normalvect = other.GetContact(0).normal;
            if(normalvect == Vector3.up){
                grounded = true;
            }
        }
    }
    void OnCollisionExit2D(Collision2D other){
        //Debug.Log(Convert.ToString(groundLayerMask,2).PadLeft(32,'0'));  // layers are stored in bitmask this prints how ground layer is represented in the bits
        if((groundLayerMask.value & (1 << other.gameObject.layer)) != 0){
            grounded = false;
        }
    }
    
    void jumpHandler(){
        if(Input.GetKeyDown(KeyCode.Space)){
            jumpBufferTimer = jumpBufferTime;
        }else{
            jumpBufferTimer -= Time.deltaTime;
        }
        if(coyoteTimer > 0 && Input.GetKeyDown(KeyCode.Space) && isInAir){
            jump();
        }
        if(jumpBufferTimer>0 && !isInAir){
            jump();
        }
        jumpCut();
    }
    void jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    void jumpCut(){
        if(Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0 && isInAir){
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * (1-jumpCutMultiplier));
            rb.AddForce(Vector2.down * rb.velocity.y * (1-jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }
    void Aircheck(){
        if(rb.velocity.y != 0 && !grounded){
            isInAir = true;
        }
    }
    void movement(){
        float targetSpeed = input.horizontal * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration:deacceleration;
        float force = Mathf.Pow(Mathf.Abs(speedDiff)*accelRate, velPower) * Mathf.Sign(speedDiff);
        rb.AddForce(force * Vector2.right);
    }
    void friction(){
        float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Math.Abs(frictionAmount));
        amount *= Mathf.Sign(rb.velocity.x);
        rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
    }

    void flipHandle(){
        if(input.horizontal > 0 && !facingRight){
            flip();
        }if(input.horizontal < 0 && facingRight){
            flip();
        }
    }
    void flip(){
        Vector3 scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
        facingRight = !facingRight;
    }
}
