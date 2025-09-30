using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUV : MonoBehaviour
{
    public Slider slider;
    public RawImage barRawImage;
    public float barSpeed = 1.2f;
    public float step = 0.001f;
    private float target;

    public void SetProgress(float progress, bool forceRefresh = false)
    {
        if (forceRefresh)
        {
            target = progress;
            slider.value = progress;
        }
        else
            target = progress;
    }

    private void Update()
    {
        if (slider.value < target)
            slider.value += step;
        Rect uvRect = barRawImage.uvRect;
        uvRect.x -= barSpeed * Time.deltaTime;
        barRawImage.uvRect = uvRect;
    }

}
