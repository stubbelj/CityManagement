using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meeple : MonoBehaviour
{
    private GameManager gameManager;

    public List<IEnumerator> currentTask = new List<IEnumerator>();
    public IEnumerator currentTaskStep;
    public List<List<IEnumerator>> taskQueue = new List<List<IEnumerator>>();

    void Start()
    {
        gameManager = GameManager.inst;
    }

    void Update()
    {
        if(currentTask == null && taskQueue.Count > 0) {
            //if there is no current task and there are tasks to perform
            //currentTask becomes null at the end of the final step of a task
            currentTask = taskQueue[0];
            taskQueue.RemoveAt(0);
        } else {
            if (currentTaskStep == null && currentTask.Count > 0) {
                //if there is no current task step and there are task steps to perform
                //currentTaskStep becomes null at the end of the final step of a task 
                currentTaskStep = currentTask[0];
                currentTask.RemoveAt(0);
                StartCoroutine(currentTaskStep);
            }
        }

    }

    public List<Room> PathToRoom(Room startRoom, Room endRoom) {
        //returns the shortest Meeple-traversable path from startRoom to endRoom, taking vert/hori traversability into account
        //
        //keep track of node list of all explored nodes
        //each node has a distance to start, distance to end, and the sum of both
        //after exploring each node, add its adj nodes to node list and modify data on all nodes
        //g-cost = dist from start
        //h-cost = dist from end
        //f-cost = g + h
        List<Room> path = new List<Room>();
        List<Room> open = new List<Room>();
        List<Room> closed = new List<Room>();
        List<List<(int, int, int, Room)>> ghfpData = new List<List<(int, int, int, Room)>>();
        for(int i = 0; i < gameManager.cityMaxWidth; i++) {
            ghfpData.Add(new List<(int, int, int, Room)>());
            for(int j = 0; j < gameManager.cityMaxHeight; j++) {
                Room ijRoom = gameManager.RoomFromCoords((i, j));
                if (ijRoom == null) {
                    ghfpData[i].Add((0, 0, 0, null));
                } else {
                    ghfpData[i].Add((ijRoom.Distance(ijRoom, startRoom), ijRoom.Distance(ijRoom, endRoom), ijRoom.Distance(ijRoom, startRoom) + ijRoom.Distance(ijRoom, endRoom), ijRoom));
                }
            }
        }
        open.Add(gameManager.RoomFromCoords((startRoom.coords.Item1, startRoom.coords.Item2)));
        Room curr = open[0];

        while(open.Count > 0) {
            curr = open[0];
            foreach(Room room in open) {
                if (ghfpData[room.coords.Item1][room.coords.Item2].Item3 < ghfpData[curr.coords.Item1][curr.coords.Item2].Item3) {
                    curr = room;
                }
            }
            open.Remove(curr);
            closed.Add(curr);
            if (curr == endRoom) {
                //record path
                while(curr != startRoom) {
                    path.Add(curr);
                    curr = ghfpData[curr.coords.Item1][curr.coords.Item2].Item4;
                }
                path.Reverse();
                return path;
            }

            foreach(Room adj in curr.AdjRoomsTraversable(curr.coords)) {
                if (!closed.Contains(adj)) {
                    int newDistToAdj = ghfpData[curr.coords.Item1][curr.coords.Item2].Item1 + curr.Distance(curr, adj);
                    if (newDistToAdj < ghfpData[adj.coords.Item1][adj.coords.Item2].Item1 || !open.Contains(adj)) {
                        // if new path to adj is better than recorded path, update g, and (consequently) f costs
                        (int, int, int, Room) adjTuple = ghfpData[adj.coords.Item1][adj.coords.Item2];
                        ghfpData[adj.coords.Item1][adj.coords.Item2] = (newDistToAdj, adjTuple.Item2, newDistToAdj + adjTuple.Item2, curr);
                        if(!open.Contains(adj)) {
                            open.Add(adj);
                        }
                    }
                }
            }
        }
        return null;
    }

    public List<IEnumerator> PathToRoomTasks(Room startRoom, Room endRoom) {
        //returns list of task steps of the form [TaskWalkPath, TaskLadderPath, TaskWalkPath...] that constitute moving along the path from endRoom to startRoom
        List<Room> path = PathToRoom(endRoom, startRoom);
        //Path has been made, time to split it up into task steps (walking, climbing)
        Room prev = path[0];
        List<Room> tempPath = new List<Room>();
        List<IEnumerator> tempTask = new List<IEnumerator>();
        bool verticalPath = false;
        foreach (Room room in path) {
            if (room.coords.Item2 != prev.coords.Item2) {
                if (!verticalPath) {
                    tempTask.Add(TaskWalkPath(tempPath));
                } else {
                    tempTask.Add(TaskLadderPath(tempPath));
                }
                tempPath.Clear();
                verticalPath = !verticalPath;
            }
            tempPath.Add(room);
            prev = room;
        }
        // then empty out last task step
        if (!verticalPath) {
            tempTask.Add(TaskWalkPath(tempPath));
        } else {
            tempTask.Add(TaskLadderPath(tempPath));
        }
        return tempTask;
    }

    public void WalkToRoom(Room startRoom, Room endRoom) {
        taskQueue.Add(PathToRoomTasks(startRoom, endRoom));
    }

    public IEnumerator TaskWalkPath(List<Room> path) {
        //makes the Meeple walk along a given horizontal path
        print("walkin");
        foreach(Room room in path) {
            transform.position = room.gameObject.transform.position;
        }
        currentTaskStep = null;
        yield return null;
    }

    public IEnumerator TaskLadderPath(List<Room> path) {
        //makes the Meeple ladder up a given vertical path
        currentTaskStep = null;
        yield return null;
    }

}
