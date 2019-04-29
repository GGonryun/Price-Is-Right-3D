using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkFloorStrategy : MonoBehaviour, IFloorDestroyer
{
    private int minBoundXCoord;
    private int minBoundXCoordLimit;
    private int maxBoundXCoord;
    private int maxBoundXCoordLimit;

    private int minBoundZCoord;
    private int minBoundZCoordLimit;
    private int maxBoundZCoord;
    private int maxBoundZCoordLimit;

    private int colorDelay = 0;
    private int intermediateDelay = 3;
    private int iterationDelay = 2;

    private List<GameObject> listOfSelectedCubes = new List<GameObject>(10);

    public void Destroy(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor)
    {
        InitializeBounds(numOfCubesInXDir, numOfCubesInZDir);
        StartCoroutine(DestroyFloorWithTimer(numOfCubesInXDir, numOfCubesInZDir, floor));
    }

    private IEnumerator DestroyFloorWithTimer(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor)
    {
        while(!((minBoundXCoord == minBoundXCoordLimit) && (minBoundZCoord == minBoundZCoordLimit)))
        {
            listOfSelectedCubes.Clear();
            for(int currentZIdx = minBoundZCoord; currentZIdx < maxBoundZCoord; currentZIdx++)
            {
                for(int currentXIdx = minBoundXCoord; currentXIdx < maxBoundXCoord; currentXIdx++)
                {
                    if (currentZIdx == minBoundZCoord || currentZIdx == maxBoundZCoord-1)
                    {
                        Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                        listOfSelectedCubes.Add(floor[tempVector3]);
                    }
                    else if(currentXIdx == minBoundXCoord || currentXIdx == maxBoundXCoord-1 )
                    {
                        Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                        listOfSelectedCubes.Add(floor[tempVector3]);
                    }
                }
            }

            yield return new WaitForSeconds(colorDelay);
            for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
            {
                GameObject go = listOfSelectedCubes[idxNum];
                go.SendMessage("ApplyColorChange");
                yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }

            yield return new WaitForSeconds(intermediateDelay);
            for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
            {
                GameObject go = listOfSelectedCubes[idxNum];
                go.SendMessage("ApplyGravity");
                yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }
            
            /*
             * Update the bounds of the floor being selected for the next iteration
             */
            if (minBoundXCoord < minBoundXCoordLimit)
            {
                minBoundXCoord += 1;
                maxBoundXCoord -= 1;
            }
            if(minBoundZCoord < minBoundZCoordLimit)
            {
                minBoundZCoord += 1;
                maxBoundZCoord -= 1;
            }
            yield return new WaitForSeconds(iterationDelay);

            // Deactivate cubes that have been destroyed
            for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
            {
                GameObject go = listOfSelectedCubes[idxNum];
                go.SendMessage("SetNotActive"); //todo create random destroy cubes and only drop if still active
                yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }
        }
    }

    private void InitializeBounds(int numOfCubesInXDir, int numOfCubesInZDir)
    {
        if((numOfCubesInXDir % 2) == 0)
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

        if((numOfCubesInZDir % 2) == 0)
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

    private Vector3 CreateVector3(int xCoord, int zCoord) //todo maybe create this at start 
    {
        return new Vector3(xCoord, 0, zCoord);
    }
}
