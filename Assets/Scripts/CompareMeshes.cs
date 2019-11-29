/* Calcula a distancia de haussdorf entre duas meshes para determianr sua similaridade */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareMeshes : MonoBehaviour
{
    public GameObject objRef, objComp;
    private float maxSizeXZ = 0.3f;
    private float maxSizeY = 0.2f;

    private void Awake()
    {
        objComp = GameObject.FindWithTag("ModelObject");
        objRef = GameObject.FindWithTag("GoalModel");
        if(objComp == null || objRef == null)
        {
            Debug.Log("Object not found! Make sure you have a object with the tag ModelObject and other with GoalModel on the scene.");
        }
        else
        {
            objRef.transform.position = new Vector3(0, -0.09f, 0.48f);
            objRef.transform.localScale = objComp.transform.localScale;
            resizeObject(objRef);

            objRef.transform.rotation = Quaternion.identity;
            objComp.transform.position = new Vector3(0, -0.09f, 0.48f);
            resizeObject(objComp);
            //objComp.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            GetComponent<Leap.Unity.RotateModel>().target = objComp;

            Debug.Log(objRef.GetComponent<Renderer>().bounds.size);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double compareMeshes()
    {
        double h = 0;
        double shortest, dist;

        Mesh mr = objRef.GetComponent<MeshFilter>().sharedMesh;
        Mesh mc = objComp.GetComponent<MeshFilter>().sharedMesh;
        Transform tr = objRef.transform;
        Transform tc = objComp.transform;

        Vector3 vtr, vtc;

        foreach (Vector3 vr in mr.vertices)
        {
            shortest = 9999999999;
            foreach (Vector3 vc in mc.vertices)
            {
                vtr = tr.TransformPoint(vr);
                vtc = tc.TransformPoint(vc);
                dist = Vector3.Distance(vtr, vtc);
                if (dist < shortest) shortest = dist;
            }
            if (shortest > h) h = shortest;
        }

        return h;
    }

    public float avaliate()
    {
        return Mathf.Max(0, 10 - (float)compareMeshes());
    }

    private void resizeObject(GameObject newObject)
    {
        Vector3 sizeOfObject = newObject.GetComponent<Renderer>().bounds.size;

        if (sizeOfObject.x > maxSizeXZ && sizeOfObject.x >= sizeOfObject.y && sizeOfObject.x >= sizeOfObject.z)
        {
            float newScaleFactor = sizeOfObject.x / maxSizeXZ;
            newObject.transform.localScale = newObject.transform.localScale / newScaleFactor;
        }
        else if (sizeOfObject.y > maxSizeY && sizeOfObject.y >= sizeOfObject.x && sizeOfObject.y >= sizeOfObject.z)
        {
            float newScaleFactor = sizeOfObject.y / maxSizeY;
            newObject.transform.localScale = newObject.transform.localScale / newScaleFactor;
        }
        else if (sizeOfObject.z > maxSizeXZ && sizeOfObject.z >= sizeOfObject.x && sizeOfObject.x >= sizeOfObject.y)
        {
            float newScaleFactor = sizeOfObject.z / maxSizeXZ;
            newObject.transform.localScale = newObject.transform.localScale / newScaleFactor;
        }
    }
}
