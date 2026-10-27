// The Outpost Garage - Inspection camera
// Author: Divya Patel (W1995126), Group [No.]
// Written with assistance from Claude (Anthropic), 27 Oct 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in individual GitHub folder.
//
// Orbit: hold left or right mouse button and drag. Zoom: mouse wheel. Reset view: R.
// A drag that STARTS over a UI element is ignored, so dragging the explode slider never moves the camera.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace OutpostGarage
{
    public class InspectionCamera : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;                 // point the camera orbits around
        public Vector3 targetOffset = new Vector3(0f, 0.9f, 0f);

        [Header("Orbit")]
        public float yawSpeed = 0.25f;           // degrees per pixel of mouse movement
        public float pitchSpeed = 0.2f;
        public float minPitch = 5f;              // clamp so the camera never goes under the floor
        public float maxPitch = 70f;

        [Header("Zoom")]
        public float distance = 9f;
        public float minDistance = 4.5f;
        public float maxDistance = 16f;
        public float zoomSpeed = 0.01f;          // per scroll unit (mouse wheel gives ~120 per notch)

        [Header("Smoothing")]
        public float smoothTime = 0.12f;

        // initial values remembered for Reset View
        float startYaw, startPitch, startDistance;
        float yaw, pitch, targetDistance;
        float yawVel, pitchVel, distVel;
        float smoothYaw, smoothPitch, smoothDistance;
        bool dragging;

        public float initialYaw = 145f;
        public float initialPitch = 18f;

        void Start()
        {
            startYaw = initialYaw; startPitch = initialPitch; startDistance = distance;
            ResetView(true);
        }

        public void ResetView() { ResetView(false); }

        void ResetView(bool snap)
        {
            yaw = startYaw; pitch = startPitch; targetDistance = startDistance;
            if (snap) { smoothYaw = yaw; smoothPitch = pitch; smoothDistance = targetDistance; Apply(); }
        }

        bool PointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        void Update()
        {
            var mouse = Mouse.current;
            var kb = Keyboard.current;
            if (kb != null && kb.rKey.wasPressedThisFrame) ResetView();

            if (mouse != null)
            {
                bool pressed = mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame;
                bool held = mouse.leftButton.isPressed || mouse.rightButton.isPressed;
                if (pressed) dragging = !PointerOverUI();   // decide once, when the drag starts
                if (!held) dragging = false;

                if (dragging)
                {
                    Vector2 d = mouse.delta.ReadValue();
                    yaw += d.x * yawSpeed;
                    pitch -= d.y * pitchSpeed;
                    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
                }

                float scroll = mouse.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > 0.01f && !PointerOverUI())
                {
                    targetDistance = Mathf.Clamp(targetDistance - scroll * zoomSpeed * targetDistance * 0.1f, minDistance, maxDistance);
                }
            }

            smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawVel, smoothTime);
            smoothPitch = Mathf.SmoothDamp(smoothPitch, pitch, ref pitchVel, smoothTime);
            smoothDistance = Mathf.SmoothDamp(smoothDistance, targetDistance, ref distVel, smoothTime);
            Apply();
        }

        void Apply()
        {
            if (target == null) return;
            Vector3 pivot = target.position + targetOffset;
            Quaternion rot = Quaternion.Euler(smoothPitch, smoothYaw, 0f);
            transform.position = pivot + rot * new Vector3(0f, 0f, -smoothDistance);
            transform.LookAt(pivot);
        }
    }
}
