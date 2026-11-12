// CW1 Member 5 - Tutorial 05.2/5.3: play minifig animations from UI Buttons (and keys)
// Author: Divya Patel (W1995126)
// Written with assistance from Claude (Anthropic), 11 Nov 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in my individual GitHub folder.
//
// The UI Buttons call PlayAnim("Walk") / PlayAnim("Stand") through their On Click () event.
// Keys W and S do the same, so the figure can also be tested without the mouse.

using UnityEngine;
using UnityEngine.InputSystem;

public class AnimPlay : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Called by a UI Button: type the Animator state name in the On Click () box
    public void PlayAnim(string stateName)
    {
        animator.Play(stateName, 0, 0f);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.wKey.wasPressedThisFrame) PlayAnim("Walk");
        if (kb.sKey.wasPressedThisFrame) PlayAnim("Stand");
    }
}
