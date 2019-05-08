using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnvironmentController))]
[RequireComponent(typeof(FloorBounds))]
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
    internal int height = 10;  //todo - get checked if this should be internal

    private FloorBounds bounds;

    private int totalNumOfCubes;
    
    private Dictionary<Vector3, GameObject> mapOfCubes;

    internal List<GameObject> outerEdges;

    internal List<GameObject> listToRandomlyDelete;

    private List<Vector3> listToNotDelete;

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

        bounds.Initialize(numCubesInXDir, numCubesInZDir, gameObject.GetComponent<EnvironmentController>().numLayersTopNotDestroy);  //todo - is there a better way to do this?
        totalNumOfCubes = numCubesInXDir * numCubesInZDir;
        mapOfCubes = new Dictionary<Vector3, GameObject>();
        outerEdges = new List<GameObject>(totalNumOfCubes);
        listToRandomlyDelete = new List<GameObject>(totalNumOfCubes);
        listToNotDelete = new List<Vector3>();
        GenerateListToNotDelete();

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

    private void GenerateListToNotDelete(){ //todo - still need to incorporate num of layers not to destroy
        int oddMiddleX = 0;
        List<int> evenMiddleX = new List<int>();
        int oddMiddleZ = 0;
        List<int> evenMiddleZ = new List<int>();
        bool numCubesInXDirEven = numCubesInXDir % 2 == 0;
        bool numCubesInZDirEven = numCubesInZDir % 2 == 0;

        if(numCubesInXDirEven == false){
            oddMiddleX = numCubesInXDir /2;
        }
        else if(numCubesInXDirEven == true){
            evenMiddleX.Add(numCubesInXDir /2);
            evenMiddleX.Add((numCubesInXDir /2)-1);
        }

        if(numCubesInZDirEven == false){
            oddMiddleZ = numCubesInZDir /2;
        }
        else if(numCubesInZDirEven){
            evenMiddleZ.Add(numCubesInZDir /2);
            evenMiddleZ.Add((numCubesInZDir /2)-1);
        }


        if(numCubesInXDirEven == true && numCubesInZDirEven == true){
            listToNotDelete.Add(new Vector3(evenMiddleX[0], height, evenMiddleZ[0]));
            listToNotDelete.Add(new Vector3(evenMiddleX[1], height, evenMiddleZ[0]));
            listToNotDelete.Add(new Vector3(evenMiddleX[0], height, evenMiddleZ[1]));
            listToNotDelete.Add(new Vector3(evenMiddleX[1], height, evenMiddleZ[1]));


        }
        else if(numCubesInXDirEven == false && numCubesInZDirEven == true){
            listToNotDelete.Add(new Vector3(oddMiddleX, height, evenMiddleZ[0]));
            listToNotDelete.Add(new Vector3(oddMiddleX, height, evenMiddleZ[1]));

        }
        else if(numCubesInXDirEven == true && numCubesInZDirEven == false){
             listToNotDelete.Add(new Vector3(evenMiddleX[0], height, oddMiddleZ));
        }
        else if(numCubesInXDirEven == false && numCubesInZDirEven == false){
            listToNotDelete.Add(new Vector3(oddMiddleX, height, oddMiddleZ));
        }
        
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
                // if(currentXIdx >= (bounds.MaxBoundXCoordLimit - bounds.NumLayersTopNotDestroy) && currentXIdx <= (bounds.MaxBoundXCoordLimit + bounds.NumLayersTopNotDestroy)){ -1?
                //     continue;
                // }
                // else if(currentZIdx >= (bounds.MaxBoundZCoordLimit - bounds.NumLayersTopNotDestroy) && currentZIdx <= (bounds.MaxBoundZCoordLimit + bounds.NumLayersTopNotDestroy)){
                //     continue;
                // }
                 if (currentZIdx == bounds.MinBoundZCoord || currentZIdx == bounds.MaxBoundZCoord - 1)
                {
                    Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                    if ((mapOfCubes[tempVector3].activeSelf == true) && !listToNotDelete.Contains(tempVector3)) // todo talk to miguel about garbage collection of floor tiles
                    {
                        outerEdges.Add(mapOfCubes[tempVector3]);
                    }
                }
                else if (currentXIdx == bounds.MinBoundXCoord || currentXIdx == bounds.MaxBoundXCoord - 1)
                {
                    Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                    if ((mapOfCubes[tempVector3].activeSelf == true) && !listToNotDelete.Contains(tempVector3)) // todo talk to miguel about garbage collection of floor tiles
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
        if (randomMaxBoundX - randomMinBoundX > 3){
            randomMaxBoundX = randomMinBoundX + 3; 
        }

        int randomMinBoundZ = UnityEngine.Random.Range(bounds.MinBoundZCoord, bounds.MaxBoundZCoord);
        int randomMaxBoundZ = UnityEngine.Random.Range(bounds.MinBoundZCoord, bounds.MaxBoundZCoord);
        if (randomMinBoundZ > randomMaxBoundZ){
            temp = randomMinBoundZ;
            randomMinBoundZ = randomMaxBoundZ;
            randomMaxBoundZ = temp;
        }
        if (randomMaxBoundZ - randomMinBoundZ > 3){
            randomMaxBoundZ = randomMinBoundZ + 3; 
        }

        for(int zIdx = randomMinBoundZ; zIdx < randomMaxBoundZ; zIdx++){
            for(int xIdx = randomMinBoundX; xIdx < randomMaxBoundX; xIdx++){
                Vector3 tempVector3 = CreateVector3(xIdx, zIdx);
                if ((mapOfCubes[tempVector3].activeSelf == true) && !listToNotDelete.Contains(tempVector3)) // todo talk to miguel about garbage collection of floor tiles
                {   
                    try{
                        listToRandomlyDelete.Add(mapOfCubes[tempVector3]);
                    }
                    catch(System.Exception e){
                        Debug.Log("Could Not randomly delete a cube");
                    }
                }
            }
        }
      
        return true;
    }
    
}
