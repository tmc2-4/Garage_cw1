// The Outpost Garage - On-screen interface
// Author: Divya Patel (W1995126), Group [No.]
// Written with assistance from Claude (Anthropic), 27 Oct 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in individual GitHub folder.
//
// Builds a compact control panel at runtime (uGUI) and connects it to the garage systems:
// exploded-view slider + assemble button, paint swatches, engine button, reset-view button and a help bar.
// Layout is sized for a 1920x1080 recording (Canvas Scaler reference resolution).

using UnityEngine;
using UnityEngine.UI;

namespace OutpostGarage
{
    public class GarageUI : MonoBehaviour
    {
        public InspectionCamera inspectionCamera;
        public ExplodedView explodedView;
        public PaintController paint;
        public EngineController engine;

        static readonly Color Panel = new Color(0.04f, 0.06f, 0.08f, 0.82f);
        static readonly Color Cyan = new Color(0.2f, 0.85f, 1f);
        static readonly Color Amber = new Color(1f, 0.62f, 0.1f);
        static readonly Color ButtonCol = new Color(0.12f, 0.16f, 0.2f, 1f);

        Font font;
        Slider explodeSlider;
        Text explodeValue, paintLabel, engineLabel, engineStatus;
        Image engineButtonImage;
        Image[] swatchFrames;

        void Start()
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var canvasGO = new GameObject("GarageCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGO.transform.SetParent(transform, false);
            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            var root = canvasGO.transform;

            // ---- Title (top-left) ----
            var title = MakePanel(root, "TitlePanel", new Vector2(0, 1), new Vector2(30, -30), new Vector2(520, 110));
            MakeText(title, "THE OUTPOST GARAGE", 38, Cyan, new Vector2(24, -18), new Vector2(480, 46), FontStyle.Bold);
            MakeText(title, "Hover vehicle inspection & service bay", 22, new Color(0.8f, 0.85f, 0.9f), new Vector2(24, -66), new Vector2(480, 30));
            MakeBar(title, Amber, new Vector2(0, 0), new Vector2(520, 4));

            // ---- Control panel (right) ----
            var panel = MakePanel(root, "ControlPanel", new Vector2(1, 1), new Vector2(-30, -30), new Vector2(420, 610));
            float y = -22;
            MakeText(panel, "EXPLODED VIEW", 22, Cyan, new Vector2(24, y), new Vector2(260, 30), FontStyle.Bold);
            explodeValue = MakeText(panel, "0%", 22, Color.white, new Vector2(300, y), new Vector2(96, 30), FontStyle.Bold, TextAnchor.UpperRight);
            y -= 44;
            explodeSlider = MakeSlider(panel, new Vector2(24, y), new Vector2(372, 28));
            explodeSlider.onValueChanged.AddListener(v => explodedView.SetAmount(v));
            y -= 46;
            MakeButton(panel, "ASSEMBLE", new Vector2(24, y), new Vector2(372, 48), () => { explodedView.Assemble(); explodeSlider.SetValueWithoutNotify(0f); });

            y -= 78;
            MakeText(panel, "PAINT", 22, Cyan, new Vector2(24, y), new Vector2(200, 30), FontStyle.Bold);
            paintLabel = MakeText(panel, "", 20, Color.white, new Vector2(150, y + 2), new Vector2(246, 30), FontStyle.Normal, TextAnchor.UpperRight);
            y -= 42;
            int n = paint.options.Length;
            swatchFrames = new Image[n];
            float w = (372f - (n - 1) * 12f) / n;
            for (int i = 0; i < n; i++)
            {
                int idx = i;
                var frame = MakeImage(panel, "Swatch" + i, new Vector2(24 + i * (w + 12), y), new Vector2(w, 60), Color.white);
                var inner = MakeImage(frame.transform, "Colour", new Vector2(4, -4), new Vector2(w - 8, 52), paint.options[i].color);
                var btn = frame.gameObject.AddComponent<Button>();
                btn.targetGraphic = inner;
                btn.onClick.AddListener(() => paint.Select(idx));
                MakeText(frame.transform, (i + 1).ToString(), 18, new Color(0, 0, 0, 0.7f), new Vector2(8, -6), new Vector2(30, 24), FontStyle.Bold);
                swatchFrames[i] = frame;
            }

            y -= 96;
            MakeText(panel, "ENGINE", 22, Cyan, new Vector2(24, y), new Vector2(200, 30), FontStyle.Bold);
            y -= 42;
            var eb = MakeButton(panel, "START ENGINE  [E]", new Vector2(24, y), new Vector2(372, 56), () => engine.Toggle());
            engineButtonImage = eb.GetComponent<Image>();
            engineLabel = eb.GetComponentInChildren<Text>();
            y -= 66;
            engineStatus = MakeText(panel, "", 19, Amber, new Vector2(24, y), new Vector2(372, 52), FontStyle.Normal);

            y -= 70;
            MakeButton(panel, "RESET VIEW  [R]", new Vector2(24, y), new Vector2(372, 48), () => inspectionCamera.ResetView());

            // ---- Help bar (bottom) ----
            var help = MakePanel(root, "HelpBar", new Vector2(0.5f, 0), new Vector2(0, 30), new Vector2(1180, 54), new Vector2(0.5f, 0));
            MakeText(help, "Drag: orbit     Wheel: zoom     R: reset view     E: engine     1-4: paint     Slider: explode", 22,
                new Color(0.85f, 0.9f, 0.95f), new Vector2(0, -12), new Vector2(1180, 30), FontStyle.Normal, TextAnchor.UpperCenter);

            explodedView.OnAmountChanged += a =>
            {
                explodeValue.text = Mathf.RoundToInt(a * 100f) + "%";
                // keep the slider in step if the amount was changed by something other than the slider (e.g. Assemble)
                if (!Mathf.Approximately(explodeSlider.value, explodedView.targetAmount))
                    explodeSlider.SetValueWithoutNotify(explodedView.targetAmount);
            };
            paint.OnPaintChanged += _ => RefreshPaint();
            engine.OnStateChanged += RefreshEngine;
            RefreshPaint(); RefreshEngine();
        }

