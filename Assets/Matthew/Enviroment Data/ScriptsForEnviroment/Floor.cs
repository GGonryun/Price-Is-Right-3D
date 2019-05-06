using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    /*
     * Declarations for creating a floor
     */
    [SerializeField]
    private GameObject floorTilePrefab;
    
    [SerializeField]
    internal int numCubesInXDir = 10;

    [SerializeField]
    internal int numCubesInZDir = 10;

    [SerializeField]
    private int height = 10;

    private FloorBounds bounds;

    private int totalNumOfCubes;
    
    private Dictionary<Vector3, GameObject> mapOfCubes;

    internal List<GameObject> outerEdges;

    internal List<GameObject> listToRandomlyDelete;

    /// <summary>
    /// This function initializes the Floor Object through setting up the correct floor bounds.
    /// This function must be called before invoking Floor.CreateFloor().
    /// </summary>
    /// <returns> A boolean value to signify if the initialization was successful or not. </returns>
    public bool Initialize()
    { 
        bounds = gameObject.AddComponent<FloorBounds>();
        if(bounds == null){
            Debug.LogError("<Color=Red> Floor <a></a></Color> could not initialize floor bounds !! ", this);
            return false;
        }

        bounds.Initialize(numCubesInXDir, numCubesInZDir, gameObject.GetComponent<EnvironmentController>().numLayersTopNotDestroy);
        totalNumOfCubes = numCubesInXDir * numCubesInZDir;
        mapOfCubes = new Dictionary<Vector3, GameObject>();
        outerEdges = new List<GameObject>(totalNumOfCubes);
        listToRandomlyDelete = new List<GameObject>(totalNumOfCubes);
        return true;       
    }

    /// <summary>
    /// This function should be called when the user want's the next iteration of the floor outer edges.
    /// </summary>
    /// <returns>  True: bounds could be updated; False: bounds have reached their limits. </returns>
    public bool UpdateBounds() 
    {
        return bounds.UpdateBounds();
    }

    /// <summary>
    /// This function generates the floor.
    /// </summary>
    /// <returns>  True: floor generated successfully; False: floor generated unsuccessfully. </returns>
    public bool CreateFloor()
    {     
        for (int currXIndex = 0; currXIndex < bounds.NumOfCubesInXDir; currXIndex++)
        {
            for (int currZIndex = 0; currZIndex < bounds.NumOfCubesInZDir; currZIndex++)
            {
                Vector3 tempVector3 = CreateVector3(currXIndex, currZIndex);
                GameObject cube = CreateGameObject(tempVector3);
                if (cube == null){
                    Debug.LogError("<Color=Red> Floor <a></a></Color> could not create floor because  !!", this); // todo: give a reason
                    return false;
                }
                mapOfCubes[tempVector3] = cube;
            }
        }
        return true;
    }

    /// <summary>
    /// This function creates and returns a new Vector3 with the passed in parameters. Used to create keys for a dictionary.
    /// </summary>
    /// <param name="xCoord"></param>
    /// <param name="zCoord"></param>
    /// <returns>Vector3</returns>
    public Vector3 CreateVector3(int xCoord, int zCoord)
    {
        return new Vector3(xCoord, height, zCoord);
    }

    /// <summary>
    /// Creates a GameObject with the passed in parameters.
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="prefab"></param>
    /// <returns>GameObject</returns>
    private GameObject CreateGameObject(Vector3 spawnPosition)
    {
        GameObject cube = Instantiate(floorTilePrefab, spawnPosition, Quaternion.identity) as GameObject;
        cube.transform.SetParent(this.transform);
        return cube;
    }

    /// <summary>
    /// This function generates a list of game objects that are current edges of the floor.
    /// </summary>
    internal bool UpdateListOfOuterEdgeTiles()
    {
        outerEdges.Clear();
        for (int currentZIdx = bounds.MinBoundZCoord; currentZIdx < bounds.MaxBoundZCoord; currentZIdx++)
        {
            for (int currentXIdx = bounds.MinBoundXCoord; currentXIdx < bounds.MaxBoundXCoord; currentXIdx++)
            {
                if (currentZIdx == bounds.MinBoundZCoord || currentZIdx == bounds.MaxBoundZCoord - 1)
                {
                    Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                    if (mapOfCubes[tempVector3].activeSelf == true) // todo talk to miguel about garbage collection of floor tiles
                    {
                        outerEdges.Add(mapOfCubes[tempVector3]);
                    }
                }
                else if (currentXIdx == bounds.MinBoundXCoord || currentXIdx == bounds.MaxBoundXCoord - 1)
                {
                    Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                    if (mapOfCubes[tempVector3].activeSelf == true) // todo talk to miguel about garbage collection of floor tiles
                    {
                        outerEdges.Add(mapOfCubes[tempVector3]);
                    }
                }
            }
        }
        if(outerEdges.Count == 0){
            Debug.LogError("<Color=Red> Floor <a></a></Color> couldn't UpdateListOfOuterEdgeTiles because there are no more active edges !!", this);
            return false;
        } 
        return true;
    }

    internal bool UpdateRandomDeleteList(){
        listToRandomlyDelete.Clear();
        int temp;
        int randomMinBoundX = UnityEngine.Random.Range(bounds.MinBoundXCoord, bounds.MaxBoundXCoord);
        int randomMaxBoundX = UnityEngine.Random.Range(bounds.MinBoundXCoord, bounds.MaxBoundXCoord);
        if (randomMinBoundX > randomMaxBoundX){
            temp = randomMinBoundX;
            randomMinBoundX = randomMaxBoundX;
            randomMaxBoundX = temp;
        }

        int randomMinBoundZ = UnityEngine.Random.Range(bounds.MinBoundZCoord, bounds.MaxBoundZCoord);
        int randomMaxBoundZ = UnityEngine.Random.Range(bounds.MinBoundZCoord, bounds.MaxBoundZCoord);
        if (randomMinBoundZ > randomMaxBoundZ){
            temp = randomMinBoundZ;
            randomMinBoundZ = randomMaxBoundZ;
            randomMaxBoundZ = temp;
        }

        for(int zIdx = randomMinBoundZ; zIdx < randomMaxBoundZ; zIdx++){
            for(int xIdx = randomMinBoundX; xIdx < randomMaxBoundX; xIdx++){
                Vector3 tempVector3 = CreateVector3(xIdx, zIdx);
                if (mapOfCubes[tempVector3].activeSelf == true) // todo talk to miguel about garbage collection of floor tiles
                {
                    listToRandomlyDelete.Add(mapOfCubes[tempVector3]);
                }
            }
        }

        return true;
    }
    
}
