using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StandardObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.transform.parent = null;
        Destroy(this);
        Destroy(this.gameObject.GetComponent<BoxCollider>());
        this.gameObject.GetComponent<MeshCollider>().convex = true;
        this.gameObject.GetComponent<MeshCollider>().isTrigger = true;
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadYourAsyncScene());
    }
    private IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("First Integration Rotation+Scale");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
