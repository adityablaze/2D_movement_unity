using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Space(10)]
    [SerializeField] float acceleration;
    [SerializeField] float deacceleration;
    [SerializeField] float moveSpeed;
    [SerializeField] float velPower;
    [SerializeField] float jumpForce;
    
    [Space(30)]
    [SerializeField] Vector2 castboxSize;
    [SerializeField] float castDistance;
    [SerializeField] LayerMask groundLayer;


    Rigidbody2D rb;
    inputManager input;
    void Start()
    {
        input = gameObject.GetComponent<inputManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isgrounded()){
            jump();
        }
    }

    void FixedUpdate(){
        movement();
    }

    bool isgrounded(){
        if(Physics2D.BoxCast(transform.position,castboxSize,0,Vector2.down,castDistance,groundLayer))
            return true;
        else
            return false;
    }
    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, castboxSize);
    }
    void jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void movement(){
        float targetSpeed = input.horizontal * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration:deacceleration;
        float force = Mathf.Pow(Mathf.Abs(speedDiff)*accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(force * Vector2.right);
    }

}
