﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class chooseStandardObjectSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;
    public GameObject cube;
    public GameObject sphere;
    public GameObject capsule;
    public string editScene = "FullSculpt";

    void Start()
    {
        keywordsModo = new string[3];
        keywordsModo[0] = "Cubo";
        keywordsModo[1] = "Esfera";
        keywordsModo[2] = "Cápsula";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Cubo")
            {
                SaveObject(cube);
            }
            else if (args.text == "Esfera")
            {
                SaveObject(sphere);
            }
            else if (args.text == "Cápsula")
            {
                SaveObject(capsule);
            }
    }
    private void SaveObject(GameObject toSaveObject)
    {
        toSaveObject.transform.parent = null;
        Destroy(toSaveObject.GetComponent<StandardObject>());
        Destroy(toSaveObject.GetComponent<BoxCollider>());
        toSaveObject.GetComponent<MeshCollider>().convex = true;
        toSaveObject.GetComponent<MeshCollider>().isTrigger = true;
        DontDestroyOnLoad(toSaveObject);
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