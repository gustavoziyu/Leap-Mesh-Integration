using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetFullSculpt : MonoBehaviour
{
    void Awale()
    {
        GameObject target = GameObject.FindWithTag("ModelObject");
        target.GetComponent<MeshCollider>().isTrigger = false;
        target.GetComponent<MeshCollider>().convex = false;
        target.transform.position = new Vector3(-2.867f, 0, 0);
        target.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (target.name == "Revolution" || target.name == "Extrusion")
            target.transform.localScale = new Vector3(10, 10, 10);
        else
            target.transform.localScale = new Vector3(2, 2, 2);
        GetComponent<Leap.Unity.RotateModelTouch>().target = target;
        GetComponent<Leap.Unity.ResizeModelTouch>().target = target;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
