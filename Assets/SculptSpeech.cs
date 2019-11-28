using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class SculptSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;
    public string materialScene = "ChangeMaterial";
    public string compareScene = "CompareMeshes";

    void Start()
    {
        keywordsModo = new string[2];
        keywordsModo[0] = "Mudar material";
        keywordsModo[1] = "Avaliar";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Mudar material")
            {
                GameObject target = GameObject.FindWithTag("ModelObject");
                DontDestroyOnLoad(target);
                StartCoroutine(LoadYourAsyncScene(materialScene));
            }
            else if (args.text == "Avaliar")
            {
                GameObject goal = GameObject.FindWithTag("GoalModel");
                if (goal == null)
                {
                    Debug.Log("Can't find the goal model object. Did you enter the learn mode?");
                }
                else
                {
                    GameObject target = GameObject.FindWithTag("ModelObject");

                    DontDestroyOnLoad(target);
                    DontDestroyOnLoad(goal);
                    StartCoroutine(LoadYourAsyncScene(compareScene));
                }
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