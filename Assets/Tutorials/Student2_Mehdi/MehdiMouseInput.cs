using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MehdiMouseInput : MonoBehaviour
{
    public float speed = 50.0f;

    void Update()
    {

    }

    void OnMouseDrag2()
    {
        float transX = Input.GetAxis("Mouse X") * speed;
        float transZ = Input.GetAxis("Mouse Y") * speed;

        transX += Time.deltaTime;
        transZ += Time.deltaTime;

        transform.Translate(transX, 0, transZ);
    }
}
