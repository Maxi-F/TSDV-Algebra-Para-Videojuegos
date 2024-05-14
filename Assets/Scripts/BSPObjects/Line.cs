using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

namespace BSPObjects
{
    public class Line
    {
        private List<Vec3> _points;
        private Vec3 _start;
        private Vec3 _end;
        private int maxTries;
    
        public Line(Vec3 start, Vec3 end, int pointsQuantity, int maxBinarySearchTries)
        {
            SetValues(start, end, pointsQuantity);
            maxTries = maxBinarySearchTries;
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
               
                foreach (var room in rooms)
                {
                    if (room.IsPointInsideRoom(_points[i]))
                    {
                        if (currentRoom.IsRoomAdjacent(room) || roomsInLine.Exists(adjacentRoom => adjacentRoom.IsRoomAdjacent(room)))
                        {
                            roomsInLine.Add(room);

                            Room[] adjacentRoomsToSeenOne = GetRoomsInLine(room, rooms, i);
                            roomsInLine.AddRange(adjacentRoomsToSeenOne);
                        }
                        else
                        {
                            if (i == 0) continue;
                            Debug.Log($"Calculating recursive with point ${i}");
                            roomsInLine.AddRange(GetRoomsConnection(
                                currentRoom,
                                room,
                                rooms,
                                new []{_points[i - 1], _points[i]},
                                this.maxTries
                                ));
                        }
                    }
                }
            }

            return roomsInLine.ToArray();
        }
        
        private Room[] GetRoomsConnection(
            Room initialRoom,
            Room endRoom, 
            Room[] rooms,
            Vec3[] pointsToCheckInBetween,
            int maxBinarySearchTries
            )
        {
            List<Room> connectedRooms = new List<Room>();
            if (maxBinarySearchTries <= 0) return connectedRooms.ToArray();
            
            Vec3 pointInBetween = Vec3.Lerp(pointsToCheckInBetween[0], pointsToCheckInBetween[1], 0.5f);

            if (initialRoom.IsPointInsideRoom(pointInBetween))
            {
                return GetRoomsConnection(
                    initialRoom,
                    endRoom,
                    rooms,
                    new[] { pointInBetween, pointsToCheckInBetween[1] },
                    maxBinarySearchTries - 1)
                    ;
            } else if (endRoom.IsPointInsideRoom(pointInBetween))
            {
                return GetRoomsConnection(
                    initialRoom,
                    endRoom,
                    rooms,
                    new[] { pointsToCheckInBetween[1], pointInBetween },
                    maxBinarySearchTries - 1
                    );
            }
            
            foreach (var room in rooms)
            {
                if (room.IsPointInsideRoom(pointInBetween))
                {
                    if (initialRoom.IsRoomAdjacent(room) && room.IsRoomAdjacent(endRoom))
                    {
                        Room[] roomsToAdd = new[] { initialRoom, room, endRoom };
                        connectedRooms.AddRange(roomsToAdd);
                    }
                }
            }
            
            return connectedRooms.ToArray();
        }

        private void DoWithPoints(int pointsQuantity, Action<Vec3, int> action)
        {
            float distance = Vec3.Distance(_start, _end);
            float inBetweenPercentage = 1.0f / pointsQuantity;
        
            for (int i = 0; i <= pointsQuantity; i++)
            {
                Vec3 place = Vec3.Lerp(_start, _end, inBetweenPercentage * i);

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
