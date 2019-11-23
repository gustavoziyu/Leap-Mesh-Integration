using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class LearnSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;

    void Start()
    {
        keywordsModo = new string[3];
        keywordsModo[0] = "Fácil";
        keywordsModo[1] = "Médio";
        keywordsModo[2] = "Difícil";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Fácil")
            {
                StartCoroutine(LoadYourAsyncScene("Escolher objeto inicial"));
            }
            else if (args.text == "Médio")
            {
                StartCoroutine(LoadYourAsyncScene("Escolher objeto inicial"));
            }
            else if (args.text == "Difícil")
            {
                StartCoroutine(LoadYourAsyncScene("Escolher objeto inicial"));
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