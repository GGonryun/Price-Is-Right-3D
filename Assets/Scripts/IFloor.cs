using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloor
{
    Dictionary<Vector3, GameObject> generate(int numOfCubesInXDir, int numOfCubesInZDir);
}
