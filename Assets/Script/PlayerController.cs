using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float mouseSensitivity = 3.5f;
    private bool lockCursor = true;

    public float walkSpeed = 6f;
    public float gravity = -13f;
    private float velocityY = 0f;
    private CharacterController controller;

    [SerializeField] [Range(0.0f, 0.5f)] private float moveSmooth = 0.3f;

    private Vector2 currentDirec = Vector2.zero;
    private Vector2 currentDirecVelocity = Vector2.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            UpdateMouseLook();
            UpdateMovement();
        }
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), 0);

        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    { 
        Vector2 targetDirec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        targetDirec.Normalize();

        currentDirec = Vector2.SmoothDamp(currentDirec, targetDirec, ref currentDirecVelocity, moveSmooth);

        if (controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;
        
        Vector3 velocity = (transform.forward * currentDirec.y + transform.right * currentDirec.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
    
}
