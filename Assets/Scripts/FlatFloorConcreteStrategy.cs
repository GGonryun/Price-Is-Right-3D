﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatFloorConcreteStrategy : MonoBehaviour, IFloor
{
    private GameObject cubePrefab;

    void Awake()
    {
        //cubePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Resources.Load<GameObject>("/Assets/Resources/Prefabs/Cube") as GameObject;
    }
    public Dictionary<Vector3, GameObject> Generate(int numOfCubesInXDir, int numOfCubesInZDir)
    {
        return InstanciateCubes(numOfCubesInXDir, numOfCubesInZDir);
    }

    private Dictionary<Vector3, GameObject> InstanciateCubes(int numOfCubesInXDir, int numOfCubesInZDir)
    {
        Dictionary<Vector3, GameObject> floorOfCubes = new Dictionary<Vector3, GameObject>();

        // Instanciate Cube GameObjects and assign them to the floorOfCubes Dictionary
        for (int currXIndex = 0; currXIndex < numOfCubesInXDir; currXIndex++)
        {
            for (int currZIndex = 0; currZIndex < numOfCubesInZDir; currZIndex++)
            {
               Vector3 tempVector3 = CreateVector3(currXIndex, currZIndex);
               GameObject cube = CreateGameObject(tempVector3);
               Debug.Log("Created cube at " + tempVector3);
               floorOfCubes[tempVector3] = cube;
            }
        }
        return floorOfCubes;
    }

    private Vector3 CreateVector3(int xCoord, int zCoord)
    {
        return new Vector3(xCoord, 0, zCoord);
    }

    private GameObject CreateGameObject(Vector3 spawnPosition)
    {
        GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity) as GameObject;
        return cube;
    }

    public void Initialize(GameObject prefab)
    {
        this.cubePrefab = prefab;
    }
}
