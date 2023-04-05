using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst = null;

    [SerializeField]
    GameObject roomPrefab;
    [SerializeField]
    GameObject roomsContainer;

    System.Random r;
    public List<List<Room>> roomGraph = new List<List<Room>>();

    public Meeple meep;

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
        AddRoom("Cozy_Room", (1, 0));

        StartCoroutine(LateStart());

    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(1f);
        meep.WalkToRoom(roomGraph[0][0], roomGraph[1][0]);
        yield return null;
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
        GameObject newRoom = GameObject.Instantiate(roomPrefab, new Vector3(initCoords.Item1 * Room.ROOM_WIDTH, initCoords.Item2 * Room.ROOM_HEIGHT, 0), Quaternion.identity);
        roomGraph[initCoords.Item1][initCoords.Item2] = newRoom.GetComponent<Room>();
        newRoom.GetComponent<Room>().coords = (initCoords.Item1, initCoords.Item2);
        newRoom.name = initCoords.Item1.ToString() + ", " + initCoords.Item2.ToString();
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
