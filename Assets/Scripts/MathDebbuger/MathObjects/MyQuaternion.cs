using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public class MyQuaternion : IEquatable<MyQuaternion>, IFormattable
    {
        public float x;
        public float y;
        public float z;
        public float w;
        
        private static readonly MyQuaternion IdentityQuaternion = new MyQuaternion(0.0f, 0.0f, 0.0f, 1f);
        public const float KEpsilon = 1E-06f;

        #region accessors
        
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    case 3:
                        return this.w;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    case 3:
                        this.w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        #endregion

        #region constructors
        
        public MyQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #endregion

        #region Setters

        public void Set(float newX, float newY, float newZ, float newW)
        {
            this.x = newX;
            this.y = newY;
            this.z = newZ;
            this.w = newW;
        }

        #endregion

        #region Getters

        // Returns or sets the euler angle representation of the rotation.
        public Vector3 eulerAngles
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public static MyQuaternion Identity
        {
            get => MyQuaternion.IdentityQuaternion;
        }

        public MyQuaternion normalized
        {
            get => MyQuaternion.Normalize(this);
        }
        
        #endregion

        #region functions
        
        // Creates a rotation which rotates from fromDirection to toDirection
        public static MyQuaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            throw new NotImplementedException();
        }
        
        // Spherically interpolates between quaternions a and b by ratio t. The parameter t is clamped to the range [0, 1]
        // returns A quaternion spherically interpolated between quaternions a and b.
        public static MyQuaternion Slerp(MyQuaternion a, MyQuaternion b, float t)
        {
            throw new NotImplementedException();
        }
        
        // Spherically interpolates between a and b by t. The parameter t is not clamped.
        public static MyQuaternion SlerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
        {
            throw new NotImplementedException();
        }
        
        // Interpolates between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1]
        // returns A quaternion interpolated between quaternions a and b.
        public static MyQuaternion Lerp(MyQuaternion a, MyQuaternion b, float t)
        {
            throw new NotImplementedException();
        }

        // Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped
        public static MyQuaternion LerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
        {
            throw new NotImplementedException();
        }

        // Creates a rotation which rotates angle degrees around axis.
        public static MyQuaternion AngleAxis(float angle, Vector3 axis)
        {
            throw new NotImplementedException();
        }

        // Creates a rotation with the specified forward and upwards directions
        public static MyQuaternion LookRotation(Vector3 forward, Vector3 upwards)
        {
            throw new NotImplementedException();
        }
        public static MyQuaternion LookRotation(Vector3 forward)
        {
            return MyQuaternion.LookRotation(forward, Vector3.up);
        }

        // Creates a rotation with the specified forward and upwards directions.
        public void SetLookRotation(Vector3 view, Vector3 up)
        {
            throw new NotImplementedException();
        }

        public void SetLookRotation(Vector3 view)
        {
            throw new NotImplementedException();
        }
        
        // Returns the angle in degrees between two rotations a and b.
        public static float Angle(MyQuaternion a, MyQuaternion b)
        {
            throw new NotImplementedException();
        }

        // returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis; applied in that order.
        public static MyQuaternion Euler(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public static MyQuaternion Euler(Vector3 euler)
        {
            throw new NotImplementedException();
        }

        public void ToAngleAxis(out float angle, out Vector3 axis)
        {
            throw new NotImplementedException();
        }

        // Creates a rotation which rotates from fromDirection to toDirection.
        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            throw new NotImplementedException();
        }

        // Rotates a rotation from towards to.
        public static MyQuaternion RotateTowards(MyQuaternion from, MyQuaternion to, float maxDegreesDelta)
        {
            throw new NotImplementedException();
        }

        public float SquaredMagnitude()
        {
            return MyQuaternion.Dot(this, this);
        }
        
        public static MyQuaternion Conjugated(MyQuaternion q)
        {
            return new MyQuaternion(-q.x, -q.y, -q.z, q.w);
        }

        // Returns the inverse rotation
        public static MyQuaternion Inverse(MyQuaternion q)
        {
            return MyQuaternion.Conjugated(q) / q.SquaredMagnitude();
        }
        
        // Converts this quaternion to one with the same orientation but with a magnitude of 1
        public static MyQuaternion Normalize(MyQuaternion q)
        {
            float squaredMagnitude = q.SquaredMagnitude();
            return new MyQuaternion(q.x / squaredMagnitude, q.y / squaredMagnitude, q.z / squaredMagnitude, q.w / squaredMagnitude);
        }

        public static float Dot(MyQuaternion lhs, MyQuaternion rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w;
        }
        
        #endregion
        

        #region operators

        public static MyQuaternion operator *(MyQuaternion lhs, MyQuaternion rhs)
        {
            return new MyQuaternion(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.w * rhs.y - lhs.x * rhs.z + lhs.y * rhs.w + lhs.z * rhs.x,
                lhs.w * rhs.z	+ lhs.x * rhs.y	- lhs.y * rhs.x	+ lhs.z * rhs.w,
                lhs.w * rhs.w	- lhs.x * rhs.x	- lhs.y * rhs.y - lhs.z * rhs.z
            );
        }

        // Preguntar esto
        public static Vec3 operator *(MyQuaternion rotation, Vec3 point)
        {
            MyQuaternion pureVectorQuaternion = new MyQuaternion(point.x, point.y, point.z, 0);
            MyQuaternion appliedPureQuaternion = MyQuaternion.Conjugated(rotation) * pureVectorQuaternion * rotation;

            return new Vec3(appliedPureQuaternion.x, appliedPureQuaternion.y, appliedPureQuaternion.z);
        }

        public static bool operator ==(MyQuaternion lhs, MyQuaternion rhs)
        {
            return MyQuaternion.Dot(lhs, rhs) > 1 - KEpsilon;
        }

        public static MyQuaternion operator /(MyQuaternion q, float value)
        {
            return new MyQuaternion(
                q.x / value,
                q.y / value,
                q.z / value,
                q.w / value
                );
        }
        public static bool operator !=(MyQuaternion lhs, MyQuaternion rhs) => !(lhs == rhs);
        public override bool Equals(object other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Equals((MyQuaternion)other);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        public bool Equals(MyQuaternion other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, w);
        }
    }
}
