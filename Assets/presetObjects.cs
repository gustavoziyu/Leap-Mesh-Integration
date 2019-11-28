using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    public class presetObjects : MonoBehaviour
    {
        private void Awake()
        {
            GameObject target = GameObject.FindWithTag("ModelObject");
            if (target == null)
                Debug.Log("Model object not found! Make sure you have a object with the tag ModelObject on the scene.");
            else
            {
                GetComponent<RotateModel>().target = target;
                GetComponent<ResizeModel>().target = target;
                target.GetComponent<MeshCollider>().isTrigger = false;
                target.GetComponent<MeshCollider>().convex = false;
                target.transform.position = new Vector3(0, 0, 0);
                target.transform.rotation = new Quaternion(0, 0, 0, 0);
                if (target.name == "Revolution" || target.name == "Extrusion") ;
                //target.transform.localScale = new Vector3(10, 10, 10);
                else
                    target.transform.localScale = new Vector3(2, 2, 2);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}