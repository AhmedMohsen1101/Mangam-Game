using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombGUI : MonoBehaviour
{
    [SerializeField] private GameObject bombPanel;
    [SerializeField] private Slider durationSlider;

    private bool isTweened = false;
    public void UpdateSliderValue(float value)
    {
       
        durationSlider.value = value;
    }

    public void SetMaxDuration(float maxDuration)
    {
        durationSlider.maxValue = maxDuration;
        LeanTween.scale(bombPanel, Vector3.one * 1.3f, 3f).setEasePunch();
        isTweened = false;
    }
}
