/**************************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                            *
 * Baseado em http://theflyingkeyboard.net/unity/unity-2d-c-drawing-lines-with-mouse/ *
 *                                                                                    *
  *************************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class DrawWithFinger : Detector
    {
        /**
         * The interval at which to check hand state.
         * @since 4.1.2
         */
        [Units("seconds")]
        [Tooltip("The interval in seconds at which to check this detector's conditions.")]
        [MinValue(0)]
        public float Period = .1f; //seconds

        private float simplifyTolerance = 0.02f;


        /**
         * The HandModelBase instance to observe. 
         * Set automatically if not explicitly set in the editor.
         * @since 4.1.2
         */
        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase DrawingHandModel = null;

  

        [SerializeField] private GameObject line;

        private IEnumerator watcherCoroutine;

        private Vector3 indexTip;
        private bool drawing;

        private void OnValidate()
        {
        }

        private void Awake()
        {
            watcherCoroutine = drawLine();
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

        private IEnumerator drawLine()
        {
            Hand drawingHand;
            LineRenderer lr = line.GetComponent<LineRenderer>();
            while (true)
            {
                if (DrawingHandModel != null)
                {
                    drawingHand = DrawingHandModel.GetLeapHand();
                    if (drawingHand != null)
                    {
                        if (Input.GetKeyDown(KeyCode.Space) && DrawingHandModel.IsTracked)
                        {
                            Debug.Log("1");
                            indexTip = drawingHand.Fingers[1].TipPosition.ToVector3();
                            Instantiate(line, indexTip, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                            drawing = true;
                        }
                        else if (Input.GetKey(KeyCode.Space) && DrawingHandModel.IsTracked && drawing)
                        {
                            indexTip = drawingHand.Fingers[1].TipPosition.ToVector3();
                            lr.positionCount++;
                            lr.SetPosition(lr.positionCount - 1, indexTip);
                        }
                        else if (Input.GetKeyUp(KeyCode.Space) && DrawingHandModel.IsTracked && drawing)
                        {
                            lr.Simplify(simplifyTolerance);
                            drawing = false;
                        }
                        else
                        {
                            drawing = false;
                        }
                    }
                }
                yield return new WaitForSeconds(Period);
            }
        }
    }
}
