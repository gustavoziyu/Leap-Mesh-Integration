using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerImpExp : MonoBehaviour
{
    public GameObject loadSelection, loadOne, loadTwo, loadThree;
    private GameObject workingObject;
    private string workingPath;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            workingObject = Instantiate(loadOne.GetComponent<displayImportObject>().loadedObject);
            workingPath = loadOne.GetComponent<displayImportObject>().objName;
            workingObject.transform.position = new Vector3(0, 1, -7);
            workingObject.AddComponent<ObjExporter>();
            workingObject.GetComponent<ObjExporter>().exportObject = workingObject.transform.GetChild(0).gameObject;
            workingObject.GetComponent<ObjExporter>().exportName = workingPath;
            hideLoadSelection();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("called export");
            workingObject.GetComponent<ObjExporter>().exportCall();
            workingObject.SetActive(false);
            showLoadSelection();
            loadOne.GetComponent<displayImportObject>().reload();
        }
    }

    void hideLoadSelection()
    {
        loadOne.GetComponent<displayImportObject>().loadedObject.SetActive(false);
        loadTwo.GetComponent<displayImportObject>().loadedObject.SetActive(false);
        loadThree.GetComponent<displayImportObject>().loadedObject.SetActive(false);
        loadSelection.SetActive(false);

    }

    void showLoadSelection()
    {
        loadOne.GetComponent<displayImportObject>().loadedObject.SetActive(true);
        loadTwo.GetComponent<displayImportObject>().loadedObject.SetActive(true);
        loadThree.GetComponent<displayImportObject>().loadedObject.SetActive(true);
        loadSelection.SetActive(true);

    }


}
