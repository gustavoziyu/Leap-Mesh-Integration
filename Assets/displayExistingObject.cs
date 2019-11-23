﻿using Dummiesman;
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
public class displayExistingObject : MonoBehaviour
{
    public string objName = string.Empty;
    private string objPath;
    string error = string.Empty;
    public GameObject loadedObject;
    private float maxSizeXZ = 0.3f;
    private float maxSizeY = 0.25f;

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
            newObject.AddComponent<ExportObject>();
            newObject.GetComponent<ExportObject>().workingPath = objName;

            newObject.AddComponent<BoxCollider>();
            newObject.GetComponent<BoxCollider>().isTrigger = true;
            newObject.tag = "ModelObject";
            setChooseObjectExportSpeech();

            loadedObject.transform.position = this.transform.position;
            loadedObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            error = string.Empty;

            resizeObject(newObject);
        }
    }

    private void Update()
    {
        if (loadedObject)
        {
            loadedObject.transform.Rotate(0, 50 * Time.deltaTime, 0);
        }
    }

    private void resizeObject(GameObject newObject)
    {
        Vector3 sizeOfObject = newObject.GetComponent<Renderer>().bounds.size;

        if(sizeOfObject.x > maxSizeXZ && sizeOfObject.x >= sizeOfObject.y && sizeOfObject.x >= sizeOfObject.z)
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
    private void setChooseObjectExportSpeech()
    {
        if (objName == "teste.obj")
        {
            GameObject menuObject = loadedObject.transform.root.gameObject;
            menuObject.GetComponent<ChooseObjectExportSpeech>().first = objName;
        }
        else if (objName == "teste2.obj")
        {
            GameObject menuObject = loadedObject.transform.root.gameObject;
            menuObject.GetComponent<ChooseObjectExportSpeech>().second = objName;
        }
        else if (objName == "teste3.obj")
        {
            GameObject menuObject = loadedObject.transform.root.gameObject;
            menuObject.GetComponent<ChooseObjectExportSpeech>().third = objName;
        }
        else
        {
            Debug.Log("displayExistingObject: speech for export won't work, check the obj names. Must be either teste.obj, teste2.obj or teste3.obj.");
        }

    }
}
