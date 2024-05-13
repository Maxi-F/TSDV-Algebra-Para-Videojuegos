using System;
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
        
        // Start is called before the first frame update
        void Start()
        {
            SetAmplitudeDistance();
        }

        // Update is called once per frame
        void Update()
        {
            SetAmplitudeDistance();
        }

        private void SetAmplitudeDistance()
        {
            // -1 because we want to set a distance for quantity - 1 angles.
            _amplitudeDistance = amplitude / (linesQuantity - 1);
        }
        
        private void OnDrawGizmos()
        {
            Vector3 inPoint = transform.position;
            float angleToUse = (180 - amplitude) / 2; 
            
            if (Application.isPlaying)
            {
                for (int i = 0; i < linesQuantity; i++)
                {
                    float xDistance = Mathf.Cos(angleToUse * Mathf.Deg2Rad) * lineDistance;
                    float yDistance = Mathf.Sin(angleToUse * Mathf.Deg2Rad) * lineDistance;
                
                    Vector3 toPoint = transform.position + xDistance * transform.right + yDistance * transform.forward;

                    Gizmos.color = Color.red;

                    Debug.Log($"{angleToUse}, {_amplitudeDistance}, {amplitude}");

                    angleToUse += _amplitudeDistance;
                
                    Gizmos.DrawLine(inPoint, toPoint);
                }
            }
        }
    }
}