        void RefreshPaint()
        {
            paintLabel.text = paint.SelectedName;
            for (int i = 0; i < swatchFrames.Length; i++)
                swatchFrames[i].color = i == paint.SelectedIndex ? Cyan : new Color(0.25f, 0.28f, 0.32f);
        }

        void RefreshEngine()
        {
            switch (engine.State)
            {
                case EngineState.Off: engineLabel.text = "START ENGINE  [E]"; engineButtonImage.color = ButtonCol; break;
                case EngineState.Starting: engineLabel.text = "STARTING..."; engineButtonImage.color = new Color(0.45f, 0.3f, 0.05f); break;
                case EngineState.Running: engineLabel.text = "STOP ENGINE  [E]"; engineButtonImage.color = new Color(0.05f, 0.4f, 0.5f); break;
                case EngineState.ShuttingDown: engineLabel.text = "SHUTTING DOWN..."; engineButtonImage.color = new Color(0.3f, 0.2f, 0.1f); break;
            }
            string state = engine.State.ToString().ToUpper();
            engineStatus.text = string.IsNullOrEmpty(engine.Message) ? "Status: " + state : engine.Message;
        }

        // ---------- small UI helpers ----------
        RectTransform MakePanel(Transform parent, string name, Vector2 anchor, Vector2 pos, Vector2 size, Vector2? pivot = null)
        {
            var img = MakeImage(parent, name, pos, size, Panel);
            var rt = img.rectTransform;
            rt.anchorMin = rt.anchorMax = anchor;
            rt.pivot = pivot ?? anchor;
            rt.anchoredPosition = pos;
            return rt;
        }

