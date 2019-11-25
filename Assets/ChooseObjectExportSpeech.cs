using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class ChooseObjectExportSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;
    public String first;
    public String second;
    public String third;
    public String editScene = "FullSculpt";

    void Start()
    {
        GameObject modelObj = GameObject.FindWithTag("ModelObject");
        modelObj.AddComponent<ObjExporter>();
        modelObj.GetComponent<ObjExporter>().exportObject = modelObj;
        modelObj.AddComponent<displayObjectModel>();

        keywordsModo = new string[3];
        keywordsModo[0] = "Um";
        keywordsModo[1] = "Dois";
        keywordsModo[2] = "Três";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Um")
            {
                SaveObject(first);
            }
            else if (args.text == "Dois")
            {
                SaveObject(second);
            }
            else if (args.text == "Três")
            {
                SaveObject(third);
            }
    }
    private void SaveObject(String workingPath)
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
        StartCoroutine(LoadYourAsyncScene(editScene));
    }

    private IEnumerator LoadYourAsyncScene(String sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}