using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    
    private IFloor generationStategy; 

    public void SetGenerationStrategy(IFloor strategy)
    {
        this.generationStategy = strategy;
    }
    
    public Dictionary<Vector3, GameObject> GenerateFloor(int numOfCubesInXDir, int numOfCubesInZDir)
    {
        return generationStategy.Generate(numOfCubesInXDir, numOfCubesInZDir);
    }
}
