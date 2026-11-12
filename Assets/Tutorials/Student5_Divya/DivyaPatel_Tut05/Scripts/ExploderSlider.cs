// CW1 Member 5 - Tutorial 05.1: Lego brick array exploded by a UI Slider
// Author: Divya Patel (W1995126)
// Written with assistance from Claude (Anthropic), 11 Nov 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in my individual GitHub folder.
//
// The slider scrubs the "Explode" animation clip: slider 0 = assembled, slider 1 = fully exploded.
// The Animator is frozen (speed 0) and the normalised time of the clip is set from the slider value,
// so dragging back and forth plays the explosion forwards and backwards.

using UnityEngine;
using UnityEngine.UI;

public class ExploderSlider : MonoBehaviour
{
    public Slider slider;                 // drag the UI Slider here in the Inspector
    public string stateName = "Explode";  // Animator state that holds the explode clip

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;                            // we drive the clip ourselves
        slider.onValueChanged.AddListener(OnSliderChanged);
        OnSliderChanged(slider.value);
    }

    void OnSliderChanged(float value)
    {
        // Play(state, layer, normalisedTime) jumps straight to that point of the clip
        animator.Play(stateName, 0, Mathf.Clamp(value, 0f, 0.999f));
        animator.Update(0f);                            // apply the pose this frame
    }
}
