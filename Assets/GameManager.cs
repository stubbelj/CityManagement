using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager inst = null;

    [SerializeField]
    List<GameObject> roomPrefabs = new List<GameObject>();


    Dictionary<string, int> roomIndices = new Dictionary<string, int>{
        {"Alchemy_Room", 0 },
        {"Ladder_Room", 1 }
    };

    System.Random r;
    public List<List<Room>> roomGraph = new List<List<Room>>();

    public Meeple meep;

    public int cityMaxWidth = 24;
    public int cityMaxHeight = 24;

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

        AddRoom("Alchemy_Room", (0, 0)); //
        AddRoom("Alchemy_Room", (2, 0)); //
        AddRoom("Alchemy_Room", (2, 1)); // checks for room (1, 1) which dne
        AddRoom("Alchemy_Room", (2, 2)); 
        AddRoom("Alchemy_Room", (0, 2));
        AddRoom("Ladder_Room", (4, 0));
        AddRoom("Ladder_Room", (4, 1));
        AddRoom("Ladder_Room", (4, 2));

        StartCoroutine(LateStart());

    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(1f);
        meep.WalkToRoom(roomGraph[0][0], roomGraph[0][2]);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
    }


    void AddRoom(string initType, (int, int) initCoords) {
        if (roomGraph[initCoords.Item1][initCoords.Item2] != null) {
            print("Error adding room in occupied location in GameManager.AddRoom()");
            print("Error adding room at" + initCoords + "due to existing room" + roomGraph[initCoords.Item1][initCoords.Item2].coords);
            return;
        }
        Room newRoom = GameObject.Instantiate(roomPrefabs[roomIndices[initType]], new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Room>();
        Vector3 spawnCoords = new Vector3();
        if (initCoords.Item1 >= cityMaxWidth || initCoords.Item2 >= cityMaxHeight) {
            print("Error adding room in coordinates that are out of bounds in GameManager.AddRoom()");
            return;
        }
        if (initCoords.Item1 > 0 ) {
            spawnCoords.x = initCoords.Item1 * newRoom.baseFloatWidth;
        }
        if (initCoords.Item2 > 0 ) {
            spawnCoords.y = initCoords.Item2 * newRoom.baseFloatHeight;
        }
        newRoom.transform.position = spawnCoords;
        
        int size = 0;
        while (size < newRoom.width) {
            roomGraph[initCoords.Item1 + size][initCoords.Item2] = newRoom;
            size++;
        }
        size = 0;
        while (size < newRoom.height) {
            roomGraph[initCoords.Item1][initCoords.Item2 + size] = newRoom;
            size++;
        }
        newRoom.coords = (initCoords.Item1, initCoords.Item2);
        newRoom.gameObject.name = "Room " + initCoords.Item1 + ", " + initCoords.Item2;
        
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

    public List<(int, int)> AllCoordsFromRoom(Room room) {
        List<(int, int)> temp = new List<(int, int)>();
        int i = 0;
        while (i < room.width) {
            temp.Add((room.coords.Item1 + i, room.coords.Item2));
            i++;
        }
        i = 0;
        while (i < room.height) {
            temp.Add((room.coords.Item1, room.coords.Item2 + i));
            i++;
        }
        return temp;
    }

}
