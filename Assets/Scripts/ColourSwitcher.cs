using UnityEngine;

public class ColourSwitcher : MonoBehaviour
{
    public MeshRenderer targetRenderer;
    public Color[] myColors;

    private int currentIndex = 0;
    private Material activeMaterial;

    void Start()
    {
        if (targetRenderer != null)
        {
            activeMaterial = targetRenderer.material;
        
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeToNextColor();
        }
    }

    void ChangeToNextColor()
    {
        if (myColors == null || myColors.Length == 0) return;
        
   
        currentIndex = (currentIndex + 1) % myColors.Length;

        activeMaterial.color = myColors[currentIndex];
    }
}
