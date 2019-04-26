﻿using System.Collections.Generic;
using UnityEngine;

public class FloorDestroyer : MonoBehaviour
{
    private IFloorDestroyer destroyerStategy;

    public void SetGenerationStrategy(IFloorDestroyer strategy)
    {
        this.destroyerStategy = strategy;
    }

    public void DestroyFloor(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor)
    {
        destroyerStategy.Destroy(numOfCubesInXDir, numOfCubesInZDir, floor);
    }
}