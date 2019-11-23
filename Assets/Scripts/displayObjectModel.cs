using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayObjectModel : MonoBehaviour
{
    public Vector3 previousSize;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.parent = GameObject.Find("ModelObject").transform;
        this.gameObject.layer = 10;
        //resize model
        this.previousSize = this.gameObject.transform.localScale;

        this.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        //place the object in the corner of the camera screen
        placeObjectInCorner();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(0, 50 * Time.deltaTime, 0);
    }

    private void placeObjectInCorner()
    {
        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
        Vector3 sizeOfObject = this.gameObject.GetComponent<Renderer>().bounds.size;
        Vector3 upperLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        upperLeft.x = upperLeft.x + sizeOfObject.x / 2;
        upperLeft.y = upperLeft.y - sizeOfObject.y / 2;
        upperLeft.z = upperLeft.z + sizeOfObject.z / 2;
        GameObject camera2 = GameObject.Find("Camera Model Object");
        Vector3 cameraPosition = upperLeft;
        cameraPosition.z = -0.2f;
        camera2.transform.position = cameraPosition;

        print(sizeOfObject);
        this.gameObject.transform.position = upperLeft;

    }
}
