﻿using System.Collections;
using CustomMath;
using UnityEngine;

namespace MathDebbuger
{
    enum Excercise
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten
    }

    public class Tester : MonoBehaviour
    {
        [SerializeField] private Excercise excerciseNumber;
        [SerializeField] private Vector3 aVectorValues;
        [SerializeField] private Vector3 anotherVectorsValues;

        private Vec3 _vector;
        private Vec3 _anotherVector;
        private Vec3 _resultVector;
    
        private const string VectorIdentifier = "vector1";
        private const string AnotherVectorIdentifier = "vector2";
        private const string ResultIdentifier = "result";
    
        void Start()
        {
            AssignVectors();
        }
    
        // Update is called once per frame
        private void OnValidate()
        {
            UpdateVectors();
        }

        private Vec3 ExcecuteExcercise()
        {
            switch (excerciseNumber)
            {
                case Excercise.One:
                    return ExcerciseOne();
                case Excercise.Two:
                    return ExcerciseTwo();
                case Excercise.Three:
                    return Vec3.Zero;
                case Excercise.Four:
                    return Vec3.Zero;
                case Excercise.Five:
                    return Vec3.Zero;
                case Excercise.Six:
                    return Vec3.Zero;
                case Excercise.Seven:
                    return Vec3.Zero;
                case Excercise.Eight:
                    return Vec3.Zero;
                case Excercise.Nine:
                    return Vec3.Zero;
                case Excercise.Ten:
                    return Vec3.Zero;
                default:
                    return Vec3.Zero;
            }
        }

        private void AssignVectors()
        {
            _vector = new Vec3(aVectorValues.x, aVectorValues.y, aVectorValues.z);
            _anotherVector = new Vec3(anotherVectorsValues.x, anotherVectorsValues.y, anotherVectorsValues.z);
            _resultVector = ExcecuteExcercise();
            
            Vector3Debugger.AddVector(_vector, Color.green, VectorIdentifier);
            Vector3Debugger.AddVector(_anotherVector, Color.magenta, AnotherVectorIdentifier);
            Vector3Debugger.AddVector(_resultVector, Color.cyan, ResultIdentifier);
            Vector3Debugger.EnableEditorView();
        }

        private void UpdateVectors()
        {
            _vector = new Vec3(aVectorValues.x, aVectorValues.y, aVectorValues.z);
            _anotherVector = new Vec3(anotherVectorsValues.x, anotherVectorsValues.y, anotherVectorsValues.z);
            _resultVector = ExcecuteExcercise();
            
            Vector3Debugger.UpdatePosition(VectorIdentifier, _vector);
            Vector3Debugger.UpdatePosition(AnotherVectorIdentifier, _anotherVector);
            Vector3Debugger.UpdatePosition(ResultIdentifier, _resultVector);
        }

        /* sum of vectors */
        private Vec3 ExcerciseOne()
        {
            return _vector + _anotherVector;
        }
    
        /* substract vectors */
        private Vec3 ExcerciseTwo()
        {
            return _anotherVector - _vector;
        }
        
        IEnumerator UpdateVector()
        {
            for (int i = 0; i < 100; i++)
            {
                //Vector3Debugger.UpdatePosition("elAzul", new Vector3(2.4f, 6.3f, 0.5f) * (i * 0.05f));
                yield return new WaitForSeconds(0.2f);
            }
        }

    }
}