using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkFloorStrategy : MonoBehaviour, IFloorDestroyer
{
    private readonly int colorDelay = 0;
    private readonly int intermediateDelay = 3;

    private List<GameObject> listOfSelectedCubes = new List<GameObject>(10);
    private readonly object balanceLock = new object();

    public void Destroy(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor, List<Vector3> cubesToDestroy,
        int minBoundXCoord, int maxBoundXCoord, int minBoundZCoord, int maxBoundZCoord)
    {
        //listOfSelectedCubes = new List<GameObject>(10);
        // Ignore cubesToDestroy for this strategy, the object will have a null value
        DestroyFloorWithTimer(numOfCubesInXDir, numOfCubesInZDir, floor, minBoundXCoord, maxBoundXCoord, minBoundZCoord, maxBoundZCoord);
        //Debug.Log(" out of routine listofSelectedCubes.count = " + listOfSelectedCubes.Count);
        //listOfSelectedCubes.Clear(); // why did this not work in in IENUMERATOR???
        //Debug.Log("listofSelectedCubes.count = " + listOfSelectedCubes.Count);
    }

    private void DestroyFloorWithTimer(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor, int minBoundXCoord, int maxBoundXCoord, int minBoundZCoord, int maxBoundZCoord)
    {
        lock (balanceLock) {
            listOfSelectedCubes = new List<GameObject>(10);
            Debug.Log("RemoveFloorEdge Call initial parameters");
            Debug.Log("minBoundXCoord : " + minBoundXCoord + "; maxBoundXCoord: " + maxBoundXCoord);
            Debug.Log("minBoundZCoord : " + minBoundZCoord + "; maxBoundZCoord: " + maxBoundZCoord);
            Debug.Log("End of initial parameters");

            for (int currentZIdx = minBoundZCoord; currentZIdx < maxBoundZCoord; currentZIdx++)
            {
                for (int currentXIdx = minBoundXCoord; currentXIdx < maxBoundXCoord; currentXIdx++)
                {
                    Debug.Log("---Beginning---");
                    // if currentZIdx is equal to minBoundZCoord or maxBoundZCoord then select game objects at all (currentXIdx, 0, currentZIdx) 
                    if (currentZIdx == minBoundZCoord || currentZIdx == maxBoundZCoord - 1)
                    {
                        bool firstConditionalValue = (currentZIdx == minBoundZCoord || currentZIdx == maxBoundZCoord - 1);
                        Debug.Log("if (currentZIdx == minBoundZCoord || currentZIdx == maxBoundZCoord - 1)  is : " + firstConditionalValue);
                        Debug.Log("minBoundXCoord : " + minBoundXCoord + "; maxBoundXCoord: " + maxBoundXCoord);
                        Debug.Log("minBoundZCoord : " + minBoundZCoord + "; maxBoundZCoord: " + maxBoundZCoord);
                        Debug.Log("CurrentXIdx: " + currentXIdx + "; CurrentZIdx: " + currentZIdx);

                        Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                        if (floor[tempVector3].activeSelf == true)
                        {
                            Debug.Log(" ^ DESTROYED! first conditional^ ");
                            listOfSelectedCubes.Add(floor[tempVector3]);
                        }
                    }
                    // if currZIdx is not equal to minBoundZCoord or maxBoundZCoord then only select the game objects at all (currentXIdx, 0, currentZIdx) when currentXIdx equals minBoundXCoord and MaxBoundXCoord
                    else if (currentXIdx == minBoundXCoord || currentXIdx == maxBoundXCoord - 1)
                    {
                        bool secondConditionalValue = (currentXIdx == minBoundXCoord || currentXIdx == maxBoundXCoord - 1);
                        Debug.Log("if (currentXIdx == minBoundXCoord || currentXIdx == maxBoundXCoord - 1) is : " + secondConditionalValue);
                        bool secondConditionalValueerrorlogiccheck = (currentXIdx == minBoundXCoord || currentXIdx == (maxBoundXCoord - 1));
                        Debug.Log("if (currentXIdx == minBoundXCoord || currentXIdx == (maxBoundXCoord - 1))  is : " + secondConditionalValueerrorlogiccheck);
                        Debug.Log("minBoundXCoord : " + minBoundXCoord + "; maxBoundXCoord: " + maxBoundXCoord);
                        Debug.Log("minBoundZCoord : " + minBoundZCoord + "; maxBoundZCoord: " + maxBoundZCoord);
                        Debug.Log("CurrentXIdx: " + currentXIdx + "; CurrentZIdx: " + currentZIdx);
                        Vector3 tempVector3 = CreateVector3(currentXIdx, currentZIdx);
                        if (floor[tempVector3].activeSelf == true)
                        {
                            Debug.Log(" ^ DESTROYED! second conditional^ ");
                            listOfSelectedCubes.Add(floor[tempVector3]);
                        }

                    }
                    Debug.Log("---End---");
                }
            }
            Debug.Log("Out of for loops; listofSelectedCubes.count = " + listOfSelectedCubes.Count);

            //yield return new WaitForSeconds(colorDelay);
            Debug.Log("color delay done");
            int idxNum2 = 0;
            Debug.Log("listofSelectedCubes.count = " + listOfSelectedCubes.Count);
            Debug.Log("idxNum < listOfSelectedCubes: " + (idxNum2 < listOfSelectedCubes.Count));
            for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
            {
                GameObject go = listOfSelectedCubes[idxNum];
                go.SendMessage("ApplyColorChange");
                Debug.Log("ApplyColorChange done");
                //yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }

            //yield return new WaitForSeconds(intermediateDelay);
            for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
            {
                GameObject go = listOfSelectedCubes[idxNum];
                go.SendMessage("ApplyGravity");
                go.SendMessage("UnfreezeConstraints");
                //yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }

            // Deactivate cubes that have been destroyed
            for (int idxNum = 0; idxNum < listOfSelectedCubes.Count; idxNum++)
            {
                GameObject go = listOfSelectedCubes[idxNum];
                go.SendMessage("SetNotActive"); //todo create random destroy cubes and only drop if still active
                //yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }
        }
        
    }

    private Vector3 CreateVector3(int xCoord, int zCoord) //todo maybe create this at start 
    {
        return new Vector3(xCoord, 0, zCoord);
    }
}
