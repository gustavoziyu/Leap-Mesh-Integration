using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class EditExtrudedModelSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;
    public GameObject extrudedObject;
    public string editScene = "FullSculpt";

    void Start()
    {
        keywordsModo = new string[1];
        keywordsModo[0] = "Editar";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Editar")
            {
                if (extrudedObject == null)
                    Debug.Log("Must define the object first.");
                else
                    SaveObject();
            }
    }
    private void SaveObject()
    {
        extrudedObject.transform.parent = null;
        extrudedObject.tag = "ModelObject";
        Destroy(extrudedObject.GetComponent<Triangulator>());

        DontDestroyOnLoad(extrudedObject);
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