using Dummiesman;
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
public class displayImportObject : MonoBehaviour
{
    public string objName = string.Empty;
    private string objPath;
    string error = string.Empty;
    public GameObject loadedObject;

    void Start()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        objPath = Path.Combine(desktopPath, objName);
        Debug.Log(objPath);
        //file path
        if (!File.Exists(objPath))
        {
            Debug.Log("File doesn't exist.");
        }
        else
        {
            if (loadedObject != null)
                Destroy(loadedObject);

            loadedObject = new OBJLoader().Load(objPath);
            loadedObject.transform.parent = this.gameObject.transform.parent;
            GameObject newObject = loadedObject.transform.GetChild(0).gameObject;
            newObject.AddComponent<MeshCollider>();

            newObject.GetComponent<MeshCollider>().convex = true;
            newObject.GetComponent<MeshCollider>().isTrigger = true;
            newObject.AddComponent<StandardObject>();

            newObject.AddComponent<BoxCollider>();
            newObject.GetComponent<BoxCollider>().isTrigger = true;
            newObject.tag = "ModelObject";
            setChooseObjectImportSpeech(newObject);

            loadedObject.transform.position = this.transform.position;
            loadedObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            error = string.Empty;
        }
    }

    private void Update()
    {
        if (loadedObject)
        {
            loadedObject.transform.Rotate(0, 50 * Time.deltaTime, 0);
        }
    }

    public void reload()
    {
        if (loadedObject != null)
        {
            Destroy(loadedObject);
        }

        loadedObject = new OBJLoader().Load(objPath);
        loadedObject.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
        loadedObject.transform.position = this.transform.position + new Vector3(0, 2, 0);
        loadedObject.transform.localScale = new Vector3(1, 1, 1);
        error = string.Empty;
    }

    private void setChooseObjectImportSpeech(GameObject newObject)
    {
        if (objName == "teste.obj")
        {
            GameObject menuObject = loadedObject.transform.root.gameObject;
            menuObject.GetComponent<ChooseObjectImportSpeech>().first = newObject;
        }
        else if (objName == "teste2.obj")
        {
            GameObject menuObject = loadedObject.transform.root.gameObject;
            menuObject.GetComponent<ChooseObjectImportSpeech>().second = newObject;
        }
        else if (objName == "teste3.obj")
        {
            GameObject menuObject = loadedObject.transform.root.gameObject;
            menuObject.GetComponent<ChooseObjectImportSpeech>().third = newObject;
        }
        else
        {
            Debug.Log("displayImportObject: speech for import won't work, check the obj names. Must be either teste.obj, teste2.obj or teste3.obj.");
        }

    }
}
