using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderWaitTime : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Collider box = gameObject.GetComponent<BoxCollider>();
        Collider mesh = gameObject.GetComponent<MeshCollider>();
        box.enabled = false;
        mesh.enabled = false;
        yield return new WaitForSeconds(2);
        box.enabled = true;
        mesh.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
