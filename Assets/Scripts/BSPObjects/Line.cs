using System;
using System.Collections.Generic;
using System.Linq;
using CustomMath;
using UnityEngine;

namespace BSPObjects
{
    public class Line
    {
        private Vec3 _start;
        private Vec3 _end;
        private Vec3 _contact;
        private List<Vec3> _points;
        private readonly int _maxTries;

        private bool _isCollidingWithOwnRoomWall = false;
        private bool _isCollidingWithAdjacentWall = false;
        private bool _isAddingAdjacent = false;
        
        public Line(Vec3 start, Vec3 end, Room currentRoom, int maxBinarySearchTries)
        {
            SetValues(start, end, currentRoom);
            _points = new List<Vec3>();
            _maxTries = maxBinarySearchTries;
        }
        public void SetValues(Vec3 start, Vec3 end, Room currentRoom)
        {
            _start = start;
            _end = GetEndValue(start, end, currentRoom);
        }

        private Vec3 GetEndValue(Vec3 start, Vec3 end, Room currentRoom)
        {
            if(currentRoom == null) return end;
            if (currentRoom.IsPointInsideRoom(end)) return end;

            return currentRoom.GetMostNearPointFromWalls(start, end, out _isCollidingWithOwnRoomWall);
        }

        public Room[] GetRoomsInLine(Room currentRoom, Room[] rooms)
        {
            return CalculateRoomsInLine(currentRoom, rooms);
        }

        /*
         * TODO it should not iterate by quantity of points to analyze.
         * What should happen here is:
         *  1. Get the max longitude of the line. It should be between where it collides with a wall without overture
         *     and it's max longitude. -> DONE
         *  2. Get the point that is at the half of the segment, and get the room that is there.
         *      a. If the room has a direct connection on the tree, then the room can be seen by the player. It then
         *         does another binary search at half the length of the segment but on the latter end (UNLESS it is the end of the segment).
         *      b. If the room is the same as the player, it does another binary search at half the length of the
         *         segment but on the latter end.
         *      c. If the room does not have a direct connection on the tree, then it does a binary search but on the
         *         first half of the segment.
         *
         * THIS SHOULD BE FIXED BECAUSE IT IS DOING A SEQUENTIAL SEARCH AND IT'S WRONG.
         */
        private Room[] CalculateRoomsInLine(Room currentRoom, Room[] rooms)
        {
            List<Room> roomsInLine = new List<Room>();
            List<Room> nonAdjacentRooms = new List<Room>();
            if (_isCollidingWithOwnRoomWall) return roomsInLine.ToArray();
            _points = new List<Vec3>();

            Vec3 endPoint = _end;
            Vec3 startingPoint = _start;
            Vec3 pointToCalculate = _end;

            for (int retriesLeft = _maxTries; retriesLeft >= 0; retriesLeft--)
            {
                _points.Add(pointToCalculate);
                bool calculateLatter = true;
                Room roomInPoint = rooms.ToList().Find(room => room.IsPointInsideRoom(pointToCalculate));

                if (roomInPoint)
                {
                    if (currentRoom.IsRoomAdjacent(roomInPoint) && !roomsInLine.Contains(roomInPoint))
                    {
                        roomsInLine.Add(roomInPoint);
                        foreach (Room nonAdjacentRoom in nonAdjacentRooms.ToList())
                        {
                            if (roomInPoint.IsRoomAdjacent(nonAdjacentRoom))
                            {
                                if (roomInPoint.CheckWallOverturesWith(startingPoint, endPoint, nonAdjacentRoom))
                                {
                                    roomsInLine.Add(nonAdjacentRoom);
                                    nonAdjacentRooms.Remove(nonAdjacentRoom);
                                    _isAddingAdjacent = true;
                                }
                                else
                                {
                                    _isCollidingWithAdjacentWall = true;
                                    break;
                                }
                            }
                        }
                        calculateLatter = retriesLeft != _maxTries;
                    } else if (currentRoom != roomInPoint && !nonAdjacentRooms.Contains(roomInPoint))
                    {
                        calculateLatter = false;
                        nonAdjacentRooms.Add(roomInPoint);
                    }
                }
                else
                {
                    calculateLatter = false;
                }
                
                Vec3 halfPoint = (endPoint + startingPoint) / 2;
                
                if (calculateLatter)
                {
                    startingPoint = halfPoint;
                    pointToCalculate = halfPoint;
                }
                else
                {
                    endPoint = halfPoint;
                    pointToCalculate = halfPoint;
                }
            }

            return roomsInLine.ToArray();
        }

        public void DrawLine()
        {
            Gizmos.color = _isCollidingWithOwnRoomWall ? Color.red : 
                _isCollidingWithAdjacentWall ? Color.magenta : 
                _isAddingAdjacent ? Color.green :
                Color.blue;
            
            Gizmos.DrawLine(_start, _end);
            DrawPoints();
        }
        
        private void DrawPoints()
        {
            
                Gizmos.color = Color.blue;
                Debug.Log(_points != null);
                if (_points == null) return;
                Debug.Log(_points.Count);
                foreach (var point in _points)
                {
                    Gizmos.DrawSphere(point.toVector3(), 0.2f);
                }
        }
    }
}
