using System;
using System.Collections;
using System.Collections.Generic;
using BSPObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class BspController : MonoBehaviour
{
    [SerializeField] private Room[] rooms;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private CameraLinesController cameraLinesController;
    
    private Room _currentRoom;
    
    void Start()
    {
        SetCurrentRoom();
    }
    // test
    void Update()
    {
        SetCurrentRoom();
        CheckAdjacentRooms();
    }

    private void SetCurrentRoom()
    {
        foreach (var room in rooms)
        {
            if (room && playerPosition && room.IsPlayerInsideRoom(playerPosition))
            {
                _currentRoom = room;
            }
        }
    }

    private void CheckAdjacentRooms()
    {
        Room[] activeRooms = cameraLinesController.GetSeenRooms(_currentRoom, rooms);
        
        foreach (var room in rooms)
        {
            if (Array.Exists(activeRooms, activeRoom => room == activeRoom))
            {
               
            }
            else
            {
                
            }
        }
    }
}
