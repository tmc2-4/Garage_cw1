using UnityEngine;

public class animY : MonoBehaviour
{
    void OnMouseDown()
    {
        this.GetComponent<Animator>().Play("practice2");
    }
}
