using System.Collections;
using System.Collections.Generic;
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
        foreach (var wall in _walls)
        {
            if(!wall.IsPlayerInNormalSide(player)) return false;
        }

        return true;
    }
}
