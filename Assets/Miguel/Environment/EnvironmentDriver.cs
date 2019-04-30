using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDriver : MonoBehaviour, IEnvironmentController
{
    [Tooltip("")]
    [SerializeField]
    private GameObject cubePrefab;

    [Tooltip("")]
    [SerializeField]
    private int size = 4;

    [Tooltip("")]
    [SerializeField]
    private int height = 5;

    private List<GameObject> cubes = null;
    private int state = 0;
    bool IEnvironmentController.Initialize()
    {
        Debug.Log("Initializing Blocks.");
        float _offset = size/2;
        cubes = new List<GameObject>(size * size);

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                GameObject _cube = Instantiate(cubePrefab, new Vector3(i - _offset, height, j - _offset), Quaternion.identity, this.transform);
                cubes.Add(_cube);
            }
        }

        return true;
    }

    bool IEnvironmentController.Paint()
    {
        Debug.Log("Painting Blocks.");
        if (state >= cubes.Count) return false;

        Renderer _cubeRenderer = cubes[state].GetComponent<Renderer>();

        MaterialPropertyBlock _materialBlock = new MaterialPropertyBlock();
        int id = Shader.PropertyToID("_Color");
        _materialBlock.SetColor(id, Color.red);
        _cubeRenderer.SetPropertyBlock(_materialBlock);

        return true;
    }

    bool IEnvironmentController.Release()
    {
        Debug.Log("Releasing Blocks.");
        if (state >= cubes.Count) return false;

        Rigidbody _rigidbody = cubes[state].GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;

        ++state;
        return true;
    }
}
