// The Outpost Garage - Exploded component view
// Author: Divya Patel (W1995126), Group [No.]
// Written with assistance from Claude (Anthropic), 27 Oct 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in individual GitHub folder.
//
// Each vehicle component group has an offset. The original local positions are cached once in Awake,
// and every frame position = original + offset * eased amount. Positions are never added to cumulatively,
// so repeated slider movement or resets cannot drift.

using UnityEngine;

namespace OutpostGarage
{
    public class ExplodedView : MonoBehaviour
    {
        [System.Serializable]
        public class Part
        {
            public Transform transform;
            public Vector3 explodeOffset;   // local-space offset at 100%
            public Vector3 explodeRotation; // extra local rotation (degrees) at 100%
            [HideInInspector] public Vector3 startPos;
            [HideInInspector] public Quaternion startRot;
        }

        public Part[] parts;
        [Range(0f, 1f)] public float targetAmount;   // what the slider asks for
        public float speed = 4f;                      // how quickly the parts follow the slider

        public float CurrentAmount { get; private set; }
        public bool IsAssembled => CurrentAmount < 0.02f && targetAmount < 0.02f;
        public event System.Action<float> OnAmountChanged;

        void Awake()
        {
            foreach (var p in parts)
            {
                if (p.transform == null) continue;
                p.startPos = p.transform.localPosition;
                p.startRot = p.transform.localRotation;
            }
        }

        public void SetAmount(float amount) { targetAmount = Mathf.Clamp01(amount); }
        public void Assemble() { targetAmount = 0f; }

        void Update()
        {
            float prev = CurrentAmount;
            CurrentAmount = Mathf.MoveTowards(CurrentAmount, targetAmount, speed * Time.deltaTime);
            if (Mathf.Approximately(prev, CurrentAmount) && Time.frameCount > 2) return;

            float e = Mathf.SmoothStep(0f, 1f, CurrentAmount);
            foreach (var p in parts)
            {
                if (p.transform == null) continue;
                p.transform.localPosition = p.startPos + p.explodeOffset * e;
                p.transform.localRotation = p.startRot * Quaternion.Euler(p.explodeRotation * e);
            }
            OnAmountChanged?.Invoke(CurrentAmount);
        }
    }
}
