using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackSystem : MonoBehaviour
{
    private PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        input.actions["Attack"].started += Attack;
    }
    private void OnDisable()
    {
        input.actions["Attack"].started -= Attack;
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        Debug.Log("Estoy atacando");
        if (Physics.Raycast(Camera.main.transform.position,
                Camera.main.transform.forward, out RaycastHit hit))
        {
            
        }
    }

    
    
    
}
