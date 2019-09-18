using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OculusTouchDebug : MonoBehaviour
{
    public Text buttonTwoStatusText;
    public Text leftIndexTriggerText;
    public Text leftThumbstickXAxisText;
    public Text leftThumbstickYAxisText;

    private int buttonTwoStatus;
    private float leftIndexTrigger;
    private Vector2 leftThumbstickPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Button two (B) tracking
        if (OVRInput.Get(OVRInput.Button.Two)) buttonTwoStatus = 2;
        else if (OVRInput.Get(OVRInput.Touch.Two)) buttonTwoStatus = 1;
        else buttonTwoStatus = 0;
        setButtonStatusMessage(buttonTwoStatusText, "Button Two (B): ", buttonTwoStatus);

        //Left index trigger tracking
        leftIndexTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        leftIndexTriggerText.text = "Left index trigger position: " + leftIndexTrigger;

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

        text.text = tag + statusMessage;
    }
}
