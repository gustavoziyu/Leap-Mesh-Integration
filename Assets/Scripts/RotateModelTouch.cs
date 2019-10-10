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
        [Range(0.0f, 10.0f)]
        public float rotationIntensity = 1;

        [Tooltip("Intensity of translation.")]
        [Range(0.0f, 1.0f)]
        public float translationIntensity = 0.05f;


        [Tooltip("Minimum distance to translate from Palm to Object.")]
        [Range(0.0f, 1.0f)]
        public float translateDistance = 0.3f;

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
            Hand usedHand;
            HandModelBase usedHandModel = HandModelRight;
            while (true)
            {
                if (usedHandModel != null)
                {
                    usedHand = usedHandModel.GetLeapHand();
                    if (usedHand != null)
                    {

                        palmObjectDistance = Vector3.Distance(target.transform.position, usedHand.PalmPosition.ToVector3());
                        if (usedHandModel.IsTracked && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) <= sensitivity && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > sensitivity)
                        {
                            target.transform.rotation = Quaternion.Euler(target.transform.rotation.eulerAngles.x,
                                                                         target.transform.rotation.eulerAngles.y + rotationIntensity * (-1) * usedHand.PalmVelocity.x,
                                                                         target.transform.rotation.eulerAngles.z);
                            target.transform.RotateAround(target.transform.position, Vector3.right, rotationIntensity * usedHand.PalmVelocity.y);
                        }
                        if (usedHandModel.IsTracked && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > sensitivity && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) <= sensitivity
                            && (palmObjectDistance < translateDistance || translating))
                        {
                            if (!translating) translating = true;
                            else
                            {
                                if (palmObjectDistance > translateDistance + 0.1f) translating = false;
                            }
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

