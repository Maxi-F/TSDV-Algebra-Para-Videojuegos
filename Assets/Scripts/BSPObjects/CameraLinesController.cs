using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

namespace BSPObjects
{
    public class CameraLinesController : MonoBehaviour
    {
        [SerializeField] private float amplitude = 60f;
        [SerializeField] private int linesQuantity = 5;
        [SerializeField] private float lineDistance = 10.0f;
        [SerializeField] private float pointsDistance = 2.5f;
        
        private float _amplitudeDistance;
        private List<Line> _lines = new List<Line>(); 
        
        // Start is called before the first frame update
        void Start()
        {
            SetLines();
        }

        // Update is called once per frame
        void Update()
        {
            SetLines();
        }

        private void SetLines()
        {
            // -1 because we want to set a distance for quantity - 1 angles.
            _lines = new List<Line>();
            _amplitudeDistance = amplitude / (linesQuantity - 1);
            float angleToUse = (180 - amplitude) / 2; 
            
            for (int i = 0; i < linesQuantity; i++)
            {
                float xDistance = Mathf.Cos(angleToUse * Mathf.Deg2Rad) * lineDistance;
                float yDistance = Mathf.Sin(angleToUse * Mathf.Deg2Rad) * lineDistance;

                Vector3 toPoint = transform.position + xDistance * transform.right + yDistance * transform.forward;

                _lines.Add(new Line(new Vec3(transform.position), new Vec3(toPoint), (int)(lineDistance / pointsDistance)));
                
                angleToUse += _amplitudeDistance;
            }
        }
        
        private void OnDrawGizmos()
        {
            foreach (var line in _lines)
            {
                line.DrawLine();
            }
        }
    }
}
