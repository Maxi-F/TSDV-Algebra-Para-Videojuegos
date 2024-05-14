using System;
using System.Collections.Generic;
using CustomMath;
using Unity.VisualScripting;
using UnityEngine;

namespace BSPObjects
{
    public class Line
    {
        private List<Vec3> _points;
        private Vec3 _start;
        private Vec3 _end;
    
        public Line(Vec3 start, Vec3 end, int pointsQuantity)
        {
            SetValues(start, end, pointsQuantity);
        }

        public void SetValues(Vec3 start, Vec3 end, int pointsQuantity)
        {
            _start = start;
            _end = end;

            SetPoints(pointsQuantity);
        }

        public void UpdateValues(Vec3 start, Vec3 end, int pointsQuantity)
        {
            _start = start;
            _end = end;
            
            DoWithPoints(pointsQuantity, (newPlace, i) => {_points[i] = new Vec3(newPlace);});
        }

        private void SetPoints(int pointsQuantity)
        {
            _points = new List<Vec3>();
            
            DoWithPoints(pointsQuantity, (newPlace, i) => {  _points.Add(new Vec3(newPlace)); });
        }

        public Room[] GetRoomsInLine(Room currentRoom, Room[] rooms, int startIndex = 0)
        {
            List<Room> roomsInLine = new List<Room>();

            for(int i = startIndex; i < _points.Count; i++)
            {
                if(currentRoom.IsPointInsideRoom(_points[i])) continue;
                // Debug.Log($"Found point not in current room!");
                foreach (var room in rooms)
                {
                    if (room.IsPointInsideRoom(_points[i]))
                    {
                        if (currentRoom.IsRoomAdjacent(room))
                        {
                            roomsInLine.Add(room);

                            Room[] adjacentRoomsToSeenOne = GetRoomsInLine(room, rooms, i);
                            roomsInLine.AddRange(adjacentRoomsToSeenOne);
                        }
                        else
                        {
                            // TODO Binary search between non adjacent rooms
                        }
                    }
                }
            }

            return roomsInLine.ToArray();
        }

        private void DoWithPoints(int pointsQuantity, Action<Vec3, int> action)
        {
            float distance = Vec3.Distance(_start, _end);
            float inBetweenPercentage = 1.0f / pointsQuantity;
        
            for (int i = 0; i < pointsQuantity; i++)
            {
                Vec3 place = Vec3.Lerp(_start, _end, inBetweenPercentage * (i + 1));

                action(place, i);
            }
        }

        public void DrawLine()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(_start, _end);
            DrawPoints();
        }
        
        private void DrawPoints()
        {
            
            foreach (var point in _points)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(point.toVector3(), 0.2f);
            }
        }
    }
}
