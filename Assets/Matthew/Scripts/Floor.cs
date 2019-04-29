using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private int numOfCubesInXDir;
    private int numOfCubesInZDir;

    Dictionary<Vector3, GameObject> floor = new Dictionary<Vector3, GameObject>();

    private IFloor floorGenerationStrategy;
    private FloorGenerator floorGenerator;

    private IFloorDestroyer shrinkMapEdgesStrategy;
    private FloorDestroyer shrinkMapEdges;
    private IFloorDestroyer selectedRemovalOfTilesStrategy;
    private FloorDestroyer selectedRemovalOfTiles;

    /// <summary>
    /// This function initializes the Floor Object
    /// </summary>
    /// <param name="floorTileprefab"></param>
    /// <param name="numOfCubesInXDir"></param>
    /// <param name="numOfCubesInZDir"></param>
    public void Initialize(GameObject floorTileprefab, int numOfCubesInXDir, int numOfCubesInZDir)
    {
        // Initialization of floor size
        this.numOfCubesInXDir = numOfCubesInXDir;
        this.numOfCubesInZDir = numOfCubesInZDir;

        // Initialization of floor generation strategy
        floorGenerationStrategy = gameObject.AddComponent<FlatFloorConcreteStrategy>();
        floorGenerationStrategy.Initialize(floorTileprefab);
        
        // Initialization of floor generator; which generates the floor based on the floor generation strategy
        floorGenerator = gameObject.AddComponent<FloorGenerator>();
        floorGenerator.SetGenerationStrategy(floorGenerationStrategy);

        // Initialization of the floor destroyer 
        shrinkMapEdgesStrategy = gameObject.AddComponent<ShrinkFloorStrategy>();
        shrinkMapEdges = gameObject.AddComponent<FloorDestroyer>();
        shrinkMapEdges.SetGenerationStrategy(shrinkMapEdgesStrategy);

        selectedRemovalOfTilesStrategy = gameObject.AddComponent<SelectedRemovalOfTilesStrategy>();
        selectedRemovalOfTiles = gameObject.AddComponent<FloorDestroyer>();
        selectedRemovalOfTiles.SetGenerationStrategy(selectedRemovalOfTilesStrategy);
    }

    /// <summary>
    /// This function generates the floor
    /// </summary>
    public void CreateFloor()
    {
        floor = floorGenerator.GenerateFloor(numOfCubesInXDir, numOfCubesInZDir);
    }

    /// <summary>
    /// This function is called to trigger the floor to shrink it's edges 
    /// </summary>
    public void BeginShrinkingFloor()
    {
        shrinkMapEdges.DestroyFloor(numOfCubesInXDir, numOfCubesInZDir, floor, null);
    }

    public void DestroyListOfCubes(List<Vector3> listToDelete)
    {
        selectedRemovalOfTiles.DestroyFloor(numOfCubesInXDir, numOfCubesInZDir, floor, listToDelete);
    }
}
