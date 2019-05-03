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
    
    /// <summary>
    /// Awake is used to initialize any variables or game state before the game starts.
    /// </summary>
    private void Awake() {
        floor = gameObject.GetComponent<Floor>();
        if(!floor){
            Debug.LogError("<Color=Red> EnvironmentController <a></a></Color> is missing a Floor component !! ", this);
        }
    }

    /// <summary>
    /// Initializer for creating a Floor using multiple cubes as game objects . 
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
            return true;
        }
    }

    /// <summary>
    /// Changes the color of each outer edge floor tile
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
            go.SendMessage("UnfreezeConstraints");
            go.SendMessage("ApplyGravity");   
        }
        floor.UpdateBounds();
    
        return releaseSuccessful;
    }

}