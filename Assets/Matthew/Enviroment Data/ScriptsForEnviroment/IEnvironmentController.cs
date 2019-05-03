using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvironmentController
{
    bool Initialize();
    bool Paint();
    bool Release();
}
