using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCamera : MonoBehaviourPunCallbacks
{
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    private float rotationX = 0;

    private PhotonView pw;

    private void Start()
    {
        pw = transform.GetComponentInParent<PhotonView>();

        if (pw.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Karakter yerel oyuncuya ait de?ilse bu bile?eni devre d??? b?rak?n veya silin
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (pw.IsMine)
        {
            // Fare ile kamera kontrolü
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * lookSpeed;

            rotationX += mouseY;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.parent.rotation *= Quaternion.Euler(0, mouseX, 0);
        }
    }
}
