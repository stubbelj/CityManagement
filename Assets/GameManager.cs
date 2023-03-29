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

    private const int cityMaxWidth = 24;
    private const int cityMaxHeight = 24;
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
        Instantiate(roomPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    Room RoomFromCoords((int, int) roomCoords) {
        return roomGraph[roomCoords.Item1][roomCoords.Item2];
    }

}
