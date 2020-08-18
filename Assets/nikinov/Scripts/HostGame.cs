using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HostGame : MonoBehaviour
{

    [SerializeField] private uint roomSize = 6;
    private string roomName;
    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void SetRoomName (string _name)
    {
        roomName = _name;
    }

    public void CreateRoom ()
    {
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
            // crate room

        }
    }
}
