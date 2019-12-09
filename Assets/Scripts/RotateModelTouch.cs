/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class RotateModelTouch : Detector
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
        public float Period = .1f; //seconds

        //The sensitivity of the triggers activation value
        public float sensitivity = 0.1f;

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

        [Tooltip("Intensity of rotation.")]
        [Range(0.0f, 1000.0f)]
        public float rotationIntensity = 500;

        [Tooltip("Intensity of translation.")]
        [Range(0.0f, 1000.0f)]
        public float translationIntensity = 1000f;

        [Tooltip("The hand that makes the rotation.")]
        public WhichHand rotatingHand = WhichHand.Right;

        private IEnumerator watcherCoroutine;

        private bool translating = false;
        private float palmObjectDistance;

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
            Vector3 startPosition = new Vector3(0, 0, 0);
            Vector3 startRotation = new Vector3(0, 0, 0);
            Vector3 currPosition;
            bool start = false;
            bool first = true;
            Hand usedHand;
            float rotateX = 0;
            float newRotateX = 0;
            float rotateY = 0;
            float newRotateY = 0;
            HandModelBase usedHandModel = HandModelRight;
            while (true)
            {
                if (usedHandModel != null && target != null)
                {
                    usedHand = usedHandModel.GetLeapHand();
                    if (usedHand != null)
                    {
                        palmObjectDistance = Vector3.Distance(target.transform.position, usedHand.PalmPosition.ToVector3());
                        if (usedHandModel.IsTracked && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) <= sensitivity && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > sensitivity)
                        {
                            if (!start) {
                                start = true;
                                startPosition = usedHand.PalmPosition.ToVector3();
                                startRotation = target.transform.rotation.eulerAngles;
                            }
                            currPosition = usedHand.PalmPosition.ToVector3();

                            if (first)
                            {
                                rotateX = 0;
                                rotateY = 0;
                                first = false;
                            }
                            newRotateX = (currPosition.y - startPosition.y) * rotationIntensity;
                            newRotateY = (currPosition.x - startPosition.x) * rotationIntensity;
                            target.transform.Rotate(0, newRotateY - rotateY, newRotateX - rotateX, UnityEngine.Space.World);
                            rotateX = newRotateX;
                            rotateY = newRotateY;

                        }
                        else {
                            start = false;
                            first = true;
                        }
                        if (usedHandModel.IsTracked && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > sensitivity && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) <= sensitivity)
                        {
                            Debug.Log(Vector3.Distance(target.transform.position, usedHand.PalmPosition.ToVector3()));
                            target.transform.position = new Vector3(target.transform.position.x + translationIntensity * usedHand.PalmVelocity.x,
                                                                    target.transform.position.y + translationIntensity * usedHand.PalmVelocity.y,
                                                                    target.transform.position.z + translationIntensity * usedHand.PalmVelocity.z);
                        }
                    }
                }
                yield return new WaitForSeconds(Period);
            }
        }
    }
}

