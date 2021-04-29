using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctTreeNode
{
    public float size, halfSize;
    public Vector3 origin;

    int objectLimit;
    float minSize;

    OctTreeNode BottomLeftBack;
    OctTreeNode TopLeftBack;
    OctTreeNode TopRightBack;
    OctTreeNode BottomRightBack;

    OctTreeNode BottomLeftFront;
    OctTreeNode BottomRightFront;
    OctTreeNode TopRightFront;
    OctTreeNode TopLeftFront;

    public List<GameObject> objects = new List<GameObject>();

    Color objectColor = new Color(Random.Range(0.3f,3f), Random.Range(0.3f, 3f), Random.Range(0.3f, 3f));

    public OctTreeNode(float size, Vector3 coords, int objectLimit, float minSize)
    {
        this.size = size;
        halfSize = size/2;
        this.origin = coords;
        this.objectLimit = objectLimit;
        this.minSize = minSize;
    }

    
    public void Draw() {
        Vector3 end = new Vector3(origin.x + size, origin.y + size, origin.z + size);
        
        Vector3 bottomLeftBackVertex = origin;
        Vector3 topLeftBackVertex = new Vector3(origin.x, end.y, origin.z);
        Vector3 topRightBackVertex = new Vector3(end.x, end.y, origin.z);
        Vector3 bottomRightBackVertex = new Vector3(end.x, origin.y, origin.z);
        
        Vector3 bottomLeftFrontVertex = new Vector3(origin.x, origin.y, end.z);
        Vector3 topLeftFrontVertex = new Vector3(origin.x, end.y, end.z);
        Vector3 topRightFrontVertex = end;
        Vector3 bottomRightFrontVertex = new Vector3(end.x, origin.y, end.z);

        Debug.DrawLine(bottomLeftBackVertex, topLeftBackVertex, Color.black, 0f);
        Debug.DrawLine(topLeftBackVertex, topRightBackVertex, Color.black, 0f);
        Debug.DrawLine(topRightBackVertex, bottomRightBackVertex, Color.black, 0f);
        Debug.DrawLine(bottomRightBackVertex, bottomLeftBackVertex, Color.black, 0f);

        Debug.DrawLine(bottomLeftFrontVertex, topLeftFrontVertex, Color.black, 0f);
        Debug.DrawLine(topLeftFrontVertex, topRightFrontVertex, Color.black, 0f);
        Debug.DrawLine(topRightFrontVertex, bottomRightFrontVertex, Color.black, 0f);
        Debug.DrawLine(bottomRightFrontVertex, bottomLeftFrontVertex, Color.black, 0f);

        Debug.DrawLine(bottomLeftBackVertex, bottomLeftFrontVertex, Color.black, 0f);
        Debug.DrawLine(topLeftBackVertex, topLeftFrontVertex, Color.black, 0f);
        Debug.DrawLine(topRightBackVertex, topRightFrontVertex, Color.black, 0f);
        Debug.DrawLine(bottomRightBackVertex, bottomRightFrontVertex, Color.black, 0f);

        if(BottomLeftBack != null) {
            BottomLeftBack.Draw();
            TopLeftBack.Draw();
            TopRightBack.Draw();
            BottomRightBack.Draw();

            BottomLeftFront.Draw();
            TopLeftFront.Draw();
            TopRightFront.Draw();
            BottomRightFront.Draw();
        }
    }

    private OctTreeNode GetOctant(Vector3 location) {
        if(location.x > origin.x + halfSize) 
        {
            if(location.y > origin.y + halfSize) 
            {
                if(location.z > origin.z + halfSize) {
                    return(TopRightFront);
                }
                else
                {
                    return(TopRightBack);
                }
            }
            else 
            {
                if(location.z > origin.z + halfSize) {
                    return(BottomRightFront);
                }
                else
                {
                    return(BottomRightBack);
                }
            }
        }
        else // Left
        {
            if(location.y > origin.y + halfSize)
            {
                if(location.z > origin.z + halfSize) {
                    return(TopLeftFront);
                }
                else
                {
                    return(TopLeftBack);
                }
            }
            else
            {
                if(location.z > origin.z + halfSize) {
                    return(BottomLeftFront);
                }
                else
                {
                    return(BottomLeftBack);
                }
            }
        }
    }

    public void Add(GameObject obj) {
        // Check if it's been split
        if(BottomLeftBack != null) 
        {
            OctTreeNode oct = GetOctant(obj.transform.position);
            oct.Add(obj);
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

    public bool HasSplit() {
        return(BottomLeftBack != null);
    }

    public bool Remove() {
        bool checkMerge = false;

        // Check if it's been split
        if(BottomLeftBack != null) {
            OctTreeNode oct;

            int octNum = Random.Range(0, 8);
            do {
                switch(octNum)
                {
                    case 0:
                        oct = BottomLeftBack;
                        break;
                    case 1:
                        oct = TopLeftBack;
                        break;
                    case 2:
                        oct = TopRightBack;
                        break;
                    case 3:
                        oct = BottomRightBack;
                        break;
                    case 4:
                        oct = BottomLeftFront;
                        break;
                    case 5:
                        oct = BottomRightFront;
                        break;
                    case 6:
                        oct = TopRightFront;
                        break;
                    default:
                        oct = TopLeftFront;
                        break;
                }
            }
            while(!oct.HasSplit() && oct.GetCount() == 0);

            checkMerge = oct.Remove();
        }
        else 
        {
            if(objects.Count == 0) {
                return(false);
            }

            int objNum = Random.Range(0, objects.Count);
            UnityEngine.Object.Destroy(objects[objNum]);
            objects.RemoveAt(objNum);

            return(true);
        }

        // Join again if sum of all octants < limit/2
        if(checkMerge && GetCount() <= objectLimit / 8) {
            Merge();
        }

        return(false);
        
    }

    public int GetCount() {
        int count = objects.Count;

        if(BottomLeftBack != null) {
            count += BottomLeftBack.GetCount();
            count += TopLeftBack.GetCount();
            count += TopRightBack.GetCount();
            count += BottomRightBack.GetCount();
            count += BottomLeftFront.GetCount();
            count += BottomRightFront.GetCount();
            count += TopRightFront.GetCount();
            count += TopLeftFront.GetCount();
        }
        
        return count;
    }

    private void Split() {
        Vector3 mid = new Vector3(origin.x + halfSize, origin.y + halfSize, origin.z + halfSize);

        BottomLeftBack = new OctTreeNode(halfSize, origin, objectLimit, minSize);
        TopLeftBack = new OctTreeNode(halfSize, new Vector3(origin.x, mid.y, origin.z), objectLimit, minSize);
        TopRightBack = new OctTreeNode(halfSize, new Vector3(mid.x, mid.y, origin.z), objectLimit, minSize);
        BottomRightBack = new OctTreeNode(halfSize, new Vector3(mid.x, origin.y, origin.z), objectLimit, minSize);

        BottomLeftFront = new OctTreeNode(halfSize, new Vector3(origin.x, origin.y, mid.z), objectLimit, minSize);
        TopLeftFront = new OctTreeNode(halfSize, new Vector3(origin.x, mid.y, mid.z), objectLimit, minSize);
        TopRightFront = new OctTreeNode(halfSize, mid, objectLimit, minSize);
        BottomRightFront = new OctTreeNode(halfSize, new Vector3(mid.x, origin.y, mid.z), objectLimit, minSize);

        while(objects.Count > 0) {
            GameObject obj = objects[0];
            objects.RemoveAt(0);
            Add(obj);
        }
    }

    private void Merge() {
        MergeOctant(BottomLeftBack);
        MergeOctant(TopLeftBack);
        MergeOctant(TopRightBack);
        MergeOctant(BottomRightBack);

        MergeOctant(BottomLeftFront);
        MergeOctant(TopLeftFront);
        MergeOctant(TopRightFront);
        MergeOctant(BottomRightFront);
    }

    private void MergeOctant(OctTreeNode oct) {
        List<GameObject> octObjects = oct.GetObjects();

        oct = null;

        while(octObjects.Count > 0) {
            GameObject obj = octObjects[0];
            octObjects.RemoveAt(0);
            Add(obj);
        }
    }

    public List<GameObject> GetObjects() {
        List<GameObject> allObjects = new List<GameObject>();
        allObjects.AddRange(objects);
        
        if(BottomLeftBack != null) {
            allObjects.AddRange(BottomLeftBack.GetObjects());
            allObjects.AddRange(TopLeftBack.GetObjects());
            allObjects.AddRange(TopRightBack.GetObjects());
            allObjects.AddRange(BottomRightBack.GetObjects());
            allObjects.AddRange(BottomLeftFront.GetObjects());
            allObjects.AddRange(TopLeftFront.GetObjects());
            allObjects.AddRange(TopRightFront.GetObjects());
            allObjects.AddRange(BottomRightFront.GetObjects());
        }

        return(allObjects);
    }
}