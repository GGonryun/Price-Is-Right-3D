using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentController : MonoBehaviour, IEnviroment
{
    [SerializeField]
    private GameObject floorTileprefab;

    private Floor floor;

    private FloorBounds bounds;

    private List<GameObject> listOfSelectedCubes = new List<GameObject>(10);

    /// <summary>
    /// Initializer for creating a Floor. 
    /// </summary>
    /// <param name="floorTileprefab"> Floor tile prefab to be used when generating the floor. </param>
    /// <param name="numOfCubesInXDir"> Number of tiles to make in the X direction of x,y,z space. </param>
    /// <param name="numOfCubesInZDir"> Number of tiles to make in the Z direction of x,y,z space.</param>
    /// <returns> A boolean value to signify if the initialization was successful or not. </returns>
    public bool Initialize(GameObject floorTileprefab, int numOfCubesInXDir, int numOfCubesInZDir)
    {
        bool initializeSuccessful = true;

        this.floorTileprefab = floorTileprefab;

        bounds = new FloorBounds(numOfCubesInXDir, numOfCubesInZDir); 

        floor = gameObject.AddComponent<Floor>();
        InitializeFloor();
        CreateFloor();

        return initializeSuccessful;
    }

    public bool Paint()
    {
        bool paintSuccessful = true;
        GetListOfTiles();
        for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
        {
            GameObject go = listOfSelectedCubes[idxNum];
            go.SendMessage("ApplyColorChange");
        }
        return paintSuccessful;
    }

    

    public bool Release()
    {
        bool releaseSuccessful = true;
        for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
        {
            GameObject go = listOfSelectedCubes[idxNum];
            go.SendMessage("UnfreezeConstraints");
            go.SendMessage("ApplyGravity");   
        }
        bounds.UpdateBounds();
        floor.UpdateBounds(bounds);
        listOfSelectedCubes.Clear(); // for next iteration
        return releaseSuccessful;
    }

    /// <summary>
    /// This function must be called before CreateFloor() because it sets up variables to hold correct values.
    /// </summary>
    public void InitializeFloor()
    {
        floor.Initialize(this.floorTileprefab, this.bounds); //todo just pass in bounds
    }

    /// <summary>
    /// Once InitializeFloor is called you can call this function which generates the floor.
    /// </summary>
    public void CreateFloor()
    {
        floor.CreateFloor();  //todo delete Debug statements
    }

    /// <summary>
    /// This Function should be called to generate a list of floor tiles to alter.
    /// </summary>
    private void GetListOfTiles()
    {
        for (int currentZIdx = bounds.MinBoundZCoord; currentZIdx < bounds.MaxBoundZCoord; currentZIdx++)
        {
            for (int currentXIdx = bounds.MinBoundXCoord; currentXIdx < bounds.MaxBoundXCoord; currentXIdx++)
            {
                if (currentZIdx == bounds.MinBoundZCoord || currentZIdx == bounds.MaxBoundZCoord - 1)
                {
                    Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                    if (floor.mapOfCubes[tempVector3].activeSelf == true)
                    {
                        listOfSelectedCubes.Add(floor.mapOfCubes[tempVector3]);
                    }
                }
                else if (currentXIdx == bounds.MinBoundXCoord || currentXIdx == bounds.MaxBoundXCoord - 1)
                {
                    Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                    if (floor.mapOfCubes[tempVector3].activeSelf == true)
                    {
                        listOfSelectedCubes.Add(floor.mapOfCubes[tempVector3]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Create a new Vector3 with the parameters. Used to create keys for a dictionary.
    /// </summary>
    /// <param name="xCoord"></param>
    /// <param name="zCoord"></param>
    /// <returns>Vector3</returns>
    private Vector3 CreateVector3(int xCoord, int zCoord) //todo maybe create this at start 
    {
        return new Vector3(xCoord, 0, zCoord);
    }
}