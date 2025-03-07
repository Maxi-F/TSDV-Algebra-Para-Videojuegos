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

    public Vec3 GetMostNearPointFromWalls(Vec3 start, Vec3 end, out bool collidesWithOwnRoomWall)
    {
        Vec3 lastContactToReturn = end;
        collidesWithOwnRoomWall = true;
        
        foreach (var wall in _walls)
        {
            if (wall.HasLineInBetween(start, lastContactToReturn))
            {
                wall.GetContactInWall(start, lastContactToReturn, out Vec3 newLastContact);
                float segmentLength = (lastContactToReturn - start).magnitude;
                float newSegmentLength = (newLastContact - start).magnitude;

                if (wall.IntersectsWithOverture(newLastContact))
                {
                    collidesWithOwnRoomWall = false;
                    lastContactToReturn = end;
                    continue;
                }
                lastContactToReturn = newSegmentLength > segmentLength ? lastContactToReturn : newLastContact;
            }
        }

        return lastContactToReturn;
    }

    public bool CheckWallOverturesWith(Vec3 startingPoint, Vec3 endPoint, Room nonAdjacentRoom)
    {
        Wall wallToCheck = Array.Find(_walls, wall => wall.HasOvertureWith(nonAdjacentRoom));

        if (!wallToCheck || !wallToCheck.GetContactInWall(startingPoint, endPoint, out Vec3 contact)) return false;

        return wallToCheck.IntersectsWithOverture(contact);
    }
}
