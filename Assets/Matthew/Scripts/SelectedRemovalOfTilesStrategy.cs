using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedRemovalOfTilesStrategy : MonoBehaviour, IFloorDestroyer
{
    private readonly int colorDelay = 0;
    private readonly int intermediateDelay = 3;
    private readonly int deactivationDelay = 2;

    public void Destroy(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor, List<Vector3> cubesToDestroy, int minBoundXCoord, int maxBoundXCoord, int minBoundZCoord, int maxBoundZCoord)
    {
        StartCoroutine(DestroyFloorWithTimer(numOfCubesInXDir, numOfCubesInZDir, floor, cubesToDestroy));
    }

    private IEnumerator DestroyFloorWithTimer(int numOfCubesInXDir, int numOfCubesInZDir, Dictionary<Vector3, GameObject> floor, List<Vector3> cubesToDestroy)
    {
        yield return new WaitForSeconds(colorDelay);
        for (int idxNum = 0; idxNum < cubesToDestroy.Count; idxNum++)
        {
            if (floor[cubesToDestroy[idxNum]].activeSelf == true)
            {
                GameObject go = floor[cubesToDestroy[idxNum]];
                go.SendMessage("ApplyColorChange");
                yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`            
            }
        }

        yield return new WaitForSeconds(intermediateDelay);

        for (int idxNum = 0; idxNum < cubesToDestroy.Count; idxNum++)
        {
            if (floor[cubesToDestroy[idxNum]].activeSelf == true)
            {
                GameObject go = floor[cubesToDestroy[idxNum]];
                go.SendMessage("ApplyGravity");
                go.SendMessage("UnfreezeConstraints");
                yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }
        }

        yield return new WaitForSeconds(deactivationDelay);
        // Deactivate cubes that have been destroyed
        for (int idxNum = 0; idxNum < cubesToDestroy.Count; idxNum++)
        {
            if (floor[cubesToDestroy[idxNum]].activeSelf == true)
            {
                GameObject go = floor[cubesToDestroy[idxNum]];
                go.SendMessage("SetNotActive"); //todo create random destroy cubes and only drop if still active
                yield return null; //wait 1 frame `yield return new WaitForEndOfFrame()`
            }
        }
    }
}
