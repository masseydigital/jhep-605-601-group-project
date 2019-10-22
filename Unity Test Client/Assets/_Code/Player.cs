using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Networking.Types;

public class Player : NetworkBehaviour
{
    Broadcast broadcast;
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            Debug.Log(":: I AM SERVER ::");
        }

        if (isClient)
        {
            Debug.Log($":: I AM CLIENT: {GetComponent<NetworkIdentity>().playerControllerId} ::");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
