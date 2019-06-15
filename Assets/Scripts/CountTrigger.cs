using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTrigger : MonoBehaviour {

    public int count;
    public string nameOther = "CapsuleLine(Clone)";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == nameOther)
        {
            count++;
            Debug.Log(count);
        }

    }
}
