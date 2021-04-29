using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : MonoBehaviour
{
    QuadTreeNode top;

    [SerializeField]
    int objectLimit = 10;

    [SerializeField]
    float initialSize = 8;

    [SerializeField]
    float minSize = 0.5f;

    [SerializeField]
    float objectSize = 0.05f;

    [SerializeField]
    int numObjects = 1000;

    void Start() 
    {
        top = new QuadTreeNode(initialSize, transform.position, objectLimit, minSize);

        for(int i = 0; i < numObjects; i++) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.localScale = new Vector3(objectSize, objectSize, objectSize);
            sphere.transform.position = new Vector3(Random.Range(top.origin.x, top.size), Random.Range(top.origin.y, top.size), 0);

            top.Add(sphere);
        }
    }

    public void Add(GameObject obj)
    {
        top.Add(obj);
    }
}
