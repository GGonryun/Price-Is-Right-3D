using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnviroment
{
    bool Initialize(GameObject floorTileprefab, int numOfCubesInXDir, int numOfCubesInZDir);
    bool Paint();
    bool Release();
}
