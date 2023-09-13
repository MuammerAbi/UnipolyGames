using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonClick : MonoBehaviourPunCallbacks
{
    [SerializeField] float raycastDistance = 10f;
    [SerializeField] LayerMask buttonLayer;
    [SerializeField] float doorSpeed = 1.0f;
    [SerializeField] GameObject doorObject;
    private bool isDoorOpen = false;

    Camera cam;
    private void Start()
    {

    }
    private void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>().transform.position, GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>().transform.forward, out hit, raycastDistance, buttonLayer))
            {
                photonView.RPC("ToggleDoorState", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void ToggleDoorState()
    {
        if (!isDoorOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            StartCoroutine(CloseDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        float startTime = Time.time;
        Vector3 startPos = doorObject.transform.position;
        Vector3 endPos = startPos - doorObject.transform.right * 4f; // Sola do?ru hareket eden yeni pozisyon

        while (Time.time - startTime <= 1.0f / doorSpeed)
        {
            float t = (Time.time - startTime) * doorSpeed;
            doorObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        isDoorOpen = true;
    }

    IEnumerator CloseDoor()
    {
        float startTime = Time.time;
        Vector3 startPos = doorObject.transform.position;
        Vector3 endPos = startPos + doorObject.transform.right * 4f; // Kap?y? ba?lang?ç konumuna geri getir

        while (Time.time - startTime <= 1.0f / doorSpeed)
        {
            float t = (Time.time - startTime) * doorSpeed;
            doorObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        isDoorOpen = false;
    }

}