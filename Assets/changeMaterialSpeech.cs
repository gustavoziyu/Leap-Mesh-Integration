using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class ChangeMaterialSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;
    public string sculptScene = "FullSculpt";
    public string exportScene = "Exportar objeto";

    void Start()
    {
        keywordsModo = new string[3];
        keywordsModo[0] = "Esculpir";
        keywordsModo[2] = "Exportar";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
        //GameObject target = GameObject.FindWithTag("ModelObject");
        //DontDestroyOnLoad(target);
        //StartCoroutine(LoadYourAsyncScene(materialScene));
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Esculpir")
            {
                GameObject target = GameObject.FindWithTag("ModelObject");
                DontDestroyOnLoad(target);
                StartCoroutine(LoadYourAsyncScene(sculptScene));
            }
            else if (args.text == "Exportar")
            {
                GameObject target = GameObject.FindWithTag("ModelObject");
                DontDestroyOnLoad(target);
                StartCoroutine(LoadYourAsyncScene(exportScene));
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

        {
            yield return null;
        }
    }

}