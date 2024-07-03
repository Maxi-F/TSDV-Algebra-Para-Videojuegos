using CustomMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMatrix4x4 : MonoBehaviour
{
    public float m00;
    public float m33;
    public float m23;
    public float m13;
    public float m03;
    public float m32;
    public float m22;
    public float m02;
    public float m12;
    public float m21;
    public float m11;
    public float m01;
    public float m30;
    public float m20;
    public float m10;
    public float m31;

    public MyMatrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3) {
        throw new NotImplementedException();
    }

    public float this[int index] { get
        {
            throw new NotImplementedException();
        } set
        {
            throw new NotImplementedException();
        }
    }
    public float this[int row, int column] { get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    //
    // Resumen:
    //     Returns a matrix with all elements set to zero (Read Only).
    public static MyMatrix4x4 zero { get
        {
            throw new NotImplementedException();
        }
    }
    //
    // Resumen:
    //     Returns the identity matrix (Read Only).
    public static MyMatrix4x4 identity { get {
            throw new NotImplementedException();
        }
    }
    //
    // Resumen:
    //     Attempts to get a rotation quaternion from this matrix.
    public MyQuaternion rotation { get {
            throw new NotImplementedException();
        }
    }
    //
    // Resumen:
    //     Attempts to get a scale value from the matrix. (Read Only)
    public Vector3 lossyScale { get {
            throw new NotImplementedException();
        }
    }
    //
    // Resumen:
    //     Checks whether this is an identity matrix. (Read Only)
    public bool isIdentity { get {
            throw new NotImplementedException();
        }
    }
    //
    // Resumen:
    //     The determinant of the matrix. (Read Only)
    public float determinant { get {
            throw new NotImplementedException();
        }
    }
    //
    // Resumen:
    //     Returns the transpose of this matrix (Read Only).
    public MyMatrix4x4 transpose { get {
            throw new NotImplementedException();
        }
    }

    //
    // Resumen:
    //     The inverse of this matrix. (Read Only)
    public Matrix4x4 inverse { get; }

    public static float Determinant(MyMatrix4x4 m) {
        throw new NotImplementedException();
    }

    public static MyMatrix4x4 Inverse(MyMatrix4x4 m) {
        throw new NotImplementedException();
    }

    public static bool Inverse3DAffine(MyMatrix4x4 input, ref MyMatrix4x4 result) {
        throw new NotImplementedException();
    }

    //
    // Resumen:
    //     Create a "look at" matrix.
    //
    // Parámetros:
    //   from:
    //     The source point.
    //
    //   to:
    //     The target point.
    //
    //   up:
    //     The vector describing the up direction (typically Vector3.up).
    //
    // Devuelve:
    //     The resulting transformation matrix.
    public static MyMatrix4x4 LookAt(Vector3 from, Vector3 to, Vector3 up) {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Creates a rotation matrix.
    //
    // Parámetros:
    //   q:
    public static MyMatrix4x4 Rotate(MyQuaternion q) {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Creates a scaling matrix.
    //
    // Parámetros:
    //   vector:
    public static MyMatrix4x4 Scale(Vec3 vector) {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Creates a translation matrix.
    //
    // Parámetros:
    //   vector:
    public static MyMatrix4x4 Translate(Vec3 vector) {
        throw new NotImplementedException();
    }

    public static MyMatrix4x4 Transpose(MyMatrix4x4 m)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Creates a translation, rotation and scaling matrix.
    //
    // Parámetros:
    //   pos:
    //
    //   q:
    //
    //   s:
    public static MyMatrix4x4 TRS(Vec3 pos, MyQuaternion q, Vec3 s)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object other) {
        throw new NotImplementedException();
    }

    public bool Equals(MyMatrix4x4 other) {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Get a column of the matrix.
    //
    // Parámetros:
    //   index:
    public Vector4 GetColumn(int index) {
        throw new NotImplementedException();
    }

    public override int GetHashCode() {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Get position vector from the matrix.
    public Vec3 GetPosition()
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Returns a row of the matrix.
    //
    // Parámetros:
    //   index:
    public Vector4 GetRow(int index)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Transforms a position by this matrix (generic).
    //
    // Parámetros:
    //   point:
    public Vec3 MultiplyPoint(Vec3 point)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Transforms a position by this matrix (fast).
    //
    // Parámetros:
    //   point:
    public Vec3 MultiplyPoint3x4(Vec3 point)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Transforms a direction by this matrix.
    //
    // Parámetros:
    //   vector:
    public Vec3 MultiplyVector(Vec3 vector)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Sets a column of the matrix.
    //
    // Parámetros:
    //   index:
    //
    //   column:
    public void SetColumn(int index, Vector4 column)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Sets a row of the matrix.
    //
    // Parámetros:
    //   index:
    //
    //   row:
    public void SetRow(int index, Vector4 row)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Sets this matrix to a translation, rotation and scaling matrix.
    //
    // Parámetros:
    //   pos:
    //
    //   q:
    //
    //   s:
    public void SetTRS(Vec3 pos, MyQuaternion q, Vec3 s)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Returns a formatted string for this matrix.
    //
    // Parámetros:
    //   format:
    //     A numeric format string.
    //
    //   formatProvider:
    //     An object that specifies culture-specific formatting.
    public override string ToString()
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Returns a formatted string for this matrix.
    //
    // Parámetros:
    //   format:
    //     A numeric format string.
    //
    //   formatProvider:
    //     An object that specifies culture-specific formatting.
    public string ToString(string format)
    {
        throw new NotImplementedException();
    }
    //
    // Resumen:
    //     Returns a formatted string for this matrix.
    //
    // Parámetros:
    //   format:
    //     A numeric format string.
    //
    //   formatProvider:
    //     An object that specifies culture-specific formatting.
    public string ToString(string format, IFormatProvider formatProvider)
    {
        throw new NotImplementedException();
    }

    public bool ValidTRS() {
        throw new NotImplementedException();
    }

    /*
    public static Vector4 operator *(Matrix4x4 lhs, Vector4 vector) {
        throw new NotImplementedException();
    }

    public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
    {
        throw new NotImplementedException();
    }
    public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
    {
        throw new NotImplementedException();
    }
    */
}
