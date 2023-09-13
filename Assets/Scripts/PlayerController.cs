using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    private PhotonView pw;

    private void Start()
    {
        pw = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (pw.IsMine)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            // Hareket yönünü ve h?z?n? hesapla
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
            float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // Z?plama i?lemi
            if (characterController.isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }
                else
                {
                    moveDirection.y = 0;
                }
            }
            else
            {
                moveDirection.y = movementDirectionY - gravity * Time.deltaTime;
            }

            // Hareketi uygula
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}