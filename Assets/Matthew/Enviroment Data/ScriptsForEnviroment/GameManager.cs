using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnvironmentController))]
public class GameManager : MonoBehaviour
{
    //[SerializeField] private IEnvironmentController environment; // if you create an interface then you can't utilize `if(!environment){}`
   [SerializeField] 
   private EnvironmentController environment = null;
   
    /// <summary>
    /// Awake is used to initialize any variables or game state before the game starts.
    /// </summary>
    private void Awake()
    {
        environment = gameObject.GetComponent<EnvironmentController>();
        if(!environment){
            Debug.LogError("<Color=Red> GameManager <a></a></Color>is missing a EnvironmentController component !! ", this);
        }
        else
        {
           environment.Initialize(); 
        }
        
    }
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        if(environment){
            StartCoroutine(DestroyFloorWithTimer());
        }
    }


    /// <summary>
    /// This function will paint and then release the floor edges in a timely manner
    /// </summary>
    private IEnumerator DestroyFloorWithTimer()
    {
        environment.Paint();
        yield return new WaitForSeconds(2);
        environment.Release();
        yield return new WaitForSeconds(5);
        environment.Paint();
        yield return new WaitForSeconds(2);
        environment.Release();
        yield return new WaitForSeconds(5);
        environment.Paint();
        yield return new WaitForSeconds(2);
        environment.Release();
        yield return new WaitForSeconds(5);
    }
}
