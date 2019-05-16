﻿/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class ResizeModel : Detector
    {
        /**
         * The interval at which to check hand state.
         * @since 4.1.2
         */
        [Units("seconds")]
        [Tooltip("The interval in seconds at which to check this detector's conditions.")]
        [MinValue(0)]
        public float Period = .1f; //seconds


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
            Hand handLeft, handRight;
            Vector3 baseDistance = new Vector3(0, 0, 0);
            Vector3 currentDistance;
            bool startResizing = true;
     
            while (true)
            {
                if (HandModelLeft != null && HandModelRight != null)
                {
                    handLeft = HandModelLeft.GetLeapHand();
                    handRight = HandModelRight.GetLeapHand();
                    if (handLeft != null && handRight != null)
                    {
                        if (HandModelLeft.IsTracked && HandModelRight.IsTracked)
                        {
                            if (handLeft.IsPinching() && handRight.IsPinching())
                            {
                                positionLeft = handLeft.GetPredictedPinchPosition();
                                positionRight = handRight.GetPredictedPinchPosition();
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
                                    target.transform.localScale += new Vector3(target.transform.lossyScale.x * intensity * (currentDistance.x - baseDistance.x),
                                                                               target.transform.lossyScale.y * intensity * (currentDistance.y - baseDistance.y),
                                                                               target.transform.lossyScale.z * intensity * (currentDistance.z - baseDistance.z));

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

