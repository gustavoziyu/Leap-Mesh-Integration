using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRigidbody : MonoBehaviour {
    Vector3 m_EulerAngleVelocity;
    Quaternion deltaRotation;
    // Use this for initialization
    void Start () {
        m_EulerAngleVelocity = new Vector3(0, 100, 0);

    }
	
	// Update is called once per frame
	void Update () {
        deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * deltaRotation);
	}
}
