using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiPanel : MonoBehaviour {

	public GameObject colorPanel;
	public Transform thumb;
    public Image ColorBar;
    public GameObject target;
    public float thumbSensitivity = 0.01f; 

    [Header("Config")]
	public Transform Picker;

    [Range(0, 5)]
    public float offZ;

    [Header("Freeze posX, posY")]
    public bool fixX;
    public bool fixY;

    private Vector2 leftThumbstick;

    private void Update ()
    {
        leftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        Debug.Log(Picker.position);
        Picker.position += new Vector3(thumbSensitivity * leftThumbstick.x, thumbSensitivity * leftThumbstick.y, 0);

        if (Input.GetKey("up"))
            Picker.position += new Vector3(0, 0.01f, 0);
		
		if(Input.GetKey("down"))
            Picker.position += Vector3.up * -.01f;
		
		if(Input.GetKey("left"))
            Picker.position += Vector3.right * -.01f;
		
		if(Input.GetKey("right"))
            Picker.position += Vector3.right * .01f;

        SetThumbPosition(Picker.position);
	}

    private void SetThumbPosition(Vector3 point)
	{
		Vector3 temp = thumb.localPosition;
        if (point.x < -1f)
            point.x = -1f;
        else if (point.x > -0.4f)
            point.x = -0.4f;
        if (point.y < -0.3f)
            point.y = -0.3f;
        else if (point.y > 0.3f)
            point.y = 0.3f;
        thumb.position = point;
		thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z+offZ);
		getImageColor(thumb.localPosition);

		showImageColor(getImageColor(thumb.localPosition));
	}
    
    private Color getImageColor(Vector2 point)
	{
		Vector2 rectPostion = mousePosToImagePos(point);
		Sprite _sprite = colorPanel.GetComponent<Image>().sprite;
		Rect rect = colorPanel.GetComponentInParent<RectTransform>().rect;
		Color imageColor = _sprite.texture.GetPixel( Mathf.FloorToInt(rectPostion.x * _sprite.texture.width / (rect.width)), 
													 Mathf.FloorToInt(rectPostion.y * _sprite.texture.height / (rect.height)));
		return imageColor;
	}
    
    private Vector2 mousePosToImagePos(Vector2 point)
	{
		Vector2 ImagePos = Vector2.zero;
		Rect rect = colorPanel.GetComponentInParent<RectTransform>().rect;
		ImagePos.x = point.x - colorPanel.transform.position.x + rect.width * 0.5f;
		ImagePos.y = point.y - colorPanel.transform.position.y + rect.height * 0.5f;
		return ImagePos;
	}

    private void showImageColor(Color _Color)
	{
		print (_Color.ToString("F2"));
        ColorBar.color = _Color;
        target.GetComponent<MeshRenderer>().material.SetColor("_Color", _Color);
    }
}
