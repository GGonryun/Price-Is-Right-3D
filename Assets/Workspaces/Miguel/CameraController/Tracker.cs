using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private Transform[] trackableObjects;

    private List<Transform> trackedObjects;

    public List<Transform> TrackedObjects
    {
        get
        {
            if (trackedObjects == null)
            {
                trackedObjects = new List<Transform>();
            }
            return trackedObjects;
        }
    }

    protected virtual void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (Transform obj in trackableObjects)
        {
            TrackedObjects.Add(obj);
        }
    }

    public void Add(Transform transform)
    {
        TrackedObjects.Add(transform);
    }

    public Transform Remove(int index)
    {
        Transform obj = TrackedObjects[index];
        TrackedObjects.RemoveAt(index);
        return obj;
    }

    public Transform Remove(Transform transform)
    {
        int index = LocateIndex(transform);
        if(index > -1)
        {
            return Remove(index);
        }
        return null;
    }

    public int LocateIndex(Transform transform)
    {
        return TrackedObjects.FindIndex(item => item == transform);
    }
}
