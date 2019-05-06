using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Floor))]
public class EnvironmentController : MonoBehaviour, IEnvironmentController
{   
    /*
     * Declarations for creating an environment
     */
    [SerializeField]
    private Floor floor;

    [SerializeField]
    private Vector3 environmentScale = new Vector3(10,1,10);

    [SerializeField] 
    internal int numLayersTopNotDestroy = 1;
    
    /// <summary>
    /// Awake is used to initialize any variables or game state before the game starts.
    /// Awake will initialize the floor game object.
    /// </summary>
    private void Awake() {
        
        floor = gameObject.GetComponent<Floor>();
        if(!floor){
            Debug.LogError("<Color=Red> EnvironmentController <a></a></Color> is missing a Floor component !! ", this);
        }

    }

    /// <summary>
    /// Initializer for creating a Floor using multiple cubes as game objects. 
    /// Side Effects: 
    /// The floor object will initialize floor bounds which controls selecting outer edges of the floor object.
    /// The floor object will create a dictionary of game objects positioned between the floor bounds, called mapOfCubes.
    /// </summary>
    /// <returns> A boolean value to signify if the initialization was successful or not. </returns>
    public bool Initialize()
    {   
        if (floor.Initialize() == false){ //todo: make this return bool 
            Debug.LogError("<Color=Red> EnvironmentController <a></a></Color> could not initialize Floor !! ", this);      
            return false;
        }
        else{
            if(floor.CreateFloor() == false){
                Debug.LogError("<Color=Red>EnvironmentController<a></a></Color> could not initialize Floor !! ", this);
                return false;
            }

            /* Configure position and scale of floor */
            float newPositionX = -((floor.numCubesInXDir * environmentScale.x) / 2);
            float newPositionZ = -((floor.numCubesInZDir * environmentScale.z) / 2);
            gameObject.transform.position = new Vector3(newPositionX, 0, newPositionZ);
            gameObject.transform.localScale = new Vector3(environmentScale.x,environmentScale.y,environmentScale.z);

            //gameObject.GetComponent<FloorBounds>().NumLayersTopNotDestroy = numLayersTopNotDestroy;
            return true;
        }
        
    }

    /// <summary>
    /// Changes the color of each outer edge floor tile
    /// Side Effects: 
    /// A list of Game Objects called outerEdges will be populated with references to the current outer edges of the floor.
    /// </summary>
    /// <returns> A boolean value to signify if the change of color was successful or not. </returns>
    public bool Paint()
    {
        bool paintSuccessful = true;
        
        
        if(floor.UpdateListOfOuterEdgeTiles() == false){
            Debug.LogError("<Color=Red> EnvironmentController <a></a></Color> couldn't paint !!", this);
            return false;
        }  

        for (int idxNum = 0; idxNum < floor.outerEdges.Count; idxNum++)
        {
            GameObject go = floor.outerEdges[idxNum];
            go.SendMessage("ApplyColorChange"); // return bool try catch
        }
        return paintSuccessful;
    }

    
    /// <summary>
    /// Applies gravity to destroy the outer edge floor tiles
    /// </summary>
    /// <returns> A boolean value to signify if gravity has been applied to the outer edge floor tiles. </returns>
    public bool Release()
    {
        bool releaseSuccessful = true;
        for (int idxNum = 0; idxNum < floor.outerEdges.Count; idxNum++)
        {
            GameObject go = floor.outerEdges[idxNum];
            go.SendMessage("UnfreezeConstraints", SendMessageOptions.RequireReceiver);
            go.SendMessage("ApplyGravity", SendMessageOptions.RequireReceiver);
            //test

        }
        
        return floor.UpdateBounds(); 
    }

    public bool PaintRandom(){
        floor.UpdateRandomDeleteList();
        for (int idxNum = 0; idxNum < floor.listToRandomlyDelete.Count; idxNum++)
        {
            GameObject go = floor.listToRandomlyDelete[idxNum];
            go.SendMessage("ApplyColorChange"); // return bool try catch
        }
        return true;
    }

    public bool ReleaseRandom(){
        for (int idxNum = 0; idxNum < floor.listToRandomlyDelete.Count; idxNum++)
        {
            GameObject go = floor.listToRandomlyDelete[idxNum];
            go.SendMessage("UnfreezeConstraints", SendMessageOptions.RequireReceiver);
            go.SendMessage("ApplyGravity", SendMessageOptions.RequireReceiver);
        }
        return true;
    }
}