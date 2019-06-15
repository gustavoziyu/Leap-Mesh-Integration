/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class SwipeDetector : Detector
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


        public float minTimeBetweenSwipes = 1.5f;

        public float minSpeed = 0.8f;

        private bool swipped = false;
        private IEnumerator watcherCoroutine;

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
            float timer = 0;
            Hand rightHand;
            Hand leftHand;
            while (true)
            {
                if (HandModelRight != null && HandModelLeft != null)
                {
                    rightHand = HandModelRight.GetLeapHand();
                    leftHand = HandModelLeft.GetLeapHand();
                    if (rightHand != null)
                    {
                        if (HandModelRight.IsTracked)
                        {
                            if (rightHand.PalmVelocity.ToVector3().x > minSpeed)
                            {
                                if (!swipped)
                                {
                                    target.GetComponent<MaterialChange>().materialChange(true);
                                    swipped = true;
                                    timer = 0;
                                }
                            }
                            if (rightHand.PalmVelocity.ToVector3().x < -minSpeed)
                            {
                                if (!swipped)
                                {
                                    target.GetComponent<MaterialChange>().materialChange(false);
                                    swipped = true;
                                    timer = 0;
                                }
                            }
                        }
                    }
                    if (leftHand != null)
                    {
                        if (HandModelLeft.IsTracked)
                        {
                            if (leftHand.PalmVelocity.ToVector3().x > minSpeed)
                            {
                                if (!swipped)
                                {
                                    target.GetComponent<MaterialChange>().shaderChange(true);
                                    swipped = true;
                                    timer = 0;
                                }
                            }
                            if (leftHand.PalmVelocity.ToVector3().x < -minSpeed)
                            {
                                if (!swipped)
                                {
                                    target.GetComponent<MaterialChange>().shaderChange(false);
                                    swipped = true;
                                    timer = 0;
                                }
                            }
                        }
                    }
                }
                if (swipped && timer > minTimeBetweenSwipes) swipped = false;
                if (swipped) timer += Time.deltaTime;

              
                yield return new WaitForSeconds(Period);
            }
        }
    }
}