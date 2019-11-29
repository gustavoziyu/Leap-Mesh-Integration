using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

public class CalculateGradeSpeech : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;

    void Start()
    {
        keywordsModo = new string[1];
        keywordsModo[0] = "Calcular";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == "Calcular")
            {
                StartCoroutine(CalculateGrade());
                
            }
    }

    private IEnumerator CalculateGrade()
    {
        GameObject.Find("Texto ajuda").GetComponent<Text>().text = "Aguarde, calculando...";
        yield return 0;    //Wait one frame
        float grade = this.gameObject.GetComponent<CompareMeshes>().avaliate();
        GameObject.Find("Nota").GetComponent<Text>().text = "Nota: " + grade.ToString("F2");
        GameObject.Find("Texto ajuda").GetComponent<Text>().text = "fale \"calcular\"";
        Debug.Log(grade);
    }
}