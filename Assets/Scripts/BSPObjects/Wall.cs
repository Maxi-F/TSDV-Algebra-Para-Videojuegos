using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private MyPlane _plane;
    [SerializeField] private float normalScale = 5.0f;
    [SerializeField] private MeshCollider[] overtures;
    
    void Start()
    {
        _plane = new MyPlane(transform.right, transform.position);
    }


    public bool IsPlayerInNormalSide(Transform player)
    {
        return _plane.GetSide(new Vec3(player.position));
    }

    public bool IsPointInNormalSide(Vec3 point)
    {
        return _plane.GetSide(point);
    }
    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            _plane.DrawNormal(new Vec3(transform.position), normalScale);
        }

    }

    public bool IntersectsWithOverture(Vec3 point)
    {
        
        return overtures.Length != 0 &&
               Array.Exists(
                   overtures, 
                   overture => CheckXYCollisionBetween(point, overture.bounds) || 
                               CheckZYCollissionBetween(point, overture.bounds));
    }

    private bool CheckXYCollisionBetween(Vec3 point, Bounds bounds)
    {
        return point.x >= bounds.center.x - bounds.extents.x &&
               point.x <= bounds.center.x + bounds.extents.x &&
               point.y >= bounds.center.y - bounds.extents.y &&
               point.y <= bounds.center.y + bounds.extents.y;
    }
    
    private bool CheckZYCollissionBetween(Vec3 point, Bounds bounds)
    {
        return point.z >= bounds.center.z - bounds.extents.z &&
               point.z <= bounds.center.z + bounds.extents.z &&
               point.y >= bounds.center.y - bounds.extents.y &&
               point.y <= bounds.center.y + bounds.extents.y;
    }
}
