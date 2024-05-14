using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private MyPlane _plane;
    [SerializeField] private float normalScale = 5.0f;
    
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
        if(Application.isPlaying) 
            _plane.DrawNormal(new Vec3(transform.position), normalScale);
    }
}
