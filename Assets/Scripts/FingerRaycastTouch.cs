/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2018.                                 *
 * Leap Motion proprietary and confidential.                                  *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/


/******************************************************************************
 * Modificado por Gustavo Ziyu Wang (2019)                                    *
 * Usando PaintVertices.cs                                                    *
  ******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity
{
    /**
     * Detects when specified fingers are pointing in the specified manner.
     * 
     * Directions can be specified relative to the global frame of reference, relative to 
     * the camera frame of reference, or using a combination of the two -- relative to the 
     * camera direction in the x-z plane, but not changing relative to the horizon.
     * 
     * You can alternatively specify a target game object.
     * 
     * If added to a HandModelBase instance or one of its children, this detector checks the
     * finger direction at the interval specified by the Period variable. You can also specify
     * which hand model to observe explicitly by setting handModel in the Unity editor or 
     * in code.
     * 
     * @since 4.1.2
     */
    public class FingerRaycastTouch : Detector
    {
        public float indicatorDistance = 0.5f;
        public float sizeIndicator = 0.05f;
        public float radius = 1.0f;
        public float pullStrength = 5.0f;
        private MeshFilter unappliedMesh;
        public FallOff fallOff = FallOff.Gauss;
        public Material indicatorMaterial;
        public Color indicatorColor;

        private bool saveMesh;
        private MeshFilter uMesh;
        private Mesh savedMesh;
        private Vector3[] savedVertices;
        private Vector3[] savedNormals;
        private GameObject indicator;
        private Vector2 leftThumbstickPosition;

        /**
         * The interval at which to check finger state.
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
            indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indicator.transform.localScale = new Vector3(sizeIndicator, sizeIndicator, sizeIndicator);
            indicator.GetComponent<MeshRenderer>().material = indicatorMaterial;
            indicator.GetComponent<Renderer>().material.SetColor("_Color", indicatorColor);
            watcherCoroutine = showRaycast();
        }

        private void OnEnable()
        {
            saveMesh = true;
            StartCoroutine(watcherCoroutine);
        }

        private void OnDisable()
        {
            StopCoroutine(watcherCoroutine);
            Deactivate();
        }

        private IEnumerator showRaycast()
        {
            Hand hand;

            RaycastHit hit;

            int selectedFinger = selectedFingerOrdinal();
            while (true)
            {
                if (HandModel != null && HandModel.IsTracked)
                {
                    hand = HandModel.GetLeapHand();
                    if (hand != null)
                    {
                        if (!(HandModel.IsTracked))
                        {
                            ApplyMeshCollider();
                        }
                        if (HandModel.IsTracked)
                        {
                            if (Physics.Raycast(hand.Fingers[selectedFinger].TipPosition.ToVector3(), hand.Fingers[selectedFinger].Direction.ToVector3(), out hit, Mathf.Infinity))
                            {
                                Debug.DrawRay(hand.Fingers[selectedFinger].TipPosition.ToVector3(), hand.Fingers[selectedFinger].Direction.ToVector3() * hit.distance, Color.yellow);
                                if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) == new Vector2(0f, 0f) && hit.transform.gameObject.tag == "ModelObject")
                                    indicator.SetActive(true);
                                else
                                    indicator.SetActive(false);
                                Vector3 startEnd = hit.point - hand.Fingers[selectedFinger].TipPosition.ToVector3();
                                indicator.transform.position = hit.point - startEnd * indicatorDistance;
                                MeshFilter filter = hit.collider.GetComponent<MeshFilter>();
                                if (filter)
                                {
                                    if (saveMesh)
                                    {
                                        // Stores previous mesh
                                        saveMesh = false;
                                        savedMesh = filter.mesh;
                                        uMesh = filter;
                                        savedVertices = savedMesh.vertices;
                                        savedNormals = savedMesh.normals;
                                    }

                                    // Don't update mesh collider every frame since physX
                                    // does some heavy processing to optimize the collision mesh.
                                    // So this is not fast enough for real time updating every frame
                                    if (filter != unappliedMesh)
                                    {
                                        ApplyMeshCollider();
                                        unappliedMesh = filter;
                                    }

                                    // Deform mesh
                                    Vector3 relativePoint = filter.transform.InverseTransformPoint(hit.point);
                                    leftThumbstickPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                                    Debug.Log(leftThumbstickPosition);
                                    DeformMesh(filter.mesh, relativePoint, leftThumbstickPosition.y * pullStrength * Time.deltaTime, radius);

                                }
                            }
                            else
                            {
                                saveMesh = true;
                                indicator.SetActive(false);
                            }
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

        private void ApplyMeshCollider()
        {
            if (unappliedMesh && unappliedMesh.GetComponent<MeshCollider>())
            {
                unappliedMesh.GetComponent<MeshCollider>().sharedMesh = unappliedMesh.sharedMesh;
            }

            unappliedMesh = null;
        }

        private float NeedleFalloff(float dist, float inRadius)
        {
            return -(dist * dist) / (inRadius * inRadius) + 1.0f;
        }

        private void DeformMesh(Mesh mesh, Vector3 position, float power, float inRadius)
        {
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            float sqrRadius = inRadius * inRadius;

            // Calculate averaged normal of all surrounding vertices	
            Vector3 averageNormal = Vector3.zero;
            for (int i = 0; i < vertices.Length; i++)
            {
                float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
                // Early out if too far away
                if (sqrMagnitude > sqrRadius)
                {
                    continue;
                }

                float distance = Mathf.Sqrt(sqrMagnitude);
                float falloff = LinearFalloff(distance, inRadius);
                averageNormal += falloff * normals[i];
            }

            averageNormal = averageNormal.normalized;

            // Deform vertices along averaged normal
            for (int i = 0; i < vertices.Length; i++)
            {
                float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
                // Early out if too far away
                if (sqrMagnitude > sqrRadius)
                {
                    continue;
                }

                float distance = Mathf.Sqrt(sqrMagnitude);
                float falloff;
                switch (fallOff)
                {
                    case FallOff.Gauss:
                        falloff = GaussFalloff(distance, inRadius);
                        break;
                    case FallOff.Needle:
                        falloff = NeedleFalloff(distance, inRadius);
                        break;
                    default:
                        falloff = LinearFalloff(distance, inRadius);
                        break;
                }

                vertices[i] += averageNormal * falloff * power;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        private static float LinearFalloff(float distance, float inRadius)
        {
            return Mathf.Clamp01(1.0f - distance / inRadius);
        }

        private static float GaussFalloff(float distance, float inRadius)
        {
            return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
        }

        public void undoDeform()
        {
            savedMesh.vertices = savedVertices;
            savedMesh.normals = savedNormals;
            savedMesh.RecalculateNormals();
            savedMesh.RecalculateBounds();
            unappliedMesh = uMesh;
            ApplyMeshCollider();
            saveMesh = false;
        }

    }
}
