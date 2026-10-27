// The Outpost Garage - Labelled maintenance/demo station for CW1 tutorial contributions
// Author: Divya Patel (W1995126), Group [No.]
// Written with assistance from Claude (Anthropic), 27 Oct 2026.
// AI use permitted by J. Ferguson (email, 27 Oct 2026). Transcript in individual GitHub folder.
//
// A station only displays its label and status. The actual tutorial work (each student's own script and
// objects) is placed as a child of the station's "Content" object by that student, so authorship stays clear.

using UnityEngine;

namespace OutpostGarage
{
    public class DemoStation : MonoBehaviour
    {
        public string stationTitle = "Station";
        public string contributor = "[Student name]";
        [TextArea] public string status = "PLACEHOLDER - awaiting contribution";
        public bool isPlaceholder = true;
        public TextMesh titleText;
        public TextMesh detailText;

        void OnValidate() { Refresh(); }
        void Start() { Refresh(); }

        public void Refresh()
        {
            if (titleText != null) titleText.text = stationTitle;
            if (detailText != null)
            {
                detailText.text = contributor + "\n" + status;
                detailText.color = isPlaceholder ? new Color(1f, 0.62f, 0.1f) : new Color(0.3f, 0.95f, 1f);
            }
        }
    }
}
