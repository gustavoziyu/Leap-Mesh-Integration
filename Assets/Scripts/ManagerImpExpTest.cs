using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerImpExpTest : MonoBehaviour
{
    public GameObject objectToExport;
    public string testSubject;
    private string workingPath;


    // Start is called before the first frame update
    void Start()
    {
        objectToExport.AddComponent<ObjExporter>();
        objectToExport.GetComponent<ObjExporter>().exportObject = objectToExport;
        objectToExport.GetComponent<ObjExporter>().exportName = testSubject + System.DateTime.Now.ToString("yyyyMMddHHmm") + ".obj";
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("called export");
            objectToExport.GetComponent<ObjExporter>().exportCall();
        }
    }


}
