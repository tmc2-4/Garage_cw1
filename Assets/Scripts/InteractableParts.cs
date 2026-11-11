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

    private static InteractableParts activeInspectedPart;
    private bool isThisPartBeingInspected = false;
    public float rotationSpeed = 5f;

    void Update()
    {
        if (isThisPartBeingInspected && Input.GetMouseButton(0))

        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.Rotate(Vector3.up, -mouseX, Space.World);
            transform.Rotate(Vector3.right, mouseY, Space.World);
        }
    }

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

        if (activeInspectedPart != null)
        {
            activeInspectedPart.isThisPartBeingInspected = false;
        }

        activeInspectedPart = this;
        isThisPartBeingInspected = true;

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

        if (activeInspectedPart !=null)
        {
            activeInspectedPart.isThisPartBeingInspected = false;
            activeInspectedPart = null;
        }
    }
}
