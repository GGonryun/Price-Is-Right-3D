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

    private Floor floor;

    void Awake()
    {
        floor = gameObject.AddComponent<Floor>();
        InitializeFloor();
        CreateFloor();
       
        //StartCoroutine(DestroyFloorWithTimer());


        
        //RemoveFloorEdge();
        //RemoveFloorEdge();
        //List<Vector3> deleteTheseCubes = new List<Vector3>(10);
        //deleteTheseCubes.Add(new Vector3(5, 0, 3));
        //deleteTheseCubes.Add(new Vector3(4, 0, 3));
        //deleteTheseCubes.Add(new Vector3(3, 0, 3));
        //deleteTheseCubes.Add(new Vector3(2, 0, 3));
        //deleteTheseCubes.Add(new Vector3(1, 0, 3));
        //DestroyListOfCubes(deleteTheseCubes);
        //deleteTheseCubes.Add(new Vector3(5, 0, 4));
        //deleteTheseCubes.Add(new Vector3(4, 0, 3));
        //deleteTheseCubes.Add(new Vector3(3, 0, 3));
        //deleteTheseCubes.Add(new Vector3(2, 0, 3));
        //deleteTheseCubes.Add(new Vector3(1, 0, 3));
        //DestroyListOfCubes(deleteTheseCubes);
        //deleteTheseCubes.Add(new Vector3(5, 0, 4));
        //deleteTheseCubes.Add(new Vector3(4, 0, 4));
        //deleteTheseCubes.Add(new Vector3(3, 0, 4));
        //deleteTheseCubes.Add(new Vector3(2, 0, 4));
        //deleteTheseCubes.Add(new Vector3(1, 0, 4));
        //DestroyListOfCubes(deleteTheseCubes);
    }
    private void Start()
    {
        RemoveFloorEdge();
        RemoveFloorEdge();
    }
    
    private IEnumerator DestroyFloorWithTimer()
    {
        RemoveFloorEdge();
        yield return new WaitForSeconds(10);
        RemoveFloorEdge();
        yield return new WaitForSeconds(10);
        RemoveFloorEdge();
        yield return new WaitForSeconds(10);
        RemoveFloorEdge();
    }

    /// <summary>
    /// Initializer for declaring the floor dimensions and prefab for floor tile
    /// </summary>
    /// <param name="floorTileprefab"></param>
    /// <param name="numOfCubesInXDir"></param>
    /// <param name="numOfCubesInZDir"></param>
    public void Initialize(GameObject floorTileprefab, int numOfCubesInXDir, int numOfCubesInZDir)
    {
        // Initialization of floor tile prefab
        this.floorTileprefab = floorTileprefab;

        // Initialization of floor size
        this.numOfCubesInXDir = numOfCubesInXDir;
        this.numOfCubesInZDir = numOfCubesInZDir;
    }

    /// <summary>
    /// This function needs to be called before calling CreateFloor. It initializes the floor assigning the correct values to the floor object.
    /// </summary>
    public void InitializeFloor()
    {
        floor.Initialize(floorTileprefab, numOfCubesInXDir, numOfCubesInZDir);
    }

    /// <summary>
    /// Once InitializeFloor is called you can call this function which generates the floor.
    /// </summary>
    public void CreateFloor()
    {
        floor.CreateFloor();  //todo delete Debug statements
    } 

    /// <summary>
    /// BeginShrinkingFloor() should be called after creating a floor. It will trigger the edges of the floor to be destroyed over time.
    /// </summary>
    public void RemoveFloorEdge() 
    {
        floor.RemoveFloorEdge(); //todo : How can I limit the ammount of times this function is called
    }

    /// <summary>
    /// DestroyListOfCubes() should be called after creating a floor. It will trigger the floor tiles specified with the listToDelete to be deleted. 
    /// </summary>
    /// <param name="listToDelete"></param>
    public void DestroyListOfCubes(List<Vector3> listToDelete) //todo outside EnviromentController create a random selection of cubes function
    {
        floor.DestroyListOfCubes(listToDelete);
    }
}
