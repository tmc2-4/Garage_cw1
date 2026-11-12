// CW1 Member 5 - Tutorial 05.1 extension: rotate the brick array with a second UI Slider
// Author: Divya Patel (W1995126)
// Written with assistance from Claude (Anthropic), 11 Nov 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in my individual GitHub folder.
//
// Slider range is 0-360 (set Max Value to 360 in the Inspector). The value is used as the
// Y rotation in degrees, added to the object's starting rotation so it never drifts.

using UnityEngine;
using UnityEngine.UI;

public class RotatorSlider : MonoBehaviour
{
    public Slider slider;            // drag the "Rotator" UI Slider here

    Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
        slider.onValueChanged.AddListener(OnSliderChanged);
        OnSliderChanged(slider.value);
    }

    void OnSliderChanged(float degrees)
    {
        transform.localRotation = startRotation * Quaternion.Euler(0f, degrees, 0f);
    }
}
