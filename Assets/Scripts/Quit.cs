using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

    public void doExitGame()
    {
        Debug.Log("we quit boyz");
        Application.Quit();
    }
}
