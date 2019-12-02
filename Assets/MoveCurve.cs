/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class MoveCurve : Detector
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


        /**
         * The HandModelBase instance to observe. The
         * Set automatically if not explicitly set in the editor.
         * @since 4.1.2
         */
        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModelLeft = null;
        public float sensitivity = .1f;
        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModelRight = null;

        [Tooltip("The game object to be translated.")]
        public GameObject target = null;

        [Tooltip("Intensity of translation.")]
        [Range(0.0f, 1.0f)]
        public float translationIntensity = 0.05f;

        [Tooltip("Minimum distance to translate from Palm to Object.")]
        [Range(0.0f, 1.0f)]
        public float translateDistance = 0.3f;

        [Tooltip("The hand that makes the translation.")]
        public WhichHand translationHand = WhichHand.Right;

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
            Hand usedHand, controlHand;
            HandModelBase usedHandModel = HandModelLeft;
            HandModelBase controlHandModel = HandModelRight;

            while (true)
            {
                if (usedHandModel != null && controlHandModel != null && target != null)
                {
                    usedHand = usedHandModel.GetLeapHand();
                    controlHand = controlHandModel.GetLeapHand();
                    if (usedHand != null && controlHand != null)
                    {
                        palmObjectDistance = Vector3.Distance(target.transform.position, controlHand.PalmPosition.ToVector3());
                        
                        if (usedHandModel.IsTracked && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > sensitivity
                            && (palmObjectDistance < translateDistance || translating))
                        {

                            if (!translating) translating = true;
                            else
                            {
                                if (palmObjectDistance > translateDistance + 0.1f) translating = false;
                            }

                            target.transform.position = new Vector3(target.transform.position.x + translationIntensity * controlHand.PalmVelocity.x,
                                                                    target.transform.position.y + translationIntensity * controlHand.PalmVelocity.y,
                                                                    0);
                            this.gameObject.GetComponent<DrawCurve>().updateCurve();
                        }
                    }
                }
                yield return new WaitForSeconds(Period);
            }
        }

    }
}

