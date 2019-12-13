using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using UnityEngine.Windows.Speech;


public class InputManagerTouch : MonoBehaviour {

    public KeyCode Undo = KeyCode.Z;
    public FingerRaycastTouch FingerDeform;

    KeywordRecognizer keywordRecognizer;

    [SerializeField]
    public string[] keywordsModo;

    void Start()
    {
        keywordsModo = new string[1];
        keywordsModo[0] = "Desfazer";

        keywordRecognizer = new KeywordRecognizer(keywordsModo);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void Update () {
		if (Input.GetKey(Undo) || OVRInput.Get(OVRInput.Button.One))
        {
            FingerDeform.undoDeform();
        }
	}


    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence == ConfidenceLevel.Medium || args.confidence == ConfidenceLevel.High)
            if (args.text == keywordsModo[0])
            {
                Debug.Log("Modo " + keywordsModo[0]);
                FingerDeform.undoDeform();
            }
    }
}
