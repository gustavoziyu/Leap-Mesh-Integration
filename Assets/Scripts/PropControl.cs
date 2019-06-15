using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropControl : MonoBehaviour
{

    public GameObject leftPalm;
    public GameObject rightPalm;
    public GameObject glow;

    public GameObject target;

    private float minHand = 0.05f;
    private float maxHand = 0.5f;

    public GameObject[] buttons;

    private bool active = false;

    public static float metallic = 0f;
    public static float smoothness = 0f;

    public bool m = false;
    public bool s = false;

    private bool sliding = false;

    // Use this for initialization
    void Start()
    {
        glow.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (leftPalm.transform.position.y < -0.2f && !sliding) sliding = true;
            if (sliding)
            {
                if (m)
                {
                    metallic = 2 * (0.25f + leftPalm.transform.position.y);
                    target.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", metallic);
                }
                if (s)
                {
                    smoothness = 2 * (0.25f + leftPalm.transform.position.y);
                    target.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", smoothness);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "bone2i")
        {
            active = !active;
            glow.SetActive(active);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<PropControl>().desactivate();
            }
        }
    }

    public void desactivate()
    {
        active = false;
        glow.SetActive(active);
        sliding = false;
    }

}
