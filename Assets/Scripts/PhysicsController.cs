
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        rb.AddForce(movement * moveForce, ForceMode.Force);
    }
}
