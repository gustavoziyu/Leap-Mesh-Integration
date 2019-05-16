using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
 
public class KeywordScript : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;

    void Start()
    {
        keywordsModo = new string[3];
        keywordsModo[0] = "Rotacionar";
        keywordsModo[1] = "Deformar";
        keywordsModo[2] = "Voltar";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == keywordsModo[0])
            {
                Debug.Log("Modo " + keywordsModo[0]);
            } else if (args.text == keywordsModo[1])
            {
                Debug.Log("Modo " + keywordsModo[1]);
            } else if (args.text == keywordsModo[2])
            {
                Debug.Log("Modo" + keywordsModo[2]);
            }
    }

}