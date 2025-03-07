using System;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

namespace BSPObjects
{
    public class Line
    {
        private Vec3 _start;
        private Vec3 _end;
        private readonly int _maxTries;
    
        public Line(Vec3 start, Vec3 end, int maxBinarySearchTries)
        {
            SetValues(start, end);
            _maxTries = maxBinarySearchTries;
        }   //asd
        public void SetValues(Vec3 start, Vec3 end)
        {
            _start = start;
            _end = end;
        }

        public void UpdateValues(Vec3 start, Vec3 end)
        {
            _start = start;
            _end = end;
        }

        public Room[] GetRoomsInLine(Room currentRoom, Room[] rooms, int startIndex = 0)
        {
            List<Room> roomsInLine = new List<Room>();
            
            /*
             * TODO it should not iterate by quantity of points to analyze.
             * What should happen here is:
             *  1. Get the max longitude of the line. It should be between where it collides with a wall without overture
             *     and it's max longitude.
             *  2. Get the point that is at the half of the segment, and get the room that is there.
             *      a. If the room has a direct connection on the tree, then the room can be seen by the player. It then
             *         does another binary search at half the length of the segment but on the latter end.
             *      b. If the room is the same as the player, it does another binary search at half the length of the
             *         segment but on the latter end.
             *      c. If the room does not have a direct connection on the tree, then it does a binary search but on the
             *         first half of the segment.
             *
             * THIS SHOULD BE FIXED BECAUSE IT IS DOING A SEQUENTIAL SEARCH AND IT'S WRONG.
             */
            
            
            /*
            for(int i = startIndex; i < _points.Count; i++)
            {
                if(currentRoom.IsPointInsideRoom(_points[i])) continue;
               
                foreach (var room in rooms)
                {
                    if (room.IsPointInsideRoom(_points[i]))
                    {
                        if (!roomsInLine.Exists(roomAlreadyInLine => roomAlreadyInLine == room))
                        {
                            Wall intersectingWall = room.GetIntersectedWall(i - 1 == -1 ? _points[0] : _points[i - 1], _points[i]);
                            if (!intersectingWall ||
                                (!intersectingWall.IntersectsWithOverture(i - 1 == -1 ? _points[0] : _points[i - 1]) && 
                                 !intersectingWall.IntersectsWithOverture(_points[i]))) return roomsInLine.ToArray();
                        }
                        
                        if (currentRoom.IsRoomAdjacent(room) || roomsInLine.Exists(adjacentRoom => adjacentRoom.IsRoomAdjacent(room)))
                        {
                            roomsInLine.Add(room);

                            Room[] adjacentRoomsToSeenOne = GetRoomsInLine(room, rooms, i);
                            roomsInLine.AddRange(adjacentRoomsToSeenOne);
                        }
                        else
                        {
                            if (i == 0) continue;
                            roomsInLine.AddRange(GetRoomsConnection(
                                currentRoom,
                                room,
                                rooms,
                                new []{_points[i - 1], _points[i]},
                                this._maxTries
                                ));
                        }
                    }
                }
            }

            return roomsInLine.ToArray();
            */
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

        public void DrawLine()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(_start, _end);
            DrawPoints();
        }
        
        private void DrawPoints()
        {
            /*
            foreach (var point in _points)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(point.toVector3(), 0.2f);
            }
            */
        }
    }
}
