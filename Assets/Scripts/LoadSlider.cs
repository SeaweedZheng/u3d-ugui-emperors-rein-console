using UnityEngine;
using UnityEngine.UI;

public class LoadSlider : MonoBehaviour
{
    private Slider slider;
    private float orignal;
    private float target;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetSliderValue(float value)
    {
        target = value;
    }

    private void Start()
    {
        slider.value = orignal;
    }

    private void Update()
    {
        if (slider.value < target)
            slider.value += 0.001f;
    }

}
