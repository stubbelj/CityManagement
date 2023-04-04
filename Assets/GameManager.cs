using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    [SerializeField]
    GameObject roomPrefab;
    [SerializeField]
    GameObject roomsContainer;

    System.Random r;
    public List<List<Room>> roomGraph = new List<List<Room>>();

    public int cityMaxWidth = 24;
    public int cityMaxHeight = 24;
    /*
    public struct Node () {
        public List<Node*> adj;
        public (int, int) coords;
        public Room room;
        public Node((int, int) initCoords) {
            adj = new List<Node*>();
            coords = initCoords
        }
    }
    Node* rootNode;
    */

    void Awake() {
        if (inst == null) {
            inst = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for(int i = 0; i < cityMaxWidth; i++) {
            roomGraph.Add(new List<Room>());
            for(int j = 0; j < cityMaxHeight; j++) {
                roomGraph[i].Add(null);
            }
        }

        AddRoom("Cozy_Room", (0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddRoom(string initType, (int, int) initCoords) {
        if (roomGraph[initCoords.Item1][initCoords.Item2] != null) {
            print("Error adding room in occupied location in GameManager.AddRoom()");
            return;
        }
        GameObject newRoom = GameObject.Instantiate(roomPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        roomGraph[initCoords.Item1 + cityMaxWidth / 2][initCoords.Item2 + cityMaxHeight / 2] = newRoom.GetComponent<Room>();
        //this is absolutely not correct, offset needs to be somewhere else
        Instantiate(roomPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public Room RoomFromCoords((int, int) roomCoords) {
        if (roomCoords.Item1 < 0 || roomCoords.Item2 < 0 || roomCoords.Item1 >= cityMaxWidth || roomCoords.Item2 >= cityMaxHeight) {
            //out of bounds of city
            return null;
        }
        Room room = roomGraph[roomCoords.Item1][roomCoords.Item2];
        if (room == null) {
            return null;
        } else {
            return room;
        }
    }

    public (int, int) CoordsFromRoom(Room room) {
        if (room == null) {
            return (-1, -1);
        }
        return (room.coords.Item1, room.coords.Item2);
    }

}
