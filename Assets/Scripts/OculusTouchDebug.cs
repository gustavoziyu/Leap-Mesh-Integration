using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OculusTouchDebug : MonoBehaviour
{
    public Text buttonThreeStatusText;
    public Text buttonFourStatusText;
    public Text leftIndexTriggerText;
    public Text leftHandTriggerText;
    public Text leftThumbstickXAxisText;
    public Text leftThumbstickYAxisText;

    private int buttonThreeStatus;
    private int buttonFourStatus;
    private float leftIndexTrigger;
    private float leftHandTrigger;
    private Vector2 leftThumbstickPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Button three (X) tracking
        if (OVRInput.Get(OVRInput.Button.Three)) buttonThreeStatus = 2;
        else if (OVRInput.Get(OVRInput.Touch.Three)) buttonThreeStatus = 1;
        else buttonThreeStatus = 0;
        setButtonStatusMessage(buttonThreeStatusText, "Button Three (X): ", buttonThreeStatus);
        
        //Button four (Y) tracking
        if (OVRInput.Get(OVRInput.Button.Four)) buttonFourStatus = 2;
        else if (OVRInput.Get(OVRInput.Touch.Four)) buttonFourStatus = 1;
        else buttonFourStatus = 0;
        setButtonStatusMessage(buttonFourStatusText, "Button Four (Y): ", buttonFourStatus);

        //Left index trigger tracking
        leftIndexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        leftIndexTriggerText.text = "Left index trigger position: " + leftIndexTrigger;
        Debug.Log("Left index trigger position: " + leftIndexTrigger);
       
        //Left hand trigger tracking
        leftHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        leftHandTriggerText.text = "Left hand trigger position: " + leftHandTrigger;
        Debug.Log("Left hand trigger position: " + leftHandTrigger);

        //Left thumbstick tracking
        leftThumbstickPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        leftThumbstickXAxisText.text = "Left thumbstick X position: " + leftThumbstickPosition.x;
        leftThumbstickYAxisText.text = "Left thumbstick Y position: " + leftThumbstickPosition.y;

    }

    void setButtonStatusMessage(Text text, string tag, int status) {
        string statusMessage;
        if (status == 0) statusMessage = "Released";
        else if (status == 1) statusMessage = "Touched";
        else if (status == 2) statusMessage = "Pressed";
        else statusMessage = "UNKNOWN STATUS VALUE";
        Debug.Log(tag + leftIndexTrigger);

        text.text = tag + statusMessage;
    }
}
