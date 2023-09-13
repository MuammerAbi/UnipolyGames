using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;


public class GetPlayerCamera : MonoBehaviour
{
    [SerializeField] Transform playerCameraRoot;
    // Start is called before the first frame update
    void Start()
    {
        NetworkObject thisObject = GetComponent<NetworkObject>();
        if (thisObject.HasStateAuthority)
        {
            GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;


        }

    }

}
