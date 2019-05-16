/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class ClapDetector : Detector
    {
        public enum WhichHand
        {
            Left,
            Right
        }
        /**
         * The interval at which to check hand state.
         * @since 4.1.2
         */
        [Units("seconds")]
        [Tooltip("The interval in seconds at which to check this detector's conditions.")]
        [MinValue(0)]
        public float Period = .5f; //seconds


        /**
         * The HandModelBase instance to observe. The
         * Set automatically if not explicitly set in the editor.
         * @since 4.1.2
         */
        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModelLeft = null;

        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModelRight = null;

        [Tooltip("The game object to be resized.")]
        public GameObject target = null;


        public float minTimeBetweenClaps = 0.3f;
        public float maxTimeBetweenClaps = 1.8f;

        private Vector3 targetStartPosition;
        private Quaternion targetStartRotation;

        private bool firstClap = false;
        private IEnumerator watcherCoroutine;

        private void OnValidate()
        {
        }

        private void Awake()
        {
            watcherCoroutine = checkGesture();
            targetStartPosition = target.transform.position;
            targetStartRotation = target.transform.rotation;
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
            float timer = 0;
            Hand leftHand, rightHand;
            while (true)
            {
                if (HandModelLeft != null && HandModelRight != null)
                {
                    leftHand = HandModelLeft.GetLeapHand();
                    rightHand = HandModelRight.GetLeapHand();
                    if (leftHand != null && rightHand != null)
                    {
                        if (HandModelLeft.IsTracked && HandModelRight.IsTracked)
                        {
                            if (Vector3.Distance(leftHand.PalmPosition.ToVector3(), rightHand.PalmPosition.ToVector3()) < 0.01f) {
                                if (!firstClap)
                                {
                                    target.transform.position = targetStartPosition;
                                    target.transform.rotation = targetStartRotation;
                                    firstClap = true;
                                }
                                else
                                {
                                    if (timer > minTimeBetweenClaps)
                                    {
                                        firstClap = false;
                                    }
                                }
                            }
                        }

                    }
                }
                if (firstClap) timer += Time.deltaTime;
                // Debug.Log(timer);
                if (timer > maxTimeBetweenClaps)
                {
                    firstClap = false;
                    timer = 0;
                }
                yield return new WaitForSeconds(Period);
            }
        }
    }
}

