using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPosition : MonoBehaviour
{
    private Quaternion rotation;
    private Vector3 position;

    private void Awake()
    {
        rotation = GetComponent<RectTransform>().rotation;
        position = GetComponent<RectTransform>().position;
    }
    private void LateUpdate()
    {
        GetComponent<RectTransform>().rotation = rotation;
        GetComponent<RectTransform>().position = position;
    }

}