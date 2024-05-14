using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Wall[] _walls;
    [SerializeField] private Room[] _adjacentRooms;
    
    void Start()
    {
        _walls = GetComponentsInChildren<Wall>();
    }

    public bool IsPlayerInsideRoom(Transform player)
    {
        if (_walls == null)
        {
            _walls = GetComponentsInChildren<Wall>();
        }

        return Array.TrueForAll(_walls, wall => wall.IsPlayerInNormalSide(player));
    }

    public bool IsPointInsideRoom(Vec3 point)
    {
        return Array.TrueForAll(_walls, wall => wall.IsPointInNormalSide(point));
    }

    public bool IsRoomAdjacent(Room room)
    {
        return Array.Exists(_adjacentRooms, adjacentRoom => adjacentRoom.CompareTag(room.tag));
    }
}
