using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharController : MonoBehaviour
{
    //PSEUDO CODE: Declare float variables for speed, maxSpeed, acceleration, deceleration, rotation.
    public float speed = 10.0f;
    public float maxSpeed = 100.0f;
    public float acc = 1.0f;
    public float dec = 1.0f;
    public float rot;
   
    // Update is called once per frame.
    void Update()
    {
        // PSEUDO CODE: Check for space key input. If not max speed, accelerate, else decelerate.
        if (Input.GetKey("space"))
        {
            if (speed < maxSpeed)
                speed += acc;
            
        }
        else
        {
            if (speed > 0)
                speed -= dec;
            
        }
        // PSEUDO CODE: Use the speed and transform to move the object forward.
        transform.position += transform.forward * Time.deltaTime * speed;
        // FUNCTION: Simplest transform with ACC, DEC, Speed (space bar), and TDT.
        // PSEUDO CODE: Check for left and right key inputs. Turn if left or right keys are pressed.
        if ((Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.Rotate(0,2.0f,0);
        }
        if ((Input.GetKey(KeyCode.RightArrow)))
        {
            transform.Rotate(0,-2.0f, 0);
        }
    }
}