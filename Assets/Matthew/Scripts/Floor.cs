using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private int numOfCubesInXDir;
    private int numOfCubesInZDir;

    private int minBoundXCoord;
    private int minBoundXCoordLimit;
    private int maxBoundXCoord;
    private int maxBoundXCoordLimit;

    private int minBoundZCoord;
    private int minBoundZCoordLimit;
    private int maxBoundZCoord;
    private int maxBoundZCoordLimit;

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

        InitializeBounds(numOfCubesInXDir, numOfCubesInZDir);
    }



    private void InitializeBounds(int numOfCubesInXDir, int numOfCubesInZDir)
    {
        if ((numOfCubesInXDir % 2) == 0)
        {
            minBoundXCoord = 0;
            maxBoundXCoord = numOfCubesInXDir;
            minBoundXCoordLimit = (int)(numOfCubesInXDir / 2);
            maxBoundXCoordLimit = numOfCubesInXDir - (int)(numOfCubesInXDir / 2); // exclusive; maxBoundX should not decrement if maxBoundX is == maxBoundxLimit
        }
        else
        {
            minBoundXCoord = 0;
            maxBoundXCoord = numOfCubesInXDir;
            minBoundXCoordLimit = (int)(numOfCubesInXDir / 2) + 1;
            maxBoundXCoordLimit = (int)(numOfCubesInXDir / 2) + 1;
        }

        if ((numOfCubesInZDir % 2) == 0)
        {
            minBoundZCoord = 0;
            maxBoundZCoord = numOfCubesInZDir;
            minBoundZCoordLimit = (int)(numOfCubesInZDir / 2);
            maxBoundZCoordLimit = numOfCubesInZDir - (int)(numOfCubesInZDir / 2);
        }
        else
        {
            minBoundZCoord = 0;
            maxBoundZCoord = numOfCubesInZDir;
            minBoundZCoordLimit = (int)(numOfCubesInZDir / 2) + 1;
            maxBoundZCoordLimit = numOfCubesInZDir - (int)(numOfCubesInZDir / 2);

        }
    }

    /// <summary>
    /// This function generates the floor
    /// </summary>
    public void CreateFloor()
    {
        floor = floorGenerator.GenerateFloor(numOfCubesInXDir, numOfCubesInZDir);
    }

    
    private int callCount = 0;
    /// <summary>
    /// This function is called to trigger the floor to shrink it's edges 
    /// </summary>
    public void RemoveFloorEdge() //todo create a limit of how many times this function can be called
    {
        callCount += 1;
        Debug.Log("RemoveFloorEdge call # : " + callCount);
        Debug.Log("minBoundXCoordLimit: " + minBoundXCoordLimit + "; maxBoundXCoordLimit: " + maxBoundXCoordLimit);
        Debug.Log("minBoundZCoordLimit: " + minBoundZCoordLimit + "; maxBoundZCoordLimit: " + maxBoundZCoordLimit);

        shrinkMapEdges.DestroyFloor(numOfCubesInXDir, numOfCubesInZDir, floor, null, minBoundXCoord, maxBoundXCoord, minBoundZCoord, maxBoundZCoord);
        /*
        * Update the bounds of the floor being selected for the next iteration
        */
        if (minBoundXCoord < minBoundXCoordLimit)
        {
            minBoundXCoord += 1;
            maxBoundXCoord -= 1;
        }
        if (minBoundZCoord < minBoundZCoordLimit)
        {
            minBoundZCoord += 1;
            maxBoundZCoord -= 1;
        }

    }

    public void DestroyListOfCubes(List<Vector3> listToDelete)
    {
        selectedRemovalOfTiles.DestroyFloor(numOfCubesInXDir, numOfCubesInZDir, floor, listToDelete, 0, 0, 0, 0); //todo change the implementation so we don't have to provide 0's
    }
}
