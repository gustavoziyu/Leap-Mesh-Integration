/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class ResizeModelTouch : Detector
    {
        /**
         * The interval at which to check hand state.
         * @since 4.1.2
         */
        [Units("seconds")]
        [Tooltip("The interval in seconds at which to check this detector's conditions.")]
        [MinValue(0)]
        public float Period = .1f; //seconds

        //Trigger sensitivity
        public float sensitivity = 0.1f;

        /**
         * The HandModelBase instance to observe. 
         * Set automatically if not explicitly set in the editor.
         * @since 4.1.2
         */
        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModelLeft = null;

        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModelRight = null;

        [Tooltip("The game object to be resized.")]
        public GameObject target = null;

        [Tooltip("Intensity of deformation.")]
        [Range(0.0f, 10.0f)]
        public float intensity = 1;

        private IEnumerator watcherCoroutine;

        private Vector3 positionLeft, positionRight;

        private void OnValidate()
        {
        }

        private void Awake()
        {
            watcherCoroutine = checkGesture();
        }

        private void OnEnable()
        {
            StartCoroutine(watcherCoroutine);
        }

        private void OnDisable()
        {
            StopCoroutine(watcherCoroutine);
            Deactivate();
        }

        private IEnumerator checkGesture()
        {
            Hand handRight;
            Vector3 baseDistance = new Vector3(0, 0, 0);
            Vector3 currentDistance;
            bool startResizing = true;
     
            while (true)
            {
                //vibration function
                //OVRInput.SetControllerVibration(0.001f, 1, OVRInput.Controller.LTouch);
                if (HandModelRight != null)
                {
                    handRight = HandModelRight.GetLeapHand();
                    if (handRight != null)
                    {
                        //TODO Add OVR tracking
                        if (HandModelRight.IsTracked)
                        {

                            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > sensitivity && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > sensitivity && handRight.IsPinching())
                            {

                                Debug.Log("1");
                                positionLeft = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                                Debug.Log("LEFT: " + positionLeft);
                                positionRight = handRight.GetPredictedPinchPosition();
                                Debug.Log("RIGHT: " + positionRight);
                                if (startResizing)
                                {
                                    startResizing = false;
                                    baseDistance.x = Mathf.Abs(positionLeft.x - positionRight.x);
                                    baseDistance.y = Mathf.Abs(positionLeft.y - positionRight.y);
                                    baseDistance.z = Mathf.Abs(positionLeft.z - positionRight.z);
                                }
                                else
                                {
                                    currentDistance.x = Mathf.Abs(positionLeft.x - positionRight.x);
                                    currentDistance.y = Mathf.Abs(positionLeft.y - positionRight.y);
                                    currentDistance.z = Mathf.Abs(positionLeft.z - positionRight.z);
                                    target.transform.localScale += new Vector3(target.transform.lossyScale.x *-1 * intensity * (currentDistance.x - baseDistance.x),
                                                                               target.transform.lossyScale.y * intensity * (currentDistance.y - baseDistance.y),
                                                                               target.transform.lossyScale.z *-1* intensity * (currentDistance.z - baseDistance.z));

                                    baseDistance = currentDistance;
                                }

                            }
                            else
                            {
                                baseDistance = new Vector3(0, 0, 0);
                                startResizing = true;
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(Period);
            }
        }
    }
}

