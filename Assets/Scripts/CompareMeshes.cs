/* Calcula a distancia de haussdorf entre duas meshes para determianr sua similaridade */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareMeshes : MonoBehaviour
{
    private double haussdorfDistance;
    public GameObject objRef, objComp;
    // Start is called before the first frame update
    void Start()
    {
        haussdorfDistance = compareMeshes();
        Debug.Log("h" + haussdorfDistance);
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
                Debug.Log(vtr + " " + vtc);
                if (dist < shortest) shortest = dist;
            }
            if (shortest > h) h = shortest;
        }
        return h;
    }
}
