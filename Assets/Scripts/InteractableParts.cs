using UnityEngine;
using UnityEngine.UI;

public class InteractableParts : MonoBehaviour
{
    public string partName; [TextArea(3, 10)]
    public string partDescription;

    
    private static Text inspectUItext;
    private static GameObject panelObj;
    private static Transform inspectionCamPos;
    private static Vector3 originalCamPos;
    private static Quaternion originalCamRot;
    private static bool hasStoredCamera = false;


    void Start()
    {
        if (inspectUItext == null)
        {
            GameObject uiObj = GameObject.Find("InspectionText");
            if (uiObj != null) inspectUItext = uiObj.GetComponent<Text>();
  
        }

        if (panelObj == null)
        {
            GameObject pObj = GameObject.Find("InspectionPanel");
            if (pObj != null)
            {
                panelObj = pObj;
                panelObj.SetActive(false);
            }
        }

        if(inspectionCamPos == null)
        {
            GameObject camObj = GameObject.Find("InspectionCameraPos");
            if (camObj != null) inspectionCamPos = camObj.transform;
        }
    }

    public void InspectingPart()
    {
        Debug.Log($"Inspecting {partName}: {partDescription}");

        if (panelObj != null) panelObj.SetActive(true);
        if(inspectUItext != null) inspectUItext.text = $"{partName}\n{partDescription}";

        if (inspectionCamPos != null && Camera.main != null && !hasStoredCamera)
        {
            originalCamPos = Camera.main.transform.position;
            originalCamRot = Camera.main.transform.rotation;
            hasStoredCamera = true;
        }

        if (inspectionCamPos != null && Camera.main != null)
        {
            Camera.main.transform.position = inspectionCamPos.position;
            Camera.main.transform.rotation = inspectionCamPos.rotation;
        }

        FirstPersonController.isInspecting = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }

    public static void RestoreCamera()
    {
        if (hasStoredCamera && Camera.main != null)
        {
            Camera.main.transform.position = originalCamPos;
            Camera.main.transform.rotation = originalCamRot;
            hasStoredCamera = false;
        }
    }
}
