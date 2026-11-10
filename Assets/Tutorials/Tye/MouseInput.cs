using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseInput : MonoBehaviour
{
    //PSEUDO CODE: Declare public variable of type float for speed;

    public float speed = 50.0f;

    void OnMouseDrag()
    {
        //TRY:
        // Declare and Initialize the X and Y mouse axes and multiply by speed.
        // The value is in the range -1 to 1
        //Mouse Y will become Z translation

        float transX = Input.GetAxis("Mouse X") * speed;
        float transY = Input.GetAxis("Mouse Y") * speed;
        // Make translation variable move at 10 meters per second instead of 10 meters per frame with TimeDeltaTime...
        transX *= Time.deltaTime;
        transY *= Time.deltaTime;
        // use transform to translate with values above
        transform.Translate(transX, 0, transY);
        //Try Stretch Task in tutorial sheet
        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));

        transform.position = new Vector3(pos_move.x, transform.position.y, pos_move.z);
    }
}
//From: https://docs.unity3d.com/ScriptReference/Input.GetAxis.html