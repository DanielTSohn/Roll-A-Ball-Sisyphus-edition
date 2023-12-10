using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Action<Vector2> MovementAction;

    public static PlayerInputHandler Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ReadMovement(InputAction.CallbackContext movement)
    {
        Vector2 moveValue = movement.ReadValue<Vector2>();
        if(moveValue != null) MovementAction?.Invoke(moveValue);
    }
}