using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    OctTree worldTree;

    [SerializeField]
    int objectLimit = 10;

    [SerializeField]
    int initialSize = 1;

    [SerializeField]
    int minSize = 1;

    [SerializeField]
    int numObjects = 1000;

    [SerializeField]
    GameObject gridObject;

    void Start() 
    {
        int nodeSize = (int)Mathf.Pow(2, initialSize);

        worldTree = new OctTree(transform.position, nodeSize, minSize, objectLimit);

        for(int i = 0; i < numObjects; i++) {
            Vector3 objLocation =  new Vector3(Random.Range(transform.position.x, nodeSize), Random.Range(transform.position.y, nodeSize), Random.Range(transform.position.z, nodeSize));
            gridObject = Instantiate(gridObject, objLocation, Quaternion.identity);

            worldTree.Add(gridObject);
        }
    }

    void Update()
    {       
        worldTree.Draw();
        
        if(Input.GetKeyDown("delete"))
        {
            worldTree.Remove();
        }
    }
}
