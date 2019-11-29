using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class ChooseLearnModelSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject initial;
    public string editScene = "FullSculpt";

    void Start()
    {
        keywordsModo = new string[3];
        keywordsModo[0] = "Primeiro";
        keywordsModo[1] = "Segundo";
        keywordsModo[2] = "Terceiro";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
        GameObject.Find("Initial").SetActive(false);
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Primeiro")
            {
                SaveObject(first);
            }
            else if (args.text == "Segundo")
            {
                SaveObject(second);
            }
            else if (args.text == "Terceiro")
            {
                SaveObject(third);
            }
    }
    private void SaveObject(GameObject toSaveObject)
    {
        toSaveObject.transform.parent = null;
        Destroy(toSaveObject.GetComponent<LearnModel>());
        Destroy(toSaveObject.GetComponent<BoxCollider>());
        toSaveObject.GetComponent<MeshCollider>().convex = true;
        toSaveObject.GetComponent<MeshCollider>().isTrigger = true;
        toSaveObject.transform.DetachChildren();
        DontDestroyOnLoad(toSaveObject);

        GameObject initialInstance = Instantiate(initial, new Vector3(1, 1, 1), Quaternion.identity);
        DontDestroyOnLoad(initialInstance);

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