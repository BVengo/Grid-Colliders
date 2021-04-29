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
    float minSize = 0.5f;

    [SerializeField]
    float objectSize = 0.05f;

    [SerializeField]
    int numObjects = 1000;

    void Start() 
    {
        top = new OctTreeNode(Mathf.Pow(2, initialSize), transform.position, objectLimit, minSize);

        for(int i = 0; i < numObjects; i++) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.localScale = new Vector3(objectSize, objectSize, objectSize);
            sphere.transform.position = new Vector3(Random.Range(top.origin.x, top.size), Random.Range(top.origin.y, top.size), Random.Range(top.origin.z, top.size));

            Add(sphere);
        }
    }

    void Update()
    {
        top.Draw();
        
        if(Input.GetKeyDown("space"))
        {
            top.Remove();
        }
    }

    public void Add(GameObject obj)
    {
        top.Add(obj);
    }
}
