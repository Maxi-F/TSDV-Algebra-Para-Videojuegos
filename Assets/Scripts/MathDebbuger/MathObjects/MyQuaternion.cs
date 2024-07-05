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
        public Vec3 EulerAngles
        {
            get
            {
                /* First, check if there is a singularity (if the X angle is in the north (90) or south(-90) degrees) */
                float unitToUse = this.SquaredMagnitude(); // Unit value to multiply if quaternion was not normalized. If normalized this value is one
                float testXAngle = this.x * this.w - this.y * this.z;

                float errorValue = 0.4999f;
                
                // if there is a singularity at the north pole
                if (testXAngle > errorValue * unitToUse)
                {
                    // Set values to not have gimbal lock. (X with value and Z = 0)
                    return NormalizeAngles(new Vec3(Mathf.PI / 2, 2f * Mathf.Atan2(this.y, this.x), 0));
                } 
                // if there is a singularity at the south pole
                if (testXAngle < -errorValue * unitToUse)
                {
                    // Set values to not have gimbal lock. (X with value and Z = 0)
                    return NormalizeAngles(new Vec3(-Mathf.PI / 2, -2f * Mathf.Atan2(this.y, this.x), 0));
                }
                
                // No singularities. Then, we apply the inverse of the euler angle to quaternion conversion.
                
                // As we are using this as reference and it does a Z Y X conversion: https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles#:~:text=q%3B%0A%7D-,Quaternion%20to%20Euler%20angles%20(in%203%2D2%2D1%20sequence)%20conversion,-%5Bedit%5D
                // We use another quaternion with it's values interchanged, to make the same calculations.
                MyQuaternion qToCalc = new MyQuaternion(this.w, this.z, this.x, this.y);

                return NormalizeAngles(
                    new Vec3(
                            Mathf.Atan2(2f * (qToCalc.x * qToCalc.w + qToCalc.y * qToCalc.z), 1 - 2f * (qToCalc.z * qToCalc.z + qToCalc.w * qToCalc.w)),
                            Mathf.Asin(2f * (qToCalc.x * qToCalc.z - qToCalc.w * qToCalc.y)),
                            Mathf.Atan2(2f * (qToCalc.x * qToCalc.y + qToCalc.z * qToCalc.w), 1 - 2f * (qToCalc.y * qToCalc.y + qToCalc.z * qToCalc.z))
                        )
                );
            }
            set
            {
                // Each euler angle represents a rotation in respect to an identity quaternion.
                // To create a quaternion from euler angles, it creates 3 quaternions representing each euler angle rotation,
                // Then you multiply in the order of y x z.

                float xInRad = Mathf.Deg2Rad * value.x * 0.5f;
                float yInRad = Mathf.Deg2Rad * value.y * 0.5f;
                float zInRad = Mathf.Deg2Rad * value.z * 0.5f;
                
                MyQuaternion qx = new MyQuaternion(Mathf.Sin(xInRad), 0, 0, Mathf.Cos(xInRad));
                MyQuaternion qy = new MyQuaternion(0, Mathf.Sin(yInRad), 0, Mathf.Cos(yInRad));
                MyQuaternion qz = new MyQuaternion(0, 0, Mathf.Sin(zInRad), Mathf.Cos(zInRad));

                MyQuaternion result = qy * qx * qz;

                this.x = result.x;
                this.y = result.y;
                this.z = result.z;
                this.w = result.w;
            }
        }
        // 1 1 -1 1
        
        public static MyQuaternion operator *(MyQuaternion lhs, MyQuaternion rhs)
        {
            return new MyQuaternion(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.w * rhs.y - lhs.x * rhs.z + lhs.y * rhs.w + lhs.z * rhs.x,
                lhs.w * rhs.z	+ lhs.x * rhs.y	- lhs.y * rhs.x	+ lhs.z * rhs.w,
                lhs.w * rhs.w	- lhs.x * rhs.x	- lhs.y * rhs.y - lhs.z * rhs.z
            );
        }

        private Vec3 NormalizeAngles(Vec3 someEulerAngles)
        {
            return new Vec3(NormalizeAngle(someEulerAngles.x), NormalizeAngle(someEulerAngles.y),
                NormalizeAngle(someEulerAngles.z));
        }

        private float NormalizeAngle(float angle)
        {
            float newAngle = angle;
            while (newAngle > 360.0f)
            {
                newAngle -= 360.0f;
            }

            while (newAngle < 0.0f)
            {
                newAngle += 360.0f;
            }

            return newAngle;
        }
        
        public static MyQuaternion Identity
        {
            get => MyQuaternion.IdentityQuaternion;
        }

        public MyQuaternion Normalized
        {
            get => MyQuaternion.Normalize(this);
        }
        
        #endregion

        #region functions
        
        // Creates a rotation which rotates from fromDirection to toDirection
        public static MyQuaternion FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            // First, create the axis vector using cross product
            Vec3 axis = Vec3.Cross(fromDirection, toDirection);

            // Then we obtain the angle between the two vectors
            float angle = Vec3.Angle(fromDirection, toDirection);

            // Then the quaternion rotaion is made out of these two values
            // (If we rotate the fromDirection vector in the plane generated by the axis with the angle we have the rotation)
            return AngleAxis(angle, axis);
        }
        
        // Spherically interpolates between quaternions a and b by ratio t. The parameter t is clamped to the range [0, 1]
        // returns A quaternion spherically interpolated between quaternions a and b.
        public static MyQuaternion Slerp(MyQuaternion a, MyQuaternion b, float t)
        {
            return SlerpUnclamped(a, b, t < 0 ? 0 : (t > 1 ? 1 : t));
        }

        // Spherically interpolates between a and b by t. The parameter t is not clamped.
        public static MyQuaternion SlerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
        {
            // https://en.wikipedia.org/wiki/Slerp#:~:text=0%20and%C2%A01.-,Geometric%20slerp,-%5Bedit%5D

            MyQuaternion normA = a.Normalized;
            MyQuaternion normB = b.Normalized;

            float cosOmega = Dot(normA, normB);

            if (cosOmega < 0.0f)
            {
                // Flip the interpolation
                cosOmega = -cosOmega;
            }

            float coeff1, coeff2;

            float omega = Mathf.Acos(cosOmega);

            coeff1 = Mathf.Sin((1 - t) * omega) / Mathf.Sin(omega);
            coeff2 = (cosOmega < 0.0f ? -1 : 1) * (Mathf.Sin(t * omega) / Mathf.Sin(omega));

            return new MyQuaternion(
                    coeff1 * normA.x + coeff2 * normB.x,
                    coeff1 * normA.y + coeff2 * normB.y,
                    coeff1 * normA.z + coeff2 * normB.z,
                    coeff1 * normA.w + coeff2 * normB.w
                );
        }
        
        // Interpolates between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1]
        // returns A quaternion interpolated between quaternions a and b.
        public static MyQuaternion Lerp(MyQuaternion a, MyQuaternion b, float t)
        {
            return LerpUnclamped(a, b, t < 0 ? 0 : (t > 1 ? 1 : t));
        }

        // Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped
        public static MyQuaternion LerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
        {
            MyQuaternion result = Identity;

            if(Dot(a, b) >= float.Epsilon) // Checks which is the shortest path to rotate thowards that path.
            {
                result.x = a.x + (b.x - a.x) * t;
                result.y = a.y + (b.y - a.y) * t;
                result.z = a.z + (b.z - a.z) * t;
                result.w = a.w + (b.w - a.w) * t;
            } else // Go in other direction
            {
                result.x = a.x - (b.x - a.x) * t;
                result.y = a.y - (b.y - a.y) * t;
                result.z = a.z - (b.z - a.z) * t;
                result.w = a.w - (b.w - a.w) * t;
            }

            return result;
        }

        // Creates a rotation which rotates angle degrees around axis.
        public static MyQuaternion AngleAxis(float angle, Vec3 axis)
        {
            Vec3 normalizedAxis = axis.normalized;
            
            // Multiplies by the angle of incidence of the imaginary part of the quaternion
            normalizedAxis *= Mathf.Sin(angle * Mathf.Deg2Rad * 0.5f);

            return new MyQuaternion(normalizedAxis.x, normalizedAxis.y, normalizedAxis.z, Mathf.Cos(angle * Mathf.Deg2Rad * 0.5f));
        }

        // Creates a rotation with the specified forward and upwards directions
        public static MyQuaternion LookRotation(Vec3 forward, Vec3 upwards)
        {
            // If forward is almost zero return identity
            if (forward.magnitude <= KEpsilon) return MyQuaternion.Identity;

            // First, set the axis to use for the rotation
            Vec3 forwardToUse = forward.normalized;
            Vec3 rightToUse = Vec3.Cross(upwards, forward).normalized;
            Vec3 upToUse = upwards.normalized;
            
            // Now we have to make the rotation matrix, using the created axis values.
            // every row of the matrix is one axis in the order of X Y Z 
            
            float m00 = rightToUse.x;
            float m01 = rightToUse.y;
            float m02 = rightToUse.z;

            float m10 = upToUse.x;
            float m11 = upToUse.y;
            float m12 = upToUse.z;

            float m20 = forwardToUse.x;
            float m21 = forwardToUse.y;
            float m22 = forwardToUse.z;
            
            // Lastly, we have to do a conversion from the Rotation Matrix to a Quaternion.

            return GetQuaternionFromRotationMatrix(
                new Vec3(m00, m10, m20),
                new Vec3(m10, m11, m12),
                new Vec3(m20, m21, m22)
            );
        }

        public static MyQuaternion GetQuaternionFromRotationMatrix(Vec3 column1, Vec3 column2, Vec3 column3)
        {
            // The base of the code is this paper https://d3cw3dd2w32x2b.cloudfront.net/wp-content/uploads/2015/01/matrix-to-quat.pdf (Thanks Michael Day!)
            
            // First we have to know which equations we should use, depending on which quaternion value we are
            // Certain it will not be zero. Then, we apply the correct matrix equations from the sums and substractions
            // created equations.

            MyQuaternion result;
            float factor;
            
            if (column3.z < 0) // sqr(X) + sqr(Y) > 1/2, which is the same to say that |(X, Y)| > |(Z, W)| if normalized
            {
                // We know for certain sqr(X) + sqr(Y) > 1/2, so we have to check which one is bigger to be certain its not zero
                if (column1.x > column2.y) // is X bigger than Y ?
                {
                    // We know for certain X is not zero, so we take the x value from the trace.
                    factor = 1 + column1.x - column2.y - column3.z; // sqr(X)
                    
                    // And the result is the equations that have multiplied by 4X.
                    result = new MyQuaternion(factor, column2.x + column1.y, column3.x + column1.z, column2.z - column3.y);
                }
                else
                {
                    // We know for certain Y is not zero
                    factor = 1 - column1.x + column2.y - column3.z; // sqr(Y)

                    // And the result is the equations that have multiplied by 4Y.
                    result = new MyQuaternion(column1.y + column2.x, factor,  column2.z + column3.y, column3.x - column1.z);
                }
            }
            else
            {
                // We know for certain sqr(Z) + sqr(W) > 1/2, so we have to check which one is bigger to be certain its not zero
                if (column1.x < -column2.y) // Is Z bigger than W ?
                {
                    // We know for certain Z is not zero
                    factor = 1 - column1.x - column2.y + column3.z; // sqr(Z)

                    // And the result is the equations that are multiplied by 4Z.
                    result = new MyQuaternion(column3.x + column1.z, column2.z + column3.y, factor, column1.y - column2.x);
                }
                else
                {
                    // We know for certain W is not zero
                    factor = 1 + column1.x + column2.y + column3.z; // sqr(W)
                    
                    // And the result is the equations that are multiplied by 4W.
                    result = new MyQuaternion(column2.z - column3.y, column3.x - column1.z, column1.y - column2.x, factor);
                }
            }
            // Finally, we have to take out the factor that is in the quaternion.

            result *= 0.5f / Mathf.Sqrt(factor);

            return result;
        }
        public static MyQuaternion LookRotation(Vec3 forward)
        {
            return MyQuaternion.LookRotation(forward, Vec3.Up);
        }

        // Creates a rotation with the specified forward and upwards directions.
        public void SetLookRotation(Vec3 view, Vec3 up)
        {
            MyQuaternion lookRotationQuaternion = LookRotation(view, up);

            this.x = lookRotationQuaternion.x;
            this.y = lookRotationQuaternion.y;
            this.z = lookRotationQuaternion.z;
        }

        public void SetLookRotation(Vec3 view)
        {
            SetLookRotation(view, Vec3.Up);
        }
        
        // Returns the angle in degrees between two rotations a and b.
        public static float Angle(MyQuaternion a, MyQuaternion b)
        {
            // It is an analogue implementation to vec 3 angle.

            float dotValue = Dot(a.Normalized, b.Normalized);
            float dotAbsValue = Mathf.Abs(dotValue);

            // It's multiplied by 2 to re-obtain the incidence we divided in the quaternion formula.
            return IsEqualUsingDot(dotValue) ? 0.0f : Mathf.Acos(dotAbsValue) * 2.0f * Mathf.Rad2Deg; 
        }

        public static bool IsEqualUsingDot(float dotValue)
        {
            return dotValue > 1 - float.Epsilon && dotValue < 1 + float.Epsilon;
        }

        // returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis; applied in that order.
        public static MyQuaternion Euler(float x, float y, float z)
        {
            MyQuaternion quaternion = MyQuaternion.Identity;
            
            quaternion.EulerAngles.Set(x, y, z);

            return quaternion;
        }

        public static MyQuaternion Euler(Vector3 euler)
        {
            MyQuaternion quaternion = MyQuaternion.Identity;
            
            quaternion.EulerAngles.Set(euler.x, euler.y, euler.z);

            return quaternion;
        }

        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            MyQuaternion thisNormalized = this.Normalized;

            // To obtain the angle we take it from the real part of the quaternion
            angle = 2.0f * Mathf.Acos(thisNormalized.w);

            // To obtain the axis values, first we check if we are almost an identity quaternion
            float magnitude = Mathf.Sqrt(1f - thisNormalized.w * thisNormalized.w);

            // if magnitude is almost zero, we return any axis, as there is no rotation
            if(magnitude < 0.0001f)
            {
                axis = new Vec3(1, 0, 0);
            } else
            {
                // If we have a rotation, then we divide the imaginary values by the sin of the angle
                // Note: This does not perform well as we are diving by floats, but for understanding ill leave it like so
                axis = new Vec3(
                    thisNormalized.x / Mathf.Sin(angle / 2f),
                    thisNormalized.y / Mathf.Sin(angle / 2f),
                    thisNormalized.z / Mathf.Sin(angle / 2f)
                    );
            }
        }

        // Creates a rotation which rotates from fromDirection to toDirection.
        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            MyQuaternion newQuaternion = FromToRotation(fromDirection, toDirection).Normalized;

            this.x = newQuaternion.x;
            this.y = newQuaternion.y;
            this.z = newQuaternion.z;
            this.w = newQuaternion.w;
        }

        // Rotates a rotation from towards to.
        public static MyQuaternion RotateTowards(MyQuaternion from, MyQuaternion to, float maxDegreesDelta)
        {
            float angle = MyQuaternion.Angle(from, to);

            if (angle == 0.0f) return to; // If no angle is specified, return one of the quaternions as both are the same.

            // Makes a SLERP taking into account the t as the max degrees to rotate over the angle between quaternions
            return MyQuaternion.SlerpUnclamped(from, to, maxDegreesDelta / angle);
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



        public static MyQuaternion operator *(MyQuaternion q, float value)
        {
            return new MyQuaternion(
                q.x * value,
                q.y * value,
                q.z * value,
                q.w * value
            );
        }
        
        // https://eater.net/quaternions/video/rotation
        public static Vec3 operator *(MyQuaternion rotation, Vec3 point)
        {
            MyQuaternion pureVectorQuaternion = new MyQuaternion(point.x, point.y, point.z, 0);
            MyQuaternion appliedPureQuaternion = rotation * pureVectorQuaternion * MyQuaternion.Conjugated(rotation);

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
            return $"( X = ${this.x}, Y = ${this.y}, Z = ${this.z}, W = ${this.w})";
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
