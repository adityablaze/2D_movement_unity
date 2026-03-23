using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputManager : MonoBehaviour
{
    public InputActionReference Horizontal;
    public InputActionReference Vertical;
    public InputActionReference Jump;
    public InputActionReference Attack;
    public InputActionReference Dash;
    public float horizontal;
    public float vertical;
    public event Action JumpPressed;
    public event Action DashPressed;
    void OnEnable()
    {
        Horizontal.action.Enable();
        Vertical.action.Enable();
        Jump.action.Enable();
        Attack.action.Enable();
        Dash.action.Enable();
    }
    void OnDisable()
    {
        Horizontal.action.Disable();
        Vertical.action.Disable();
        Jump.action.Disable();
        Attack.action.Disable();
        Dash.action.Disable();
    }
    void Start()
    {

    }

    void Update()
    {
        horizontal = Horizontal.action.ReadValue<float>();
        vertical = Vertical.action.ReadValue<float>();
        if (Jump.action.WasPressedThisFrame())
        {
            JumpPressed?.Invoke();
        }
        if (Dash.action.WasPressedThisFrame())
        {
            DashPressed?.Invoke();
        }
    }
}
