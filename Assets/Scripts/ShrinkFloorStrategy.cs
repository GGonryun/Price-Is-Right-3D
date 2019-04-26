using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkFloorStrategy : MonoBehaviour, IFloorDestroyer
{
    //     SelectionStrategy selectionStrategy // code reusability rather do this in the controller 
    int minBoundXCoord; 
    int minBoundXCoordLimit; // inclusive
    int maxBoundXCoord;
    int maxBoundXCoordLimit;

    int minBoundZCoord;
    int minBoundZCoordLimit;
    int maxBoundZCoord;
    int maxBoundZCoordLimit;

    List<GameObject> listOfSelectedCubes = new List<GameObject>(10);

    public void Destroy(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor)
    {
        InitializeBounds(numOfCubesInXDir, numOfCubesInZDir);
        //StartCoroutine(PerformWait(numOfCubesInXDir, numOfCubesInZDir, floor));
        Debug.Log("minBoundXCoord : " + minBoundXCoord + "; minBoundXCoordLimit: " + minBoundXCoordLimit + "; maxBoundXCoord: " + maxBoundXCoord + "; maxBoundXCoordLimit: " + maxBoundXCoordLimit);
        Debug.Log("minBoundZCoord : " + minBoundZCoord + "; minBoundZCoordLimit: " + minBoundZCoordLimit + "; maxBoundZCoord: " + maxBoundZCoord + "; maxBoundZCoordLimit: " + maxBoundZCoordLimit);
        StartCoroutine(PerformWait2(numOfCubesInXDir, numOfCubesInZDir, floor));
    }

    IEnumerator PerformWait2(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor)
    {
       
        while(!((minBoundXCoord == minBoundXCoordLimit) && (minBoundZCoord == minBoundZCoordLimit)))
        {
            // todo clear list each iteration
            for(int currentZIdx = minBoundZCoord; currentZIdx < maxBoundZCoord; currentZIdx++)
            {
                for(int currentXIdx = minBoundXCoord; currentXIdx < maxBoundXCoord; currentXIdx++)
                {
                    Debug.Log("");
                    Debug.Log("CurrentXIdx: " + currentXIdx + "; CurrentZIdx: " + currentZIdx);
                    Debug.Log("minBoundXCoord : " + minBoundXCoord + "; minBoundXCoordLimit: " + minBoundXCoordLimit + "; maxBoundXCoord: " + maxBoundXCoord + "; maxBoundXCoordLimit: " + maxBoundXCoordLimit);
                    Debug.Log("minBoundZCoord : " + minBoundZCoord + "; minBoundZCoordLimit: " + minBoundZCoordLimit + "; maxBoundZCoord: " + maxBoundZCoord + "; maxBoundZCoordLimit: " + maxBoundZCoordLimit);
                    Debug.Log("CurrentXIdx: " + currentXIdx + "; CurrentZIdx: " + currentZIdx);
                    if (currentZIdx == minBoundZCoord || currentZIdx == maxBoundZCoord-1)
                    {
                        Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                        Debug.Log("Delete: " + tempVector3);
                        listOfSelectedCubes.Add(floor[tempVector3]);
                        //floor[tempVector3].gameObject.SendMessage("ApplyGravity");
                        //floor.[tempVector3].sendMessage
                        // put them in a list then apply color to all of them in one for loop // and then after 5 seconds do "our destroy (floor catcher)"
                        //clear list after 
                    }
                    else if(currentXIdx == minBoundXCoord || currentXIdx == maxBoundXCoord-1 )
                    {
                        Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                        Debug.Log("Delete: " + tempVector3);
                        //floor[tempVector3].AddComponent<Rigidbody>();
                        listOfSelectedCubes.Add(floor[tempVector3]);
                    }
                }
            }
           
            // Alter selected floor tiles 


            // when updating next inner grid removal yield return new WaitForSeconds(1.0f);
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
            yield return new WaitForSeconds(5.0f);
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
            minBoundXCoordLimit = (int)(numOfCubesInXDir / 2);
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
            minBoundZCoordLimit = (int)(numOfCubesInZDir / 2);
            maxBoundZCoordLimit = numOfCubesInZDir - (int)(numOfCubesInZDir / 2);

        }
    }

    IEnumerator PerformWait(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor)
    {
        
        for (int currentZIdx = 0; currentZIdx < numOfCubesInZDir; currentZIdx++)
        {
            for (int currentXIdx = 0; currentXIdx < numOfCubesInXDir; currentXIdx++)
            {
                Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                floor[tempVector3].AddComponent<Rigidbody>();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private Vector3 CreateVector3(int xCoord, int zCoord)
    {
        return new Vector3(xCoord, 0, zCoord);
    }
}
