using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExportObject : MonoBehaviour
{
    public string workingPath;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject modelObj = GameObject.FindWithTag("ModelObject");
        ObjExporter objExp = modelObj.GetComponent<ObjExporter>();
        objExp.exportName = workingPath;
        modelObj.transform.localScale = modelObj.GetComponent<displayObjectModel>().previousSize;
        objExp.exportCall();

        modelObj.transform.parent = null;
        Destroy(modelObj.GetComponent<BoxCollider>());
        Destroy(modelObj.GetComponent<ObjExporter>());
        Destroy(modelObj.GetComponent<displayObjectModel>());

        DontDestroyOnLoad(modelObj);
        StartCoroutine(LoadYourAsyncScene());
    }
    private IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FullSculpt");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
