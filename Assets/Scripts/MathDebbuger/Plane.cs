using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CustomMath
{
    public class Plane : IFormattable
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
        public Plane(Vec3 inNormal, Vec3 inPoint) {
            normal = inNormal.normalized;
            _distance = GetDCoefficient(inNormal, inPoint);
        }
        public Plane(Vec3 inNormal, float d) {
            normal = inNormal.normalized;
            _distance = d; 
        }
        public Plane(Vec3 a, Vec3 b, Vec3 c) {
            normal = Vec3.Cross(c , b).normalized;
            _distance = GetDCoefficient(normal, c);
        }

        #endregion

        #region properties
        public Vec3 normal { get { return _normal; } set { _normal = value; } }
        public float distance { get { return _distance; } set { _distance = value; } }
        public Plane flipped { get { return new Plane(new Vec3(-normal.x, -normal.y, -normal.z), _distance); } }

        #endregion

        #region Functions

        #endregion
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "";
        }
    }
}

