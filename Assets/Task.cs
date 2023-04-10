using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public string type;
    public List<IEnumerator> steps = new List<IEnumerator>();
    public List<List<Room>> paths = new List<List<Room>>();
}
