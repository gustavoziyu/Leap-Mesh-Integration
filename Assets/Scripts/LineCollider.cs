/*
 * Fonte: https://answers.unity.com/questions/470943/collider-for-line-renderer.html
 * Modificado por Gustavo Ziyu Wang (2019)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour
{
    public Transform start;
    public Transform target;

    LineRenderer line;
    CapsuleCollider capsule;

    public float LineWidth = 0.1f; // use the same as you set in the line renderer.

    public LineCollider(Transform start, Transform target)
    {
        this.start = start;
        this.target = target;
    }

    void Start()
    {
        line = gameObject.AddComponent<LineRenderer>();
        capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.radius = LineWidth / 2;
        capsule.center = Vector3.zero;
        capsule.direction = 2; // Z-axis for easier "LookAt" orientation

        line.SetPosition(0, start.position);
        line.SetPosition(1, target.position);

        capsule.transform.position = start.position + (target.position - start.position) / 2;
        capsule.transform.LookAt(start.position);
        capsule.height = (target.position - start.position).magnitude;
    }

}
