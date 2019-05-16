/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class GestureDetector : Detector
    {
        /**
         * The interval at which to check hand state.
         * @since 4.1.2
         */
        [Units("seconds")]
        [Tooltip("The interval in seconds at which to check this detector's conditions.")]
        [MinValue(0)]
        public float Period = .1f; //seconds

        [Tooltip("If False, it is the Right Hand")]
        public bool isLeft;

        /**
         * The HandModelBase instance to observe. 
         * Set automatically if not explicitly set in the editor.
         * @since 4.1.2
         */
        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModel = null;

        /**
         * The finger to compare to the specified direction.
         * @since 4.1.2
         */
        [Tooltip("The finger to observe.")]
        public Finger.FingerType FingerName = Finger.FingerType.TYPE_INDEX;

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
            Hand hand;

            while (true)
            {
                if (HandModel != null)
                {
                    hand = HandModel.GetLeapHand();
                    if (hand != null)
                    {
                        if (HandModel.IsTracked)
                        {
                            if (hand.IsPinching())
                                Debug.Log(hand.PinchDistance);
                        }
                    }
                }
                yield return new WaitForSeconds(Period);
            }
        }

        private int selectedFingerOrdinal()
        {
            switch (FingerName)
            {
                case Finger.FingerType.TYPE_INDEX:
                    return 1;
                case Finger.FingerType.TYPE_MIDDLE:
                    return 2;
                case Finger.FingerType.TYPE_PINKY:
                    return 4;
                case Finger.FingerType.TYPE_RING:
                    return 3;
                case Finger.FingerType.TYPE_THUMB:
                    return 0;
                default:
                    return 1;
            }
        }
    }
}
