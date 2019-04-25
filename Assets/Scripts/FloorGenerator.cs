using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    /*
     * Declarations for Floor Size
     */
    [SerializeField]
    private int numOfCubesInXDir;
    [SerializeField]
    private int numOfCubesInZDir;
    
    /*
     * Declaration to hold Floor of Cubes
     */

    private IFloor generationStategy; 

    public FloorGenerator(IFloor floorStrategy)
    {
        this.generationStategy = floorStrategy;
    }

    public void SetGenerationStrategy(IFloor strategy)
    {
        this.generationStategy = strategy;
    }
    
    public Dictionary<Vector3, GameObject> GenerateFloor()
    {
        return generationStategy.generate(numOfCubesInXDir, numOfCubesInZDir);
    }
}
