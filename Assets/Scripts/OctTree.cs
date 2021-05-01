using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctTree
{
    OctTreeNode top;

    int objectLimit;
    int minSize;

    public OctTree(Vector3 origin, int startSize, int minSize, int objectLimit)
    {
        top = new OctTreeNode(origin, startSize, minSize, objectLimit);
    }

    public void Draw()
    {
        top.Draw();
    }

    public void Add(GameObject obj)
    {
        top.Add(obj);
    }

    public void Remove()
    {
        top.Remove();
    }

    public void Expand(Vector3 location)
    {
        //expand so that the coordinates fit inside the OctTree
    }
}
