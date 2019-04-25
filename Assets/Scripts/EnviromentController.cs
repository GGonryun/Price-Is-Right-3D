using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentController : MonoBehaviour
{
    IFloor floorGenerationStrategy;
    FloorGenerator floorGenerator;
    public Dictionary<Vector3, GameObject> floor;
    void Awake()
    {
        /* ERROR: 
         * You are trying to create a MonoBehaviour using the 'new' keyword.  
         * This is not allowed.  MonoBehaviours can only be added using AddComponent(). 
         */
        //floorGenerationStrategy = new FlatFloorConcreteStrategy();
        //floorGenerator = new FloorGenerator(floorGenerationStrategy);
        //floor = floorGenerator.GenerateFloor();

        /* Take 1 at solution */
        floorGenerationStrategy = gameObject.AddComponent<FlatFloorConcreteStrategy>();
        floorGenerator = gameObject.AddComponent<FloorGenerator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
