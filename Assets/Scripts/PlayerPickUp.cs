using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPickUp : MonoBehaviourPunCallbacks
{
    [SerializeField] float raycastDistance;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] Transform pickupPosition;
    [SerializeField] KeyCode pickupButton = KeyCode.E;
    [SerializeField] KeyCode dropButton = KeyCode.G;

    Camera cam;
    bool hasObjectInHand;
    GameObject objInHand;
    Transform worldObjectHolder;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();
        worldObjectHolder = GameObject.FindGameObjectWithTag("WorldObjects").transform;
    }
    
    private void Update()
    {
        if (photonView.IsMine) // Yaln?zca yerel oyuncu raycast'? kullanmal?d?r.
        {
            if (Input.GetKeyDown(pickupButton))
                Pickup();

            if (Input.GetKeyDown(dropButton))
                Drop();
        }
    }

    void Pickup()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, raycastDistance, pickupLayer))
        {
            if (!hasObjectInHand)
            {
                objInHand = hit.transform.gameObject;
                photonView.RPC("SetObjectInHand", RpcTarget.All, objInHand.GetPhotonView().ViewID, pickupPosition.position, pickupPosition.rotation);
                hasObjectInHand = true;
            }
        }
    }

    [PunRPC]
    void SetObjectInHand(int objectViewID, Vector3 position, Quaternion rotation)
    {
        GameObject obj = PhotonView.Find(objectViewID).gameObject;
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.parent = photonView.gameObject.transform;

        if (obj.GetComponent<Rigidbody>() != null)
            obj.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Drop()
    {
        if (!hasObjectInHand)
            return;

        photonView.RPC("DropObject", RpcTarget.All, objInHand.GetPhotonView().ViewID);
        hasObjectInHand = false;
        objInHand = null;
    }

    [PunRPC]
    void DropObject(int objectViewID)
    {
        GameObject obj = PhotonView.Find(objectViewID).gameObject;
        obj.transform.parent = worldObjectHolder;

        if (obj.GetComponent<Rigidbody>() != null)
            obj.GetComponent<Rigidbody>().isKinematic = false;
    }
}