using System;
using System.Collections;
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
        
        [SerializeField] private UInt16 maxBinarySearchTries = 5;
        
        private float _amplitudeDistance;
        private List<Line> _lines = new List<Line>();
        private Room _currentRoom;
        
        void Start()
        {
            SetLines();
        }
        
        void Update()
        {
            StartCoroutine(LateSetLines());
        }

        private IEnumerator LateSetLines()
        {
            yield return new WaitForEndOfFrame();
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
            DoWithLines((newPosition, newEndPosition, i) =>
            {
                _lines.Add(new Line(newPosition, newEndPosition, _currentRoom, maxBinarySearchTries));
            });
        }

        private void DoWithLines(Action<Vec3, Vec3, int> action)
        {
            float aspectRatio = screenWidth / screenHeight;
            _verticalFieldOfViewAngle = fieldOfViewAngle / aspectRatio;

            int linesInAngleQuantityWidth = (int) (fieldOfViewAngle / amplitudeBetweenLines);
            int linesInAngleQuantityHeight = (int) (_verticalFieldOfViewAngle / amplitudeBetweenLines);
            
            float angleToUseInWidth = 90 - fieldOfViewAngle / 2;
            
            // +1 because we also count the line that is in angle 0 on X
            for (int i = 0; i < linesInAngleQuantityWidth + 1; i++)
            {
                float xDistance = Mathf.Cos(angleToUseInWidth * Mathf.Deg2Rad) * lineDistance;
                float zDistance = Mathf.Sin(angleToUseInWidth * Mathf.Deg2Rad) * lineDistance;

                float angleToUseInHeight = - _verticalFieldOfViewAngle / 2;
                
                // Same as horizontal lines, +1 because we also count the line that is in angle 0
                for (int j = 0; j < linesInAngleQuantityHeight + 1; j++)
                {
                    float yDistance = Mathf.Sin(angleToUseInHeight * Mathf.Deg2Rad) * lineDistance;
                    
                    Vector3 toPoint = transform.position + 
                                      xDistance * transform.right +
                                      yDistance * transform.up +
                                      zDistance * transform.forward;
                    
                    
                    action(new Vec3(transform.position), new Vec3(toPoint), i);

                    angleToUseInHeight += amplitudeBetweenLines;
                }
                
                angleToUseInWidth += amplitudeBetweenLines;
            }
        }
        
        public void DrawLines()
        {
            foreach (var line in _lines)
            {
                line.DrawLine();
            }
        }

        public void SetCurrentRoom(Room currentRoom)
        {
            _currentRoom = currentRoom;
        }
    }
}
