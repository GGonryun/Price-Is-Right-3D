using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollection : MonoBehaviour
{
    GameObject cube;

    void Awake()
    {
        cube = gameObject;
    }

    void SetNotActive()
    {
        cube.SetActive(false);
    }

    void SetActive()
    {
        cube.SetActive(true);
    }
}
