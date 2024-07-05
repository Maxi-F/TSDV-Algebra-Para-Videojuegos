using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

namespace MathDebbuger.Tests
{
    enum QuaternionExcercise
    {
        One,
        Two,
        Three
    }
    
    public class QuaternionTester : MonoBehaviour
    {
        [SerializeField] private QuaternionExcercise excerciseNumber;
        [SerializeField] private float angle;
        [SerializeField] private Vec3 _aVectorValues = new Vec3(10, 0, 0);

        private Vec3 _excercise2Vec1 = new Vec3(0, 10, 0);
        private Vec3 _excercise2Vec2 = new Vec3(10, 0, 0);
        
        private Vec3 _vector;
        private const string VectorIdentifier = "vector1";
        private const string Vector2Identifier1 = "vector21";
        private const string Vector2Identifier2 = "vector22";
        
        void Start()
        {
        }
        
        private void OnValidate()
        {
            if(Application.isPlaying)
                UpdateVectors();
        }
        
        void FixedUpdate()
        {
            switch (excerciseNumber)
            {
                case QuaternionExcercise.One:
                    MyQuaternion rotation = MyQuaternion.AngleAxis(angle, Vec3.Up);
                    _vector = rotation * _vector;
                    break;
                case QuaternionExcercise.Two:
                    MyQuaternion rotation2 = MyQuaternion.AngleAxis(angle, Vec3.Up);
                    _vector = rotation2 * _vector;
                    _excercise2Vec2 = rotation2 * _vector;
                    break;
            }
        }

        private void UpdateVectors()
        {
            _vector = new Vec3(_aVectorValues.x, _aVectorValues.y, _aVectorValues.z);
        }

        private void OnDrawGizmos()
        {
            switch (excerciseNumber)
            {
                case QuaternionExcercise.One:
                    Gizmos.DrawLine(transform.position, _vector);
                    break;
                case QuaternionExcercise.Two:
                    Gizmos.DrawLine(transform.position, _vector);
                    
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position + _vector, _vector + _excercise2Vec1);
                    
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position + _vector + _excercise2Vec1, _vector + _excercise2Vec1 + _excercise2Vec2);
                    break;
            }
        }
    }
}
