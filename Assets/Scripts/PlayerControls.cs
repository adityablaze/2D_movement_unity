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

    [SerializeField] float acceleration;
    [SerializeField] float deacceleration;
    [SerializeField] float moveSpeed;
    [SerializeField] float velPower;

    Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        attackpressed = Input.GetKey(KeyCode.Mouse0);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate(){
        float targetSpeed = horizontal*moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration:deacceleration;
        float force = Mathf.Pow(Mathf.Abs(speedDiff)*accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(force * Vector2.right);
    }
}
