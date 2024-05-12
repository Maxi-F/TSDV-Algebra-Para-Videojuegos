using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    [SerializeField] private Transform unityPoint;
    [SerializeField] private Transform testPoint;
    [SerializeField] private Transform planePoint;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var _plane = new MyPlane(new Vec3(transform.up), new Vec3(transform.position));
        var _unityPlane = new Plane(transform.up, transform.position);
        
        planePoint.position = _plane.ClosestPointOnPlane(new Vec3(testPoint.position));
        unityPoint.position = _unityPlane.ClosestPointOnPlane(testPoint.position);
        
        Debug.Log($"my plane normal: ${_plane.normal}");
        Debug.Log($"Unity plane normal: ${_unityPlane.normal}");
    }
}
