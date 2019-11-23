using System.Collections;
using UnityEngine;
using UnityEngine.UI;
class TextFadeOut : MonoBehaviour
{
    //Fade time in seconds
    public float fadeOutTime;
    public Color originalColor;
    public float waitTime;

    public void setToOriginalColor()
    {
        GetComponent<Text>().color = originalColor;
    }
    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }
    private IEnumerator FadeOutRoutine()
    {
        Text text = GetComponent<Text>();
        yield return new WaitForSeconds(waitTime);
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {  
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
    }
}