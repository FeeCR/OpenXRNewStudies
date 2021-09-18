using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    Animator animator;

    [SerializeField] private InputActionReference gripActivate;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gripActivate.action.performed += GripActivated;
        gripActivate.action.canceled += GripCanceled;
    }

    private void GripCanceled (InputAction.CallbackContext obj) {
        animator.SetBool("fist", false);
    }

    private void GripActivated (InputAction.CallbackContext obj) {
        animator.SetBool("fist", true);
    }

}
