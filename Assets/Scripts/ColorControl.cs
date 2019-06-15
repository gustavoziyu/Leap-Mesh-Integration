using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControl : MonoBehaviour {

    public GameObject leftPalm;
    public GameObject rightPalm;
    public GameObject glow;

    public GameObject target;

    private float minHand = 0.05f;
    private float maxHand = 0.5f;

    public GameObject[] buttons;

    private bool active = false;

    public static float cgreen = 1f;
    public static float cred = 1f;
    public static float cblue = 1f;

    public bool r = false;
    public bool g = false;
    public bool b = false;

    private bool sliding = false;

	// Use this for initialization
	void Start () {
        glow.SetActive(active);
	}
	
	// Update is called once per frame
	void Update () {
		if (active)
        {
            if (rightPalm.transform.position.y < -0.2f && !sliding) sliding = true;
            if (sliding)
            {
                if (r) cred = 1 - 2 * (0.25f + rightPalm.transform.position.y);
                if (g) cgreen = 1 - 2 * (0.25f + rightPalm.transform.position.y);
                if (b) cblue = 1 - 2 * (0.25f + rightPalm.transform.position.y);
                if (r || g || b) target.GetComponent<MeshRenderer>().material.SetColor("_Color", new Vector4(cred, cgreen, cblue, 1f));
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
                buttons[i].GetComponent<ColorControl>().desactivate();
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
