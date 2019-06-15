using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaterialEditor : MonoBehaviour
{
    MeshRenderer meshRenderer;

    public Texture[] textures;
    public Texture normalTexture;
    public Texture albertoTexture;

    int index_texture = 0;
    int numberTextures = 2;

    GameObject textureButton;
    GameObject colorButton;
    GameObject albedoButton;    
    GameObject metallicSlider;
    GameObject smoothnessSlider;
    GameObject rColorSlider;
    GameObject gColorSlider;
    GameObject bColorSlider;
    GameObject okayButton;

    float rColorValue = 1f;
    float gColorValue = 1f;
    float bColorValue = 1f;



    // Start is called before the first frame update
    void Start()
    {
        textures = new Texture[numberTextures];
        textures[0] = albertoTexture;
        textures[1] = normalTexture;
        meshRenderer = GetComponent<MeshRenderer>();
        textureButton = GameObject.Find("TextureButton");
        albedoButton = GameObject.Find("AlbedoButton");
        metallicSlider = GameObject.Find("MetallicSlider");
        smoothnessSlider = GameObject.Find("SmoothnessSlider");
        colorButton = GameObject.Find("ColorButton");
        rColorSlider = GameObject.Find("R.Slider");
        gColorSlider = GameObject.Find("G.Slider");
        bColorSlider = GameObject.Find("B.Slider");
        okayButton = GameObject.Find("OkayButton");

        albedoButton.SetActive(false);
        metallicSlider.SetActive(false);
        smoothnessSlider.SetActive(false);

        rColorSlider.SetActive(false);
        gColorSlider.SetActive(false);
        bColorSlider.SetActive(false);

        okayButton.SetActive(false);

    }

    public void OnClickTextureButton()
    {
        textureButton.SetActive(false);
        colorButton.SetActive(false);
        albedoButton.SetActive(true);
        metallicSlider.SetActive(true);
        smoothnessSlider.SetActive(true);
        okayButton.SetActive(true);
    }

    public void OnClickColorButton()
    {
        textureButton.SetActive(false);
        colorButton.SetActive(false);
        rColorSlider.SetActive(true);
        gColorSlider.SetActive(true);
        bColorSlider.SetActive(true);
        okayButton.SetActive(true);
    }

    public void OnClickOkayButton()
    {
        textureButton.SetActive(true);
        colorButton.SetActive(true);
        albedoButton.SetActive(false);
        metallicSlider.SetActive(false);
        smoothnessSlider.SetActive(false);
        rColorSlider.SetActive(false);
        gColorSlider.SetActive(false);
        bColorSlider.SetActive(false);
        okayButton.SetActive(false);
    }


    public void ChangeAlbedoTexture()
    {
       meshRenderer.material.SetTexture("_MainTex", textures[index_texture]);
        Debug.Log(index_texture);
        Debug.Log(textures.Length);
       if (index_texture == textures.Length-1){
        index_texture = 0;
       }
       else
        {
            index_texture += 1;
        }
    }

    public void RColorValue(float rColor)
    {
        rColorValue = 1-rColor;
        meshRenderer.material.SetColor("_MainColor", new Vector4(rColorValue, gColorValue, bColorValue, 1f));
    }

    public void GColorValue(float gColor)
    {
        gColorValue = 1-gColor;
        meshRenderer.material.SetColor("_Color", new Vector4(rColorValue, gColorValue, bColorValue, 1f));
    }

    public void BColorValue(float bColor)
    {
        bColorValue = 1-bColor;
        meshRenderer.material.SetColor("_Color", new Vector4(rColorValue, gColorValue, bColorValue, 1f));
    }

    public void MetallicSlider(float metallic)
    {
        meshRenderer.material.SetFloat("_Metallic", metallic);
    }

    public void SmoothnessSlider(float smoothness)
    {
        meshRenderer.material.SetFloat("_Glossiness", smoothness);
    }

}
