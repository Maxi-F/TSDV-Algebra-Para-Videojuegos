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
            UpdateLines();
        }

        public Room[] GetSeenRooms(Room currentRoom, Room[] rooms)
        {
            List<Room> activeRooms = new List<Room>();
            foreach (var line in _lines)
            {
                Room[] activeRoomsInLine = line.GetRoomsInLine(currentRoom, rooms);
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
                _lines.Add(new Line(newPosition, newEndPosition, pointsQuantity));
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
            _amplitudeDistance = amplitude / (linesQuantity - 1);
            float angleToUse = (180 - amplitude) / 2; 
            
            for (int i = 0; i < linesQuantity; i++)
            {
                float xDistance = Mathf.Cos(angleToUse * Mathf.Deg2Rad) * lineDistance;
                float yDistance = Mathf.Sin(angleToUse * Mathf.Deg2Rad) * lineDistance;

                Vector3 toPoint = transform.position + xDistance * transform.right + yDistance * transform.forward;

                action(new Vec3(transform.position), new Vec3(toPoint), (int)(lineDistance / pointsDistance), i);
                
                angleToUse += _amplitudeDistance;
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
