using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedRemovalOfTilesStrategy : MonoBehaviour, IFloorDestroyer
{
    List<Vector3> listToDelete;
    public void Destroy(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor)
    {


    }

    public void Initialize(List<Vector3> listToDelete)
    {
        this.listToDelete = listToDelete;
    }
}
