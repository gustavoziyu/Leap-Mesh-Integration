using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetChangeMaterial : MonoBehaviour
{
    private void Awake()
    {
        GameObject target = GameObject.FindWithTag("ModelObject");
        if (target == null)
        {
            Debug.Log("Model object not found! Make sure you have a object with the tag ModelObject on the scene.");
        }
        else
        {
            GetComponent<Leap.Unity.SwipeDetector>().target = target;
            GetComponent<Leap.Unity.RotateModelMaterial>().target = target;
            CopyComponent(GameObject.Find("MaterialChangePreset").GetComponent<MaterialChange>(), target);
            target.transform.parent = GameObject.Find("Scene").transform;
            target.transform.localPosition = Vector3.zero;
            target.transform.rotation = Quaternion.identity;
        }
    }

    // Start
    private void Start()
    {
    }

    private Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
