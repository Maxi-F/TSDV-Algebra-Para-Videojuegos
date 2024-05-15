using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

namespace BSPObjects
{
    public class CameraLinesController : MonoBehaviour
    {
        [SerializeField] private float screenWidth = 1920f;
        [SerializeField] private float screenHeight = 1080f;
        
        [SerializeField] private float fieldOfViewAngle = 90f;
        private float _verticalFieldOfViewAngle;
        
        [SerializeField] private float amplitudeBetweenLines = 5.0f;
        
        [SerializeField] private float lineDistance = 10.0f;
        
        [SerializeField] private float pointsDistance = 2.5f;
        
        [SerializeField] private UInt16 maxBinarySearchTries = 5;
        
        private float _amplitudeDistance;
        private List<Line> _lines = new List<Line>(); 
        
        void Start()
        {
            SetLines();
        }
        
        void Update()
        {
            SetLines();
        }

        public Room[] GetSeenRooms(Room currentRoom, Room[] rooms)
        {
            List<Room> activeRooms = new List<Room>();
            foreach (var line in _lines)
            {
                Room[] activeRoomsInLine = line.GetRoomsInLine(currentRoom, rooms);
                if(activeRoomsInLine == null) continue;
                activeRooms.AddRange(activeRoomsInLine);
            }

            return activeRooms.ToArray();
        }

        private void OnValidate()
        {
            SetLines();
        }

        private void SetLines()
        {
            _lines = new List<Line>();
            DoWithLines((newPosition, newEndPosition, pointsQuantity, i) =>
            {
                _lines.Add(new Line(newPosition, newEndPosition, pointsQuantity, maxBinarySearchTries));
            });
        }

        private void UpdateLines()
        {
            DoWithLines((newPosition, newEndPosition, pointsQuantity, i) =>
            {
                _lines[i].UpdateValues(newPosition, newEndPosition, pointsQuantity);
            });
        }

        private void DoWithLines(Action<Vec3, Vec3, int, int> action)
        {
            float aspectRatio = screenWidth / screenHeight;
            _verticalFieldOfViewAngle = fieldOfViewAngle / aspectRatio;

            int linesQuantityWidth = (int) (fieldOfViewAngle / amplitudeBetweenLines);
            int linesQuantityHeight = (int) (_verticalFieldOfViewAngle / amplitudeBetweenLines);
            
            float angleToUseInWidth = 90 - fieldOfViewAngle / 2;
            
            for (int i = 0; i < linesQuantityWidth; i++)
            {
                float xDistance = Mathf.Cos(angleToUseInWidth * Mathf.Deg2Rad) * lineDistance;
                float zDistance = Mathf.Sin(angleToUseInWidth * Mathf.Deg2Rad) * lineDistance;

                float angleToUseInHeight = - _verticalFieldOfViewAngle / 2;
                for (int j = 0; j < linesQuantityHeight; j++)
                {
                    float yDistance = Mathf.Sin(angleToUseInHeight * Mathf.Deg2Rad) * lineDistance;
                    
                    Vector3 toPoint = transform.position + 
                                      xDistance * transform.right +
                                      yDistance * transform.up +
                                      zDistance * transform.forward;
                    
                    
                    action(new Vec3(transform.position), new Vec3(toPoint), (int)(lineDistance / pointsDistance), i);

                    angleToUseInHeight += amplitudeBetweenLines;
                }
                
                angleToUseInWidth += amplitudeBetweenLines;
            }
        }
        
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                foreach (var line in _lines)
                {
                    line.DrawLine();
                }
            }
        }
    }
}
