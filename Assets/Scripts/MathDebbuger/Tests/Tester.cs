using System;
using System.Collections;
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
        [SerializeField] private float vectorFiveVelocity = 1f;
        [SerializeField] private float vectorTenVelocity = 1.2f;
        [SerializeField] private float excerciseTenTime = 10f;
        private Vec3 _vector;
        private Vec3 _anotherVector;
        private Vec3 _resultVector;
        private float timeElapsedExcerciseTen = 0f;
    
        private const string VectorIdentifier = "vector1";
        private const string AnotherVectorIdentifier = "vector2";
        private const string ResultIdentifier = "result";
    
        void Start()
        {
            AssignVectors();
        }

        private void OnValidate()
        {
            if(Application.isPlaying)
                UpdateVectors();
        }

        private void Update()
        {
            switch (excerciseNumber)
            {
                case Excercise.Five:
                    Vec3 direction = _anotherVector - _vector;
                    _resultVector += direction * vectorFiveVelocity * Time.deltaTime;
                    
                    if (Vec3.Angle(_vector, _anotherVector) < Vec3.Angle(_vector, _resultVector))
                    {
                        _resultVector = _vector;
                    };
                    
                    UpdateVectorPositions();
                    break;
                case Excercise.Ten:
                    Vec3 resultDirection = _vector - _anotherVector;
                    _resultVector += resultDirection * vectorTenVelocity * Time.deltaTime;
                    timeElapsedExcerciseTen += Time.deltaTime;

                    if (timeElapsedExcerciseTen > excerciseTenTime)
                    {
                        _resultVector = _anotherVector;
                        timeElapsedExcerciseTen = 0f;
                    }
                    
                    UpdateVectorPositions();
                    break;
            }
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
                    return ExcerciseThree();
                case Excercise.Four:
                    return ExcerciseFour();
                case Excercise.Five:
                    return ExcerciseFive();
                case Excercise.Six:
                    return ExcerciseSix();
                case Excercise.Seven:
                    return ExcerciseSeven();
                case Excercise.Eight:
                    return ExcerciseEight();
                case Excercise.Nine:
                    return ExcerciseNine();
                case Excercise.Ten:
                    return ExcerciseTen();
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

            UpdateVectorPositions();
        }

        private void UpdateVectorPositions()
        {
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

        private Vec3 ExcerciseThree()
        {
            Vec3 newVector = _vector;
            newVector.Scale(_anotherVector);

            return newVector;
        }
        
        private Vec3 ExcerciseFour()
        {
            return Vec3.Cross(_anotherVector, _vector);
        }
        
        private Vec3 ExcerciseFive()
        {
            return _vector;
        }
        
        private Vec3 ExcerciseSix()
        {
            return Vec3.Max(_vector, _anotherVector);
        }
        
        private Vec3 ExcerciseSeven()
        {
            return Vec3.Project(_vector, _anotherVector);
        }
        
        private Vec3 ExcerciseEight()
        {
            Vec3 sum = _vector + _anotherVector;

            return sum.normalized * Vec3.Distance(_vector, _anotherVector);
        }
        
        private Vec3 ExcerciseNine()
        {
            return Vec3.Reflect(_vector, _anotherVector.normalized);
        }
        
        private Vec3 ExcerciseTen()
        {
            timeElapsedExcerciseTen = 0f;
            return _anotherVector;
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