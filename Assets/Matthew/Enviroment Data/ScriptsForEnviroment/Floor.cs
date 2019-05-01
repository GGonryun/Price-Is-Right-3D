using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private FloorBounds bounds;
    private GameObject floorTilePrefab;
    private GameObject GameManagerReference;

    public Dictionary<Vector3, GameObject> mapOfCubes = new Dictionary<Vector3, GameObject>();

    /// <summary>
    /// This function initializes the Floor Object
    /// </summary>
    /// <param name="floorTileprefab"></param>
    /// <param name="bounds"></param>
    public void Initialize(GameObject floorTilePrefab, FloorBounds bounds)
    {
        this.floorTilePrefab = floorTilePrefab;
        this.bounds = bounds;
        GameManagerReference = GameObject.Find("GameManager");
    }

    public void UpdateBounds(FloorBounds bounds) 
    {
        this.bounds = bounds;
    }

    /// <summary>
    /// This function generates the floor
    /// </summary>
    public void CreateFloor()
    {     
        for (int currXIndex = 0; currXIndex < bounds.NumOfCubesInXDir; currXIndex++)
        {
            for (int currZIndex = 0; currZIndex < bounds.NumOfCubesInZDir; currZIndex++)
            {
                Vector3 tempVector3 = CreateVector3(currXIndex, currZIndex);
                GameObject cube = CreateGameObject(tempVector3, floorTilePrefab);
                mapOfCubes[tempVector3] = cube;
            }
        }
    }

    /// <summary>
    /// Create a new Vector3 with the parameters. Used to create keys for a dictionary.
    /// </summary>
    /// <param name="xCoord"></param>
    /// <param name="zCoord"></param>
    /// <returns>Vector3</returns>
    private Vector3 CreateVector3(int xCoord, int zCoord)
    {
        return new Vector3(xCoord, 0, zCoord);
    }

    /// <summary>
    /// Creates a GameObject with the given parameters.
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private GameObject CreateGameObject(Vector3 spawnPosition, GameObject prefab)
    {
        GameObject cube = Instantiate(prefab, spawnPosition, Quaternion.identity) as GameObject;
        cube.transform.SetParent(GameManagerReference.transform);
        return cube;
    }
}
