using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    string type = "noType";
    (int, int) coords = (0, 0);
    bool traversable = true;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize(string initType, (int, int) initCoords) {
        type = initType;
        coords = initCoords;
    }

    public List<Room> AdjRooms((int, int) coords) {
        List<Room> temp = new List<Room>();
        temp.Add(gameManager.roomFromCoords(coords.Item1 + 1, coords.Item2));
        temp.Add(gameManager.roomFromCoords(coords.Item1 - 1, coords.Item2));
        temp.Add(gameManager.roomFromCoords(coords.Item1, coords.Item2 + 1));
        temp.Add(gameManager.roomFromCoords(coords.Item1, coords.Item2 - 1));
        return temp;
    }

    public float Distance(Room a, Room b) {
        return Mathf.Abs(a.coords.Item1 - b.coords.Item1) + Mathf.Abs(a.coords.Item2 - b.coords.Item2);
    }

    /*List<Room> PathToRoom(Room endRoom, Room startRoom) {
        //keep track of node list of all explored nodes
        //each node has a distance to start, distance to end, and the sum of both
        //after exploring each node, add its adj nodes to node list and modify data on all nodes
        //g-cost = dist from start
        //h-cost = dist from end
        //f-cost = g + h
        //A* pathfinding algo
        List<Room> open = new List<Room>();
        List<Room> closed = new List<Room>();
        List<List<(int, int, Room)>> ghfpData = new List<List<Room>>();
        for(int i = 0; i < gameManager.cityMaxWidth; i++) {
            ghfpData.Add(new List<(int, int, Room)>());
            for(int j = 0; j < gameManager.cityMaxHeight; j++) {
                ghfpData[i].Add((Distance(gameManager.roomFromCoords(i, j), startRoom), Distance(gameManager.roomFromCoords(i, j), endRoom), Distance(gameManager.roomFromCoords(i, j), startRoom) + Distance(gameManager.roomFromCoords(i, j), endRoom), gameManager.roomFromCoords(i, j)));
            }
        }
        open.Add(gameManager.roomFromCoords(startRoom.coords.Item1, startRoom.coords.Item2));
        Room curr = unvisted[0]

        while(open.Count > 0) {
            foreach(Room room in open) {
                if (ghfpData[room.coords].Item3 < ghfpData[curr.coords].Item3) {
                    curr = room;
                }
            }
            open.Remove(curr);
            closed.Add(curr);
            if (curr == endRoom) {
                //return path
                List<Room> path = new List<Room>();
                while(curr != startRoom) {
                    path.Add(curr);
                    curr = ghfpData[curr.coords].Item4;
                }
                path.Reverse();
                return path;
            }

            foreach(Room adj in AdjRooms(curr.coords)) {
                if (adj.traversable && !closed.Contains(adj)) {
                    newDistToAdj = ghfpData[curr.coords].Item1 + Distance(curr, adj);
                    if (newDistToAdj < ghfpData[adj.coords].Item1 || !open.Contains(adj)) {
                        // if new path to adj is better than recorded path
                        ghfpData[adj.coords].Item1 = newDistToAdj;
                        ghfpData[adj.coords].Item2 = Distance(adj, endRoom);
                        ghfpData[adj.coords].Item4 = curr;
                        if(!open.Contains(adj)) {
                            open.Add(adj);
                        }
                    }
                }
            }
        }
    }*/
}
