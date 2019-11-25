using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class ChooseObjectSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;

    public string standardObjectScene = "Objeto padrão";
    public string extrusionScene = "First Integration Extrusion+CreatePlane";
    public string revolutionScene = "Objeto padrão";
    public string importScene = "Importar objeto";

    void Start()
    {
        keywordsModo = new string[4];
        keywordsModo[0] = "Template";
        keywordsModo[1] = "Extrusão";
        keywordsModo[2] = "Revolução";
        keywordsModo[3] = "Importar";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Template")
            {
                StartCoroutine(LoadYourAsyncScene(standardObjectScene));
            }
            else if (args.text == "Extrusão")
            {
                StartCoroutine(LoadYourAsyncScene(extrusionScene));
            }
            else if (args.text == "Revolução")
            {
                Debug.Log("Modo " + keywordsModo[2]);
            }
            else if (args.text == "Importar")
            {
                StartCoroutine(LoadYourAsyncScene(importScene));
            }
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