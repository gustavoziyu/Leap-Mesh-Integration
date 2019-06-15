using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour {

    public Material[] materialList;

    private int count;

    public Shader[] shaderList;

    private int countShader;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void materialChange(bool next)
    {
        if (next) GetComponent<Renderer>().material = materialList[++count % materialList.Length];
        else
        {
            if (count <= 0)
            {
                count = materialList.Length;
                GetComponent<Renderer>().material = materialList[--count % materialList.Length];
            }
            else GetComponent<Renderer>().material = materialList[--count % materialList.Length];
        }
    }

    public void shaderChange(bool next)
    {
        if (next) GetComponent<Renderer>().material.shader = shaderList[++countShader % shaderList.Length];
        else
        {
            if (countShader <= 0)
            {
                countShader = shaderList.Length;
                GetComponent<Renderer>().material.shader = shaderList[--countShader % shaderList.Length];
            }
            else GetComponent<Renderer>().material.shader = shaderList[++countShader % shaderList.Length];
        }
    }
}
