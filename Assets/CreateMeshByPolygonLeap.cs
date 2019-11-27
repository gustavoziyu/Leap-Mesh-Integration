/******************************************************************************
 * Modified by Victoria Akina Tanaka (2019)                                *
 * 
 * Requirements: 
 *      must be a object tagged with Plane in the scene;
 *      the object must have a Mesh Filter and a Mesh Renderer component;
 *      polygons are assumed to be an array of Vector3;
 *      polygons are assumed to be convex.
  ******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    public class CreateMeshByPolygonLeap : Detector
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

        [Tooltip("The game object to receive the mesh. TODO: set this to private and get this dynamically.")]
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
            GameObject.Find("Editar modelo").SetActive(false);
            GameObject.Find("TextEdit").SetActive(false);
            GameObject textError = GameObject.Find("TextErrorDisplay");
            if(textError != null)
            {
                textError.transform.parent = GameObject.Find("Background").transform.GetChild(0);
                textError.transform.position = GameObject.Find("TextError").transform.position;
                textError.GetComponent<TextFadeOut>().FadeOut();
            }

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

        public void Setup()
        {
            target = GameObject.FindWithTag("Plane");

            if (target == null)
                print("No object in scene with tag Plane. Please create a plane before extrusion.");

            target.AddComponent<MeshCollider>();
            target.name = "Extrusion";
            GameObject menu = GameObject.Find("Menu");
            menu.transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Background").transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

            GameObject.Find("Editar modelo").GetComponent<EditExtrudedPlane>().objectModel = target;
            menu.GetComponent<EditExtrudedModelSpeech>().extrudedObject = target;

            Vector3[] poly = new Vector3[] {
            //triangle
            // new Vector2(0,9),
            // new Vector2(5,0),
            // new Vector2(0,-5),
            
            //square
            // new Vector2(0,0),
		    // new Vector2(0,10), 
		    // new Vector2(10,10),
		    // new Vector2(10,0),

		    //   __________
            //  |          |
            //     |    |
            //     |    |
            //  |__________|
            new Vector3(5,5,0),
            new Vector3(-5,5,0),
            new Vector3(-5,3,0),
            new Vector3(-2,3,0),
            new Vector3(-2,-3,0),
            new Vector3(-5,-3,0),
            new Vector3(-5,-5,0),
            new Vector3(5,-5,0),
            new Vector3(5,-3,0),
            new Vector3(2,-3,0),
            new Vector3(2,3,0),
            new Vector3(5,3,0),
            };

            //Mesh m = CreateMeshByPolygon.CreateMesh(poly);
            //target.GetComponent<MeshFilter>().mesh = m;
        }

        private IEnumerator checkGesture()
        {
            Hand handLeft, handRight;
            bool startExtruding = true;
            Vector3 baseDistance = new Vector3(0, 0, 0);
            Vector3 currentDistance;

            while (true)
            {
                if (HandModelLeft != null && HandModelRight != null && target != null)
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
                                if (startExtruding)
                                {
                                    startExtruding = false;
                                    baseDistance.x = Mathf.Abs(positionLeft.x - positionRight.x);
                                    baseDistance.y = Mathf.Abs(positionLeft.y - positionRight.y);
                                    baseDistance.z = Mathf.Abs(positionLeft.z - positionRight.z);
                                }
                                else
                                {
                                    currentDistance.x = Mathf.Abs(positionLeft.x - positionRight.x);
                                    currentDistance.y = Mathf.Abs(positionLeft.y - positionRight.y);
                                    currentDistance.z = Mathf.Abs(positionLeft.z - positionRight.z);

                                    Mesh mesh = this.target.GetComponent<MeshFilter>().mesh;
                                    Vector3[] vertices = mesh.vertices;
                                    Vector3[] normals = mesh.normals;
                                    int polyLength = vertices.Length / 2;

                                    for (var i = 0; i < polyLength; i++)
                                    {
                                        vertices[i].z -= intensity*(currentDistance.x - baseDistance.x); // front vertex
                                        vertices[i + polyLength].z += intensity*(currentDistance.x - baseDistance.x);  // back vertex   
                                    }

                                    mesh.vertices = vertices;
                                    this.target.GetComponent<MeshCollider>().sharedMesh = mesh;
                                }

                            }
                            else
                            {
                                startExtruding = true;
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(Period);
            }
        }
    }
}

