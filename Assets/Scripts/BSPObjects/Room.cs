using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Wall[] _walls;
    [SerializeField] private Room[] _adjacentRooms;

    private bool _isActive = true;
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

    public void DeactivateMesh()
    {
        if (!_isActive) return;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }

        _isActive = false;
    }

    public void ActivateMesh()
    {
        if (_isActive) return;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = true;
        }

        _isActive = true;
    }

    public Wall GetIntersectedWall(Vec3 initialPoint, Vec3 point)
    {
        return Array.Find(_walls, wall => wall.HasLineInBetween(initialPoint, point));
    }
}
