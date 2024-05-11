using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CustomMath
{
    public class MyPlane : IFormattable
    {
        /*
         * The equation of a plane can be determined as:
         *   
         *      N . (X - A) = 0
         * 
         * Where N is a normal vector to the plane, X and A are points that are in the plane.
         * If X and A are in the plane, then the vector formed by the substraction of those points is parallel to the plane, making the dot product equal to zero.
         * 
         * This can be determined also as:
         * 
         *      (Nx, Ny, Nz) . ( (x, y, z) - (Ax, Ay, Az) ) = 0
         *      (Nx, Ny, Nz) . (x - Ax, y - Ay, z - Az) = 0
         *      
         *      Nx(x - Ax) + Ny(y - Ay) + Nz(z - Az) = 0
         *      Nx.x - NxAx + Ny.y - NyAy + Nz.z - NzAz = 0
         *      Nx.x + Ny.y + Nz.z - (NxAx + NyAy + NzAz) = 0
         *      
         * Where if we replace Nx = A, Ny = B, Nz = C and (-NxAx - NyAy - NzAz) = D we get a simplified equation for the plane:
         * 
         *      A.x + B.y + C.z + D = 0
         */

        #region coeficients

        private Vec3 _normal;
        private float _distance;

        #endregion

        #region constructors

        private float GetDCoefficient(Vec3 normal, Vec3 point)
        {
            return -Vec3.Dot(normal, point);
        }
        public MyPlane(Vec3 inNormal, Vec3 inPoint) {
            SetNormalAndPosition(inNormal, inPoint);
        }
        public MyPlane(Vec3 inNormal, float d) {
            normal = inNormal.normalized;
            _distance = d; 
        }

        // a b and c are points. You have to create two vectors that are on the plane ((b - a) and (c - b)) and make a cross product from it 
        public MyPlane(Vec3 a, Vec3 b, Vec3 c) {
            Set3Points(a, b, c);
        }

        #endregion

        #region properties
        public Vec3 normal { get { return _normal; } set { _normal = value; } }
        public float distance { get { return _distance; } set { _distance = value; } }
        public MyPlane flipped { get { return new MyPlane(new Vec3(-normal.x, -normal.y, -normal.z), _distance); } }

        #endregion

        #region Functions

        // If we want to translate the plane, we would want all points from the translation offset
        // to be on the plane:
        // 
        //  N . (X - (A + Of)) = 0
        //
        //  Which translates to:
        //  Nx.x + Ny.y + Nz.z - (Nx(Ax + Ofx) + Ny(Ay + Ofy) + Nz(Az + Ofz)) = 0
        //  Nx(Ax + Ofx) + Ny(Ay + Ofy) + Nz(Az + Ofz) = NxAx + NyAy + NzAz + NxOfx + NyOfy + NzOfz = D + D'
        public MyPlane Translate(MyPlane plane, Vec3 translation) {
            return new MyPlane(_normal, _distance + Vec3.Dot(_normal, translation));
        }

        // The closest point on a plane from a point is going to be on the same line perpendicular to the plane, as that is the closest distance
        // the point is going to be from the plane.
        //
        // To get that line we make a function out of the normal (coefficient) and the point
        // Line(t) = Normal * t + Point
        // So we have (A * t + Px), (B * t + Py), (C * t + Pz). If we replace it on the plane ecuation:
        //
        // A(A * t + Px) + B(B * t + Py) + C(C * t + Pz) + D = 0
        // A * A * t + APx + B * B * t + BPy + C * C * t + CPz + D = 0
        // t(A * A + B * B + C * C) = APx + BPy + CPz + D
        // t = APx + Bpy + CPz + D / A * A + B * B + C * C
        //
        // Then, we obtain (A * t + Px), (B * t + Py), (C * t + Pz)
        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            float t = Vec3.Dot(_normal, point) + _distance / Vec3.Dot(_normal, _normal);

            return new Vec3(_normal.x * t + point.x, _normal.y * t + point.y, _normal.z + point.z);
        }

        // Makes the plane face in the opposite direction. flips the normal.
        public void Flip() {
            _normal = new Vec3(-_normal.x, -_normal.y, -_normal.z);
        }

        // This can be obtained by obtaining the magnitude of the scalar projection of the vector
        // that goes from a point on the plane to the point to obtain the distance from
        // to the normal vector.
        // In math words:
        //
        // d = |v . n| where v is the vector determined by a point in the plane (x0, y0, z0) and the point to obtain the distance to (x1, y1, z1), and n the normalized normal vector.
        // as n is already normalized in this plane implementation, we can determine this as:
        // d = | A(x1 - x0) + B(x1 - x0) + C(x1 - x0) | -> As we know, D = - (Ax0 + Bx0 + Cx0). then:
        // d = | Ax1 + Bx1 + Cx1 + D |
        public float GetDistanceToPoint(Vec3 point)
        {
            return Vec3.Dot(_normal, point) + _distance;
        }
        //
        // Resumen:
        //     Is a point on the positive side of the plane?
        //
        // Par�metros:
        //   point:
        public bool GetSide(Vec3 point)
        {
            return Vec3.Dot(_normal, point) + _distance > 0;
        }
        //
        // Resumen:
        //     Are two points on the same side of the plane?
        //
        // Par�metros:
        //   inPt0:
        //
        //   inPt1:
        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            return GetSide(inPt0) == GetSide(inPt1);
        }
        //
        // Resumen:
        //     Sets a plane using three points that lie within it. The points go around clockwise
        //     as you look down on the top surface of the plane.
        //
        // Par�metros:
        //   a:
        //     First point in clockwise order.
        //
        //   b:
        //     Second point in clockwise order.
        //
        //   c:
        //     Third point in clockwise order.
        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            normal = Vec3.Cross(b - a, c - b).normalized;
            _distance = GetDCoefficient(normal, a);
        }
        //
        // Resumen:
        //     Sets a plane using a point that lies within it along with a normal to orient
        //     it.
        //
        // Par�metros:
        //   inNormal:
        //     The plane's normal vector.
        //
        //   inPoint:
        //     A point that lies on the plane.
        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            normal = inNormal.normalized;
            _distance = GetDCoefficient(inNormal, inPoint);
        }

        #endregion
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "";
        }
    }
}

