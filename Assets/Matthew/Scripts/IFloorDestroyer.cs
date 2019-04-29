using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloorDestroyer
{
    void Destroy(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor, List<Vector3> cubesToDestroy);
}