        Image MakeImage(Transform parent, string name, Vector2 pos, Vector2 size, Color c)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1); rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = pos; rt.sizeDelta = size;
            var img = go.GetComponent<Image>(); img.color = c;
            return img;
        }

        void MakeBar(Transform parent, Color c, Vector2 pos, Vector2 size)
        {
            var img = MakeImage(parent, "Accent", pos, size, c);
            img.rectTransform.anchorMin = img.rectTransform.anchorMax = new Vector2(0, 0);
            img.rectTransform.pivot = new Vector2(0, 0);
            img.raycastTarget = false;
        }

        Text MakeText(Transform parent, string s, int size, Color c, Vector2 pos, Vector2 box, FontStyle style = FontStyle.Normal, TextAnchor anchor = TextAnchor.UpperLeft)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1); rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = pos; rt.sizeDelta = box;
            var t = go.GetComponent<Text>();
            t.font = font; t.fontSize = size; t.color = c; t.text = s; t.fontStyle = style; t.alignment = anchor;
            t.raycastTarget = false; t.horizontalOverflow = HorizontalWrapMode.Wrap; t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }

        Button MakeButton(Transform parent, string label, Vector2 pos, Vector2 size, UnityEngine.Events.UnityAction onClick)
        {
            var img = MakeImage(parent, "Button_" + label, pos, size, ButtonCol);
            var btn = img.gameObject.AddComponent<Button>();
            var cb = btn.colors; cb.highlightedColor = new Color(1.25f, 1.25f, 1.25f); cb.pressedColor = new Color(0.8f, 0.8f, 0.8f); btn.colors = cb;
            btn.onClick.AddListener(onClick);
            var t = MakeText(img.transform, label, 21, Color.white, new Vector2(0, -size.y / 2 + 13), new Vector2(size.x, 28), FontStyle.Bold, TextAnchor.UpperCenter);
            var outline = img.gameObject.AddComponent<Outline>(); outline.effectColor = new Color(Cyan.r, Cyan.g, Cyan.b, 0.5f);
            return btn;
        }

        Slider MakeSlider(Transform parent, Vector2 pos, Vector2 size)
        {
            var bg = MakeImage(parent, "ExplodeSlider", pos, size, new Color(0.15f, 0.18f, 0.22f));
            var slider = bg.gameObject.AddComponent<Slider>();
            var fillArea = new GameObject("Fill Area", typeof(RectTransform)); fillArea.transform.SetParent(bg.transform, false);
            var fa = (RectTransform)fillArea.transform; fa.anchorMin = new Vector2(0, 0); fa.anchorMax = new Vector2(1, 1); fa.offsetMin = new Vector2(6, 6); fa.offsetMax = new Vector2(-6, -6);
            var fill = new GameObject("Fill", typeof(RectTransform), typeof(Image)); fill.transform.SetParent(fillArea.transform, false);
            fill.GetComponent<Image>().color = Cyan;
            var fr = (RectTransform)fill.transform; fr.anchorMin = Vector2.zero; fr.anchorMax = new Vector2(0, 1); fr.sizeDelta = Vector2.zero;
            var handleArea = new GameObject("Handle Area", typeof(RectTransform)); handleArea.transform.SetParent(bg.transform, false);
            var ha = (RectTransform)handleArea.transform; ha.anchorMin = Vector2.zero; ha.anchorMax = Vector2.one; ha.offsetMin = new Vector2(12, 0); ha.offsetMax = new Vector2(-12, 0);
            var handle = new GameObject("Handle", typeof(RectTransform), typeof(Image)); handle.transform.SetParent(handleArea.transform, false);
            handle.GetComponent<Image>().color = Color.white;
            var hr = (RectTransform)handle.transform; hr.anchorMin = new Vector2(0, 0); hr.anchorMax = new Vector2(0, 1); hr.sizeDelta = new Vector2(22, 10);
            slider.fillRect = fr; slider.handleRect = hr; slider.targetGraphic = handle.GetComponent<Image>();
            slider.minValue = 0; slider.maxValue = 1; slider.value = 0;
            return slider;
        }
    }
}
