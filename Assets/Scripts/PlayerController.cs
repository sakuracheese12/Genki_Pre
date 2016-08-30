using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject mc;
    public float speed = 50;
    public float jump = 10;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        Quaternion yRotate = mc.GetComponent<CameraController>().getYRotate();

        // add torque
        int right = !Input.GetKey(KeyCode.D) ? Input.GetKey(KeyCode.A) ? -1 : 0 : 1;
        int forward = !Input.GetKey(KeyCode.W) ? Input.GetKey(KeyCode.S) ? -1 : 0 : 1;
        int yyy = !Input.GetKey(KeyCode.E)? Input.GetKey(KeyCode.Q)? -1 : 0 : 1;

        addTorque(right * speed, yRotate * Vector3.back);
        addTorque(forward * speed, yRotate * Vector3.right);
        addTorque(yyy * speed, yRotate * Vector3.down);

        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, -jump, 0), ForceMode.Impulse);
        }
    }

    // ----

    private void addTorque(float power, Vector3 axis)
    {
        if (power == 0) return;
        rb.AddTorque (axis.normalized * power, ForceMode.Force);
    }

}
