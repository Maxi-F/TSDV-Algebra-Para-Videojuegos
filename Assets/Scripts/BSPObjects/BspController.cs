using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BspController : MonoBehaviour
{
    [SerializeField] private Room[] _rooms;
    [SerializeField] private Transform _playerPosition;

    private Room _currentRoom;
    
    void Start()
    {
        SetCurrentRoom();
    }
    
    void Update()
    {
        SetCurrentRoom();
    }

    private void SetCurrentRoom()
    {
        foreach (var room in _rooms)
        {
            if (room.IsPlayerInsideRoom(_playerPosition))
            {
                _currentRoom = room;
            }
        }
        
        Debug.Log(_currentRoom.tag);
    }
}
