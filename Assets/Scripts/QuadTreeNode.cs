using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    public float size, halfSize;
    public Vector2 origin;

    int objectLimit;
    float minSize;

    QuadTreeNode TopLeft;
    QuadTreeNode TopRight;
    QuadTreeNode BottomLeft;
    QuadTreeNode BottomRight;

    List<GameObject> objects = new List<GameObject>();

    Color objectColor = new Color(Random.Range(0.3f,3f), Random.Range(0.3f, 3f), Random.Range(0.3f, 3f));

    public QuadTreeNode(float size, Vector2 coords, int objectLimit, float minSize)
    {
        this.size = size;
        halfSize = size/2;
        this.origin = coords;
        this.objectLimit = objectLimit;
        this.minSize = minSize;

        Draw();
    }

    
    void Draw() 
    {
        float endX = origin.x + size;
        float endY = origin.y + size;
        
        Vector2 topRight = new Vector2(endX, origin.y);
        Vector2 bottomRight = new Vector2(endX, endY);
        Vector2 bottomLeft = new Vector2(origin.x, endY);

        Debug.DrawLine(origin, topRight, Color.green, 100f);
        Debug.DrawLine(topRight, bottomRight, Color.green, 100f);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green, 100f);
        Debug.DrawLine(bottomLeft, origin, Color.green, 100f);

        if(BottomLeft != null) {
            TopLeft.Draw();
            TopRight.Draw();
            BottomLeft.Draw();
            BottomRight.Draw();
        }
    }

    private QuadTreeNode GetQuadrant(Vector3 location) {
        if(location.x > origin.x + halfSize) 
        {
            if(location.y > origin.y + halfSize) 
            {
                return(TopRight);
            }
            else 
            {
                return(BottomRight);
            }
        }
        else // Left
        {
            if(location.y > origin.y + halfSize)
            {
                return(TopLeft);
            }
            else
            {
                return(BottomLeft);
            }
        }
    }

    public void Add(GameObject obj)
    {
        // Check if it's been split
        if(BottomLeft != null) 
        {
            QuadTreeNode quad = GetQuadrant(obj.transform.position);
            quad.Add(obj);
        }
        else 
        {
            obj.GetComponent<Renderer>().material.SetColor("_Color", objectColor);

            objects.Add(obj);

            if(objects.Count > objectLimit && size/2 >= minSize) {
                Split();
            }
        }
    }

    public void Remove(GameObject obj) 
    {
        if(BottomLeft != null) {
            QuadTreeNode quad = GetQuadrant(obj.transform.position);
            quad.Remove(obj);
        }
        else {
            objects.Remove(obj);
        }   
    }

    private void Split() {
        float newSize = size / 2;
        float midX = origin.x + newSize;
        float midY = origin.y + newSize;

        TopLeft = new QuadTreeNode(newSize, new Vector2(origin.x, midY), objectLimit, minSize);
        TopRight = new QuadTreeNode(newSize, new Vector2(midX, midY), objectLimit, minSize);
        BottomLeft = new QuadTreeNode(newSize, new Vector2(origin.x, origin.y), objectLimit, minSize);
        BottomRight = new QuadTreeNode(newSize, new Vector2(midX, origin.y), objectLimit, minSize);

        while(objects.Count > 0) {
            GameObject obj = objects[0];
            objects.RemoveAt(0);
            Add(obj);
        }
    }

    private void Join() {
        
    }
}