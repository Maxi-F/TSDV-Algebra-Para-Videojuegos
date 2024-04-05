using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathDebbuger;
using CustomMath;
public class Tester : MonoBehaviour
{
    void Start()
    {
        List<Vector3> vectors = new List<Vector3>();
        vectors.Add(new Vec3(10.0f, 0.0f, 0.0f));
        vectors.Add(new Vec3(10.0f, 10.0f, 0.0f));
        vectors.Add(new Vec3(20.0f, 10.0f, 0.0f));
        vectors.Add(new Vec3(20.0f, 20.0f, 0.0f));
        Vector3Debugger.AddVectorsSecuence(vectors, false, Color.red, "secuencia");
        Vector3Debugger.EnableEditorView("secuencia");
        Vec3 azul = new Vec3(10, 10, 0);
        Vec3 verde = new Vec3(0, -7, 0);

        Vector3Debugger.AddVector(new Vec3(10, 10, 0), Color.blue, "elAzul");
        Vector3Debugger.EnableEditorView("elAzul");
        Vector3Debugger.AddVector(new Vec3(0, -7, 0), Color.green, "elVerde");
        Vector3Debugger.EnableEditorView("elVerde");

        Vector3Debugger.AddVector(Vec3.Cross(azul, verde), Color.black, "elNegro");
        Vector3Debugger.EnableEditorView("elNegro");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Vector3Debugger.TurnOffVector("elAzul");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3Debugger.TurnOnVector("elAzul");
        }
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
