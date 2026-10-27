// The Outpost Garage - Engine startup sequence
// Author: Divya Patel (W1995126), Group [No.]
// Written with assistance from Claude (Anthropic), 27 Oct 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in individual GitHub folder.
//
// Toggle with E or the on-screen button. A single "power" value moves between 0 (off) and 1 (running),
// so pressing again part-way through simply reverses the sequence. Power drives emission brightness,
// thruster lights, a hover bob and a procedurally generated engine hum (no audio asset needed).
// The engine only starts when the vehicle is assembled; exploding the vehicle shuts it down.

using UnityEngine;
using UnityEngine.InputSystem;

namespace OutpostGarage
{
    public enum EngineState { Off, Starting, Running, ShuttingDown }

    public class EngineController : MonoBehaviour
    {
        public ExplodedView explodedView;
        public Transform hoverBody;              // object that bobs while running (the vehicle root)
        public Renderer[] glowRenderers;         // renderers whose glow materials are animated
        public Material[] glowMaterials;         // shared glow assets to clone at runtime
        public Light[] engineLights;
        public float startupTime = 1.6f;
        public float shutdownTime = 1.0f;
        public float maxEmission = 6f;
        public float idleEmission = 0.4f;
        public float maxLightIntensity = 6f;

        public EngineState State { get; private set; } = EngineState.Off;
        public string Message { get; private set; } = "";
        public event System.Action OnStateChanged;

        float power;        // 0..1
        bool wantOn;
        Material[] runtimeGlow;
        Color[] baseEmission;
        Vector3 bodyStart;
        AudioSource hum;
        float messageTimer;

        void Awake()
        {
            runtimeGlow = new Material[glowMaterials.Length];
            baseEmission = new Color[glowMaterials.Length];
            for (int i = 0; i < glowMaterials.Length; i++)
            {
                runtimeGlow[i] = new Material(glowMaterials[i]) { name = glowMaterials[i].name + " (Runtime)" };
                runtimeGlow[i].EnableKeyword("_EMISSION");
                Color c = glowMaterials[i].GetColor("_EmissionColor");
                float m = Mathf.Max(c.r, Mathf.Max(c.g, c.b));
                baseEmission[i] = m > 0 ? c / m : new Color(0.1f, 0.8f, 1f);
            }
            foreach (var r in glowRenderers)
            {
                if (r == null) continue;
                var mats = r.sharedMaterials;
                for (int s = 0; s < mats.Length; s++)
                    for (int i = 0; i < glowMaterials.Length; i++)
                        if (mats[s] == glowMaterials[i]) mats[s] = runtimeGlow[i];
                r.sharedMaterials = mats;
            }
            if (hoverBody != null) bodyStart = hoverBody.localPosition;
            hum = gameObject.AddComponent<AudioSource>();
            hum.clip = BuildHumClip();
            hum.loop = true; hum.volume = 0f; hum.playOnAwake = false; hum.spatialBlend = 0f;
            Apply();
        }

        void Update()
        {
            var kb = Keyboard.current;
            if (kb != null && kb.eKey.wasPressedThisFrame) Toggle();

            // exploding the vehicle while running shuts the engine down safely
            if (wantOn && explodedView != null && explodedView.targetAmount > 0.02f)
            {
                wantOn = false;
                Flash("Engine shut down: components separated");
            }

            float target = wantOn ? 1f : 0f;
            float rate = 1f / (wantOn ? startupTime : shutdownTime);
            power = Mathf.MoveTowards(power, target, rate * Time.deltaTime);

            EngineState s = power <= 0f ? EngineState.Off : power >= 1f ? EngineState.Running
                          : wantOn ? EngineState.Starting : EngineState.ShuttingDown;
            if (s != State) { State = s; OnStateChanged?.Invoke(); }

            if (messageTimer > 0f) { messageTimer -= Time.deltaTime; if (messageTimer <= 0f) { Message = ""; OnStateChanged?.Invoke(); } }
            Apply();
        }

        public void Toggle()
        {
            if (!wantOn && explodedView != null && !explodedView.IsAssembled)
            {
                Flash("Assemble the vehicle before starting the engine");
                return;
            }
            wantOn = !wantOn;
            OnStateChanged?.Invoke();
        }

        public void ForceOff() { wantOn = false; power = 0f; Apply(); }

        void Flash(string msg) { Message = msg; messageTimer = 2.5f; OnStateChanged?.Invoke(); }

        void Apply()
        {
            float eased = power * power * (3f - 2f * power);
            float flicker = State == EngineState.Starting ? (Mathf.PerlinNoise(Time.time * 18f, 0f) * 0.6f + 0.4f) : 1f;
            float pulse = 1f + 0.08f * Mathf.Sin(Time.time * 6f) * eased;
            float emission = Mathf.Lerp(idleEmission, maxEmission, eased) * flicker * pulse;
            for (int i = 0; i < runtimeGlow.Length; i++)
                runtimeGlow[i].SetColor("_EmissionColor", baseEmission[i] * emission);
            foreach (var l in engineLights)
                if (l != null) l.intensity = maxLightIntensity * eased * flicker;

            if (hoverBody != null)
                hoverBody.localPosition = bodyStart + Vector3.up * (eased * (0.12f + 0.04f * Mathf.Sin(Time.time * 2.2f)));

            if (hum != null)
            {
                hum.volume = 0.35f * eased;
                hum.pitch = 0.6f + 0.5f * eased;
                if (eased > 0f && !hum.isPlaying) hum.Play();
                if (eased <= 0f && hum.isPlaying) hum.Stop();
            }
        }

        static AudioClip BuildHumClip()
        {
            int rate = 44100; int len = rate * 2;       // 2 s seamless loop (all frequencies are whole Hz)
            var data = new float[len];
            var rng = new System.Random(7);
            for (int i = 0; i < len; i++)
            {
                float t = (float)i / rate;
                float v = 0.5f * Mathf.Sin(2 * Mathf.PI * 55f * t)
                        + 0.25f * Mathf.Sin(2 * Mathf.PI * 110f * t)
                        + 0.12f * Mathf.Sin(2 * Mathf.PI * 220f * t + Mathf.Sin(2 * Mathf.PI * 3f * t))
                        + 0.05f * (float)(rng.NextDouble() * 2 - 1);
                data[i] = v * 0.6f;
            }
            var clip = AudioClip.Create("EngineHum", len, 1, rate, false);
            clip.SetData(data, 0);
            return clip;
        }

        void OnDestroy()
        {
            if (runtimeGlow == null) return;
            foreach (var m in runtimeGlow) if (m != null) Destroy(m);
        }
    }
}
