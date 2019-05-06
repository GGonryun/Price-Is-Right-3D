using UnityEngine;

public class FloorBounds : MonoBehaviour {

    private int numOfCubesInXDir;
    private int numOfCubesInZDir;

    private int minBoundXCoord;
    private int maxBoundXCoord;
    private int minBoundXCoordLimit;
    private int maxBoundXCoordLimit;

    private int minBoundZCoord;
    private int maxBoundZCoord;
    private int minBoundZCoordLimit;
    private int maxBoundZCoordLimit;
    
    private int numLayersTopNotDestroy = 1; //todo - should I set this through environment


    public int MinBoundXCoord { get => minBoundXCoord; set => minBoundXCoord = value; }
    public int MaxBoundXCoord { get => maxBoundXCoord; set => maxBoundXCoord = value; }
    public int MinBoundXCoordLimit { get => minBoundXCoordLimit; set => minBoundXCoordLimit = value; }
    public int MaxBoundXCoordLimit { get => maxBoundXCoordLimit; set => maxBoundXCoordLimit = value; }
    public int MinBoundZCoord { get => minBoundZCoord; set => minBoundZCoord = value; }
    public int MaxBoundZCoord { get => maxBoundZCoord; set => maxBoundZCoord = value; }
    public int MinBoundZCoordLimit { get => minBoundZCoordLimit; set => minBoundZCoordLimit = value; }
    public int MaxBoundZCoordLimit { get => maxBoundZCoordLimit; set => maxBoundZCoordLimit = value; }
    public int NumOfCubesInXDir { get => numOfCubesInXDir; set => numOfCubesInXDir = value; }
    public int NumOfCubesInZDir { get => numOfCubesInZDir; set => numOfCubesInZDir = value; }

    public int NumLayersTopNotDestroy { get => numLayersTopNotDestroy; set => numLayersTopNotDestroy = value; }

    public void Initialize(int numCubesInXDir, int numCubesInZDir, int numLayersTopNotDestroy)
    {
        NumOfCubesInXDir = numCubesInXDir;
        NumOfCubesInZDir = numCubesInZDir;
        
        if ((NumOfCubesInXDir % 2) == 0)
        {
            MinBoundXCoord = 0;
            MaxBoundXCoord = numCubesInXDir;
            MinBoundXCoordLimit = (int)(numCubesInXDir / 2);
            MaxBoundXCoordLimit = numCubesInXDir - (int)(numCubesInXDir / 2); // exclusive; maxBoundX should not decrement if maxBoundX is == maxBoundxLimit
        }
        else
        {
            MinBoundXCoord = 0;
            MaxBoundXCoord = numCubesInXDir;
            MinBoundXCoordLimit = (int)(numCubesInXDir / 2) + 1;
            MaxBoundXCoordLimit = (int)(numCubesInXDir / 2) + 1;
        }

        if ((NumOfCubesInZDir % 2) == 0)
        {
            MinBoundZCoord = 0;
            MaxBoundZCoord = numCubesInZDir;
            MinBoundZCoordLimit = (int)(numCubesInZDir / 2);
            MaxBoundZCoordLimit = numCubesInZDir - (int)(numCubesInZDir / 2);
        }
        else
        {
            MinBoundZCoord = 0;
            MaxBoundZCoord = numCubesInZDir;
            MinBoundZCoordLimit = (int)(numCubesInZDir / 2) + 1;
            MaxBoundZCoordLimit = numCubesInZDir - (int)(numCubesInZDir / 2);

        }
        
        NumLayersTopNotDestroy = numLayersTopNotDestroy;
    }

   

    /// <summary>
    /// Update the bounds of the floor being selected for the next iteration
    /// </summary>
    /// <returns> A boolean value signifying if the bounds could be updated or not. Will return false if the bounds have hit their limits.</returns>
    public bool UpdateBounds()
    {
        bool successfulUpdate = true;

        if (MinBoundXCoord < MinBoundXCoordLimit - NumLayersTopNotDestroy)
        {
            MinBoundXCoord += 1;
            MaxBoundXCoord -= 1;
        }
        if (minBoundZCoord < minBoundZCoordLimit - NumLayersTopNotDestroy)
        {
            MinBoundZCoord += 1;
            MaxBoundZCoord -= 1;
        }
        
        if((MinBoundXCoord == (MinBoundXCoordLimit - NumLayersTopNotDestroy)) && (MinBoundZCoord == (MinBoundZCoordLimit - NumLayersTopNotDestroy)))
        {
            successfulUpdate = false;
        }

        return successfulUpdate;
    }
}