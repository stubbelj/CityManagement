using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meeple : MonoBehaviour
{
    private GameManager gameManager;

    public class Task {
        public string name;
        public Task() {
            name = "taskNotAssigned";
        }
    }

    public Task task = new Task();
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.inst;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void assignTask(string taskName) {
        task.name = taskName;
    }
}
