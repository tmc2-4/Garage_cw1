// The Outpost Garage - Paint customisation
// Author: Divya Patel (W1995126), Group [No.]
// Written with assistance from Claude (Anthropic), 27 Oct 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in individual GitHub folder.
//
// Only material slots that use the "paint" material are changed. Glass, trim, engine and light materials
// keep their own materials. A runtime copy of the paint material is used so the project asset is never edited.

using UnityEngine;
using UnityEngine.InputSystem;

namespace OutpostGarage
{
    public class PaintController : MonoBehaviour
    {
        [System.Serializable]
        public class PaintOption
        {
            public string name;
            public Color color;
            [Range(0, 1)] public float metallic = 0.6f;
            [Range(0, 1)] public float smoothness = 0.65f;
        }

        public Material paintMaterial;          // the shared body paint asset
        public Renderer[] renderers;            // renderers that may use the paint material
        public PaintOption[] options;
        public int SelectedIndex { get; private set; }
        public string SelectedName => options != null && options.Length > 0 ? options[SelectedIndex].name : "";
        public event System.Action<int> OnPaintChanged;

        Material runtimePaint;

        void Awake()
        {
            runtimePaint = new Material(paintMaterial) { name = paintMaterial.name + " (Runtime)" };
            foreach (var r in renderers)
            {
                if (r == null) continue;
                var mats = r.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                    if (mats[i] == paintMaterial) mats[i] = runtimePaint;
                r.sharedMaterials = mats;
            }
        }

        void Start() { Select(0); }

        void Update()
        {
            var kb = Keyboard.current;
            if (kb == null) return;
            // number keys 1-4 select paints
            if (kb.digit1Key.wasPressedThisFrame) Select(0);
            if (kb.digit2Key.wasPressedThisFrame) Select(1);
            if (kb.digit3Key.wasPressedThisFrame) Select(2);
            if (kb.digit4Key.wasPressedThisFrame) Select(3);
        }

        public void Select(int index)
        {
            if (options == null || index < 0 || index >= options.Length) return;
            SelectedIndex = index;
            var o = options[index];
            runtimePaint.SetColor("_BaseColor", o.color);
            runtimePaint.SetFloat("_Metallic", o.metallic);
            runtimePaint.SetFloat("_Smoothness", o.smoothness);
            OnPaintChanged?.Invoke(index);
        }

        void OnDestroy() { if (runtimePaint != null) Destroy(runtimePaint); }
    }
}
