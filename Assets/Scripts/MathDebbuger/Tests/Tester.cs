using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathDebbuger;
using CustomMath;

enum Excercise
{
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT,
    NINE,
    TEN
}

public class Tester : MonoBehaviour
{
    [SerializeField] private Excercise excerciseNumber;
    [SerializeField] private Vector3 aVectorValues;
    [SerializeField] private Vector3 anotherVectorsValues;

    private Vec3 _vector;
    private Vec3 _anotherVector;
    
    private const string VECTOR_IDENTIFIER = "vector1";
    private const string ANOTHER_VECTOR_IDENTIFIER = "vector2";
    private const string RESULT_IDENTIFIER = "result";
    
    void Start()
    {
        AssignVectors();
        ExcerciseOne();
    }
    
    // Update is called once per frame
    private void OnValidate()
    {
        Vector3Debugger.DeleteVector(VECTOR_IDENTIFIER);
        Vector3Debugger.DeleteVector(ANOTHER_VECTOR_IDENTIFIER);
        Vector3Debugger.DeleteVector(RESULT_IDENTIFIER);
        
        AssignVectors();
        ExcerciseOne();
    }

    private void AssignVectors()
    {
        _vector = new Vec3(aVectorValues.x, aVectorValues.y, aVectorValues.z);
        _anotherVector = new Vec3(anotherVectorsValues.x, anotherVectorsValues.y, anotherVectorsValues.z);
    }

    private void ExcerciseOne()
    {
        Vec3 sumOfVectors = _vector + _anotherVector;
        
        Vector3Debugger.AddVector(_vector, Color.green, VECTOR_IDENTIFIER);
        Vector3Debugger.AddVector(_vector, Color.magenta, ANOTHER_VECTOR_IDENTIFIER);
        Vector3Debugger.AddVector(sumOfVectors, Color.white, RESULT_IDENTIFIER);
        Vector3Debugger.EnableEditorView();
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
