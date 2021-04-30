using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctTree : MonoBehaviour
{
    OctTreeNode top;

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
        top = new OctTreeNode(nodeSize, transform.position, objectLimit, minSize);

        for(int i = 0; i < numObjects; i++) {
            Vector3 objLocation =  new Vector3(Random.Range(transform.position.x, nodeSize), Random.Range(transform.position.y, nodeSize), Random.Range(transform.position.z, nodeSize));
            gridObject = Instantiate(gridObject, objLocation, Quaternion.identity);

            Add(gridObject);
        }
    }

    void Update()
    {
        top.Draw();
        
        if(Input.GetKeyDown("delete"))
        {
            top.Remove();
        }
    }

    public void Add(GameObject obj)
    {
        top.Add(obj);
    }
}
