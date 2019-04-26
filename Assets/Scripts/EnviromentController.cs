using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentController : MonoBehaviour 
{
    /*
     * Declarations for Floor Size
     */
    [SerializeField]
    private int numOfCubesInXDir = 10;
    [SerializeField]
    private int numOfCubesInZDir = 10;

    [SerializeField]
    private GameObject floorTileprefab;

    IFloor floorGenerationStrategy;
    FloorGenerator floorGenerator;
    private Dictionary<Vector3, GameObject> floor;

    IFloorDestroyer shrinkMapEdgesStrategy;
    FloorDestroyer shrinkMapEdges;

    IFloorDestroyer randomRemovalOfTilesStrategy;
    FloorDestroyer randomRemovalOfTiles;

    void Awake()
    {
        floorGenerationStrategy = gameObject.AddComponent<FlatFloorConcreteStrategy>();
        floorGenerationStrategy.SetPrefab(floorTileprefab);

        floorGenerator = gameObject.AddComponent<FloorGenerator>();
        floorGenerator.SetGenerationStrategy(floorGenerationStrategy); 
        floor = floorGenerator.GenerateFloor(numOfCubesInXDir, numOfCubesInZDir);
 
        shrinkMapEdgesStrategy = gameObject.AddComponent<ShrinkFloorStrategy>();
        shrinkMapEdges = gameObject.AddComponent<FloorDestroyer>();
        shrinkMapEdges.SetGenerationStrategy(shrinkMapEdgesStrategy);

        randomRemovalOfTilesStrategy = gameObject.AddComponent<RandomRemovalOfTilesStrategy>();
        randomRemovalOfTiles = gameObject.AddComponent<FloorDestroyer>();
        randomRemovalOfTiles.SetGenerationStrategy(randomRemovalOfTilesStrategy);
    }

    private void Start()
    {
        shrinkMapEdges.DestroyFloor(numOfCubesInXDir, numOfCubesInZDir, floor);
        //randomRemovalOfTiles.DestroyFloor(numOfCubesInXDir, numOfCubesInZDir, floor);
    }

}
