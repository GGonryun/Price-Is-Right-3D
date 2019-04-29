using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentController : MonoBehaviour 
{
    /*
     * Declarations for generating a floor
     */
    [SerializeField]
    private int numOfCubesInXDir = 10;
    [SerializeField]
    private int numOfCubesInZDir = 10;

    [SerializeField]
    private GameObject floorTileprefab;

    [SerializeField]
    private Floor floor;

    void Awake()
    {
        floor = gameObject.AddComponent<Floor>();
        InitializeFloor();
        CreateFloor();
        BeginShrinkingFloor();
        List<Vector3> deleteTheseCubes = new List<Vector3>(10);
        deleteTheseCubes.Add(new Vector3(5, 0, 3));
        deleteTheseCubes.Add(new Vector3(4, 0, 3));
        deleteTheseCubes.Add(new Vector3(3, 0, 3));
        deleteTheseCubes.Add(new Vector3(2, 0, 3));
        deleteTheseCubes.Add(new Vector3(1, 0, 3));
        DestroyListOfCubes(deleteTheseCubes);
    }

    public void Initialize(GameObject floorTileprefab, int numOfCubesInXDir, int numOfCubesInZDir)
    {
        // Initialization of floor tile prefab
        this.floorTileprefab = floorTileprefab;

        // Initialization of floor size
        this.numOfCubesInXDir = numOfCubesInXDir;
        this.numOfCubesInZDir = numOfCubesInZDir;
    }

    public void InitializeFloor()
    {
        floor.Initialize(floorTileprefab, numOfCubesInXDir, numOfCubesInZDir);
    }

    public void CreateFloor()
    {
        floor.CreateFloor();
    } 

    public void BeginShrinkingFloor()
    {
        floor.BeginShrinkingFloor();
    }

    //public void DestroyRandomCubes()
    //{
    //    floor.RandomlyDestroyCubes();
    //}

    public void DestroyListOfCubes(List<Vector3> listToDelete) //todo outside EnviromentController create a random selection of cubes function
    {
        floor.DestroyListOfCubes(listToDelete);
    }


    private void Start()
    {
  
        //randomRemovalOfTiles.DestroyFloor(numOfCubesInXDir, numOfCubesInZDir, floor);
    }

    // singleton debugger 
    // todo: put strategies in an list // up to the caller to choose these strategies
    // todo: function to build an enviroment 
    // todo:  another function that lets you start the fall 

    //private void dropCube(List<Vector3> DeletionList ) {
    //    throw new NotImplemetedException();
    //}
}
