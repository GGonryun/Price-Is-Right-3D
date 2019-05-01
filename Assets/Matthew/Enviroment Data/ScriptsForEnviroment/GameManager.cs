using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    IEnviroment enviroment;
    [SerializeField] GameObject cubePrefab;
    [SerializeField] int numCubesInXDir = 10;
    [SerializeField] int numCubesInZDir = 10;

    private void Awake()
    {
        enviroment = gameObject.AddComponent<EnviromentController>();
        enviroment.Initialize(cubePrefab, numCubesInXDir , numCubesInZDir);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyFloorWithTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator DestroyFloorWithTimer()
    {
        enviroment.Paint();
        yield return new WaitForSeconds(2);
        enviroment.Release();
        yield return new WaitForSeconds(5);
        enviroment.Paint();
        yield return new WaitForSeconds(2);
        enviroment.Release();
        yield return new WaitForSeconds(5);
        enviroment.Paint();
        yield return new WaitForSeconds(2);
        enviroment.Release();
        yield return new WaitForSeconds(5);
    }
}
