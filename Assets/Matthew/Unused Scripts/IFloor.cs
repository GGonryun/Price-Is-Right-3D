using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloor
{
    Dictionary<Vector3, GameObject> Generate(FloorBounds bounds);
    void Initialize(GameObject prefab);// initialization function 
}
