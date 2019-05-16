using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class InputManager : MonoBehaviour {

    public KeyCode Undo = KeyCode.Z;
    public FingerRaycast FingerDeform;

	void Start () {
	}
	
	void Update () {
		if (Input.GetKey(Undo))
        {
            FingerDeform.undoDeform();
        }
	}
}
