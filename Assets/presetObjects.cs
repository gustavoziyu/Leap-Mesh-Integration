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
                    target.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            GameObject goal = GameObject.FindWithTag("GoalModel");
            if (goal != null)
            {
                goal.GetComponent<MeshCollider>().isTrigger = false;
                goal.GetComponent<MeshCollider>().convex = false;
                goal.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                goal.transform.position = GameObject.Find("GoalModelPosition").transform.position;
                goal.transform.rotation = new Quaternion(0, 0, 0, 0);
                goal.AddComponent<RotateModelDisplay>();
            }
            else
            {
                GameObject.Find("Spot Light").SetActive(false);
                GameObject.Find("Spot Light 2").SetActive(false);
            }
                
           
            }

        // Update is called once per frame
        void Update()
        {

        }

        private void placeObjectInCorner(GameObject myObj)
        {
            Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
            Vector3 sizeOfObject = myObj.GetComponent<Renderer>().bounds.size;
            Vector3 upperLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            upperLeft.x = upperLeft.x + sizeOfObject.x / 2;
            upperLeft.y = upperLeft.y + sizeOfObject.y / 2;
            upperLeft.z = upperLeft.z + sizeOfObject.z / 2;

            myObj.transform.position = upperLeft;

        }
    }
}