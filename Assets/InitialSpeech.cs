using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class InitialSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;

    public string learnScene = "Escolher dificuldade";
    public string createScene = "Escolher objeto inicial";

    void Start()
    {
        keywordsModo = new string[2];
        keywordsModo[0] = "Aprender";
        keywordsModo[1] = "Criar";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Aprender")
            {
                StartCoroutine(LoadYourAsyncScene(learnScene));
            }
            else if (args.text == "Criar")
            {
                StartCoroutine(LoadYourAsyncScene(createScene));
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