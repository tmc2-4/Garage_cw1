using UnityEngine;

public class InteractableParts : MonoBehaviour
{
    public string partName = "Left Thruster";
    public string partDescription = "This is the left thruster of the spaceship. It provides thrust and maneuverability in space.";

    public void InspectingPart()
    {
        Debug.Log($"Inspecting {partName}: {partDescription}");
    }


}
