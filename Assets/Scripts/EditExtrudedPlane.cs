using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class EditExtrudedPlane : MonoBehaviour
{
    public String sceneName = "FullSculpt";
    public GameObject objectModel;
    private bool lockX = true;
    private bool lockY = true;
    private bool lockZ = false;

    private float returnSpeed = 5.0f;
    private float activationDistance = 0.05f;

    private Color inactiveColor = new Color(255, 255, 255);
    private Color transitionColor = new Color(222, 180, 180);
    private Color activeColor = new Color(237, 28, 36);

    protected bool pressed = false;
    protected bool released = false;
    protected Vector3 startPosition;

    void Start()
    {
        // Remember start position of button
        startPosition = transform.localPosition;
    }

    void Update()
    {
        released = false;

        // Use local position instead of global, so button can be rotated in any direction
        Vector3 localPos = transform.localPosition;
        if (lockX) localPos.x = startPosition.x;
        if (lockY) localPos.y = startPosition.y;
        if (lockZ) localPos.z = startPosition.z;
        transform.localPosition = localPos;

        // Return button to startPosition
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * returnSpeed);

        //Get distance of button press. Make sure to only have one moving axis.
        Vector3 allDistances = transform.localPosition - startPosition;
        float distance = 0f;
        if (!lockX) distance = Math.Abs(allDistances.x);
        if (!lockY) distance = Math.Abs(allDistances.y);
        if (!lockZ) distance = Math.Abs(allDistances.z);
        float pressComplete = Mathf.Clamp(1 / activationDistance * distance, 0f, 1f);

        //Activate pressed button
        if (pressComplete >= 0.7f && !pressed)
        {
            pressed = true;
            //Change color of object to activation color
            StartCoroutine(ChangeColor(gameObject, transitionColor, activeColor, 0.2f));
            objectModel.transform.parent = null;
            Destroy(objectModel.GetComponent<Triangulator>());
            objectModel.tag = "ModelObject";
            DontDestroyOnLoad(objectModel);
            StartCoroutine(LoadYourAsyncScene());
        }
        //Dectivate unpressed button
        else if (pressComplete <= 0.2f && pressed)
        {
            pressed = false;
            released = true;
            //Change color of object back to normal
            StartCoroutine(ChangeColor(gameObject, transitionColor, inactiveColor, 0.3f));
        }
        else if (pressComplete >= 0.2f && pressComplete <= 0.7f)
        {
            if (pressed) StartCoroutine(ChangeColor(gameObject, activeColor, transitionColor, 0.2f));
            else StartCoroutine(ChangeColor(gameObject, inactiveColor, transitionColor, 0.2f));
        }

    }


    private IEnumerator ChangeColor(GameObject obj, Color from, Color to, float duration)
    {
        float timeElapsed = 0.0f;
        float t = 0.0f;

        while (t < 1.0f)
        {
            timeElapsed += Time.deltaTime;
            t = timeElapsed / duration;
            obj.GetComponent<Renderer>().material.color = Color.Lerp(from, to, t);
            yield return null;
        }

    }

    private IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}