using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float horizontal;
    [SerializeField] float vertical;
    [SerializeField] bool attackpressed;
    [SerializeField] bool jumpPressed;

    [SerializeField] float acceleration;
    [SerializeField] float deacceleration;
    [SerializeField] float moveSpeed;
    [SerializeField] float velPower;
    [SerializeField] float jumpForce;

    Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        attackpressed = Input.GetKey(KeyCode.Mouse0);
        jumpPressed = Input.GetKey(KeyCode.Space);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(KeyCode.Space)){
            jump();
        }
    }

    void FixedUpdate(){
        movement();
        
    }


    void jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    void movement(){
        float targetSpeed = horizontal*moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration:deacceleration;
        float force = Mathf.Pow(Mathf.Abs(speedDiff)*accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(force * Vector2.right);
    }
}
