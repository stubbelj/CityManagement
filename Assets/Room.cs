using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    bool verticalTraversal = false;
    [SerializeField]
    bool horizontalTraversal = true;
    public int width = 2;
    public int height = 1;

    public float floatWidth = 80f;
    public float floatHeight = 48f;
    public float baseFloatWidth = 40f;
    public float baseFloatHeight = 48f;

    string type = "noType";
    public (int, int) coords = (-1, -1);

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
        // returns up to 4 horizontally and vertically adjacent rooms
        List<Room> temp = new List<Room>();
        foreach ((int, int) roomCoords in gameManager.AllCoordsFromRoom(this)) {
            foreach ((int, int) pair in new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1) }) {
                Room room = gameManager.RoomFromCoords((roomCoords.Item1 + pair.Item1, roomCoords.Item2 + pair.Item2));
                if (room != null) {
                    temp.Add(room);
                }
            }
        }
        return temp;
    }

    public List<Room> AdjRoomsTraversable((int, int) coords) {
        // returns up to 4 horizontally and vertically adjacent rooms
        List<Room> temp = new List<Room>();
        Room room = gameManager.RoomFromCoords(coords);
        if (room.horizontalTraversal) {
            foreach ((int, int) roomCoords in gameManager.AllCoordsFromRoom(this)) {
                foreach ((int, int) pair in new (int, int)[] { (-1, 0), (1, 0) }) {
                    Room adjRoom = gameManager.RoomFromCoords((roomCoords.Item1 + pair.Item1, roomCoords.Item2 + pair.Item2));
                    if (adjRoom != null && adjRoom.horizontalTraversal) {
                        temp.Add(adjRoom);
                    }
                }
            }
        }
        if (room.verticalTraversal) {
            foreach ((int, int) roomCoords in gameManager.AllCoordsFromRoom(this)) {
                foreach ((int, int) pair in new (int, int)[] { (0, -1), (0, 1) }) {
                    Room adjRoom = gameManager.RoomFromCoords((roomCoords.Item1 + pair.Item1, roomCoords.Item2 + pair.Item2));
                    if (adjRoom != null && adjRoom.verticalTraversal) {
                        temp.Add(adjRoom);
                    }
                }
            }
        }
        return temp;
    }

    public int Distance(Room a, Room b) {
        //returns the distance between room a and b in terms of the number of strictly vertical or horizontal steps between them
        if (a == null || b == null) {
            return -1;
        }
        return Mathf.Abs(a.coords.Item1 - b.coords.Item1) + Mathf.Abs(a.coords.Item2 - b.coords.Item2);
    }

}
