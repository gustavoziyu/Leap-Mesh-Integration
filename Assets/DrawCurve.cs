using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Attributes;
using UnityEngine.SceneManagement;

namespace Leap.Unity
{
    public class DrawCurve : Detector
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

        [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
        public HandModelBase HandModelRight = null;

        [Tooltip("Marker that indicates the vertices of the curve.")]
        public GameObject marker = null;

        [Tooltip("Object with revolution script.")]
        public GameObject objectModel = null;

        [Tooltip("Y Axis.")]
        public GameObject Yaxis = null;

        private List<GameObject> markers = new List<GameObject>();
        private List<GameObject> lines = new List<GameObject>();

        private List<Vector3> curve = new List<Vector3>();

        public GameObject line;

        [Tooltip("The hand that makes the rotation.")]
        public WhichHand drawingHand = WhichHand.Right;

        private bool isDrawing = true;

        private int vCount = 0;


        private IEnumerator watcherCoroutine;

        private void OnValidate()
        {
        }

        private void Awake()
        {
            GameObject.Find("Editar modelo").SetActive(false);
            GameObject.Find("TextEdit").SetActive(false);

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

        public void activateDrawing()
        {
            isDrawing = true;
        }

        private IEnumerator checkGesture()
        {
            Hand usedHand, controlHand;
            Transform start = this.transform; // Só para o compilador não reclamar
            Transform target;
            float dist, controldist;
            float depth = 0.3f;
            bool pinch = false;
            bool finished = false;
            GameObject lastMarker, currentLine;
            Vector3 diff = new Vector3(0, 0, 0);
            HandModelBase usedHandModel = (drawingHand == WhichHand.Left) ? HandModelLeft : HandModelRight;
            HandModelBase controlHandModel = (drawingHand == WhichHand.Left) ? HandModelRight : HandModelLeft;
            while (true)
            {
                if (usedHandModel != null && controlHandModel != null)
                {
                    usedHand = usedHandModel.GetLeapHand();
                    controlHand = controlHandModel.GetLeapHand();
                    if (isDrawing)
                    {
                        if (usedHand != null)
                        {
                            if (usedHandModel.IsTracked)
                            {
                                dist = Vector3.Distance(usedHand.Fingers[1].TipPosition.ToVector3(), usedHand.Fingers[0].TipPosition.ToVector3());
                                if (dist < 0.025f && !pinch)
                                {
                                    lastMarker = Instantiate(marker, usedHand.Fingers[1].TipPosition.ToVector3(), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                                    if (vCount > 0) lastMarker.transform.position = new Vector3(lastMarker.transform.position.x, lastMarker.transform.position.y, depth);
                                    markers.Add(lastMarker);
                                    curve.Add(new Vector3(lastMarker.transform.position.x, lastMarker.transform.position.y, 0));
                                    if (vCount == 0)
                                    {
                                        depth = lastMarker.transform.position.z; // depth is always the first marker's z position
                                        start = lastMarker.transform;
                                        objectModel.transform.position = new Vector3(0, 0, depth);
                                        Yaxis.transform.position = new Vector3(0, 0, depth);

                                    }
                                    else
                                    {
                                        // Cria linha entre o novo ponto e o último ponto
                                        target = lastMarker.transform;
                                        diff = target.position - start.position;
                                        currentLine = Instantiate(line);
                                        currentLine.transform.localScale = new Vector3(0.01f, diff.magnitude / 2, 0.01f);
                                        currentLine.transform.position = start.position + diff / 2;
                                        currentLine.transform.LookAt(start.position, Vector3.back);

                                        currentLine.transform.Rotate(90, 0, 0); // Corrige rotação

                                        lines.Add(currentLine);

                                        start = target;
                                    }
                                    pinch = true;
                                    vCount++;
                                }
                                else if (dist > 0.05f)
                                {
                                    pinch = false;
                                }
                            }
                        }

                        if (controlHand != null && vCount > 1)
                        {
                            if (controlHandModel.IsTracked)
                            {
                                controldist = Vector3.Distance(controlHand.Fingers[1].TipPosition.ToVector3(), controlHand.Fingers[0].TipPosition.ToVector3());
                                if (controldist < 0.025f)
                                {
                                    isDrawing = false; // Encerra processo de criãção
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!finished)
                        {
                            createCurve();
                            finished = true;
                        }
                    }
                }

                yield return new WaitForSeconds(Period);
            }
        }


        private void createCurve()
        {
            objectModel.GetComponent<CreateMeshByRotation>().curve = curve.ToArray();
            Mesh m = objectModel.GetComponent<CreateMeshByRotation>().createMesh();
            objectModel.GetComponent<MeshFilter>().mesh = m;
            objectModel.GetComponent<MeshCollider>().sharedMesh = m;
            GameObject.Find("Menu").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Background").transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
    }

}
