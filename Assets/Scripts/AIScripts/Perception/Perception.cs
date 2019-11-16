using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Perception : MonoBehaviour
{
    public Dictionary<GameObject, MemoryRecord> MemoryMap = new Dictionary<GameObject, MemoryRecord>();
    public GameObject[] sensedObject;
    public MemoryRecord[] sensedRecord;
    
    public void clearView()
    {
        foreach(KeyValuePair<GameObject, MemoryRecord> Memory in MemoryMap)
        {
            Memory.Value.withinView = false;
        }
    }

    public void addMemory(GameObject target)
    {
        MemoryRecord mRecord = new MemoryRecord(DateTime.Now, target.transform.position, true);

        if (MemoryMap.ContainsKey(target))
        {
            MemoryMap[target] = mRecord;
        }
        else
        {
            MemoryMap.Add(target, mRecord);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sensedObject = new GameObject[MemoryMap.Keys.Count];
        sensedRecord = new MemoryRecord[MemoryMap.Values.Count];
        MemoryMap.Keys.CopyTo(sensedObject, 0);
        MemoryMap.Values.CopyTo(sensedRecord, 0);
    }
}

[Serializable]
public class MemoryRecord
{
    [SerializeField]
    public DateTime lastSensed;

    [SerializeField]
    public Vector3 lastSensedPos;

    [SerializeField]
    public bool withinView;

    public MemoryRecord()
    {
        lastSensed = DateTime.MinValue;
        lastSensedPos = Vector3.zero;
        withinView = false;
    }

    public MemoryRecord(DateTime time, Vector3 position, bool fieldOfView)
    {
        lastSensed = time;
        lastSensedPos = position;
        withinView = fieldOfView;
    }
}
