using UnityEngine;
using System.Collections;

public partial class PlayerController : MonoBehaviour {

    public GameObject mc;
    public float power = 5;
    public float maxAV = 15;
    public float jump = 10;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = maxAV;
    }

    void Update ()
    {
        Quaternion yRotate = mc.GetComponent<CameraController>().getYRotate();

        // add torque
        int right = !Input.GetKey(KeyCode.D) ? Input.GetKey(KeyCode.A) ? -1 : 0 : 1;
        int forward = !Input.GetKey(KeyCode.W) ? Input.GetKey(KeyCode.S) ? -1 : 0 : 1;
        int yyy = !Input.GetKey(KeyCode.E)? Input.GetKey(KeyCode.Q)? -1 : 0 : 1;

        addTorque(right * power, yRotate * Vector3.back);
        addTorque(forward * power, yRotate * Vector3.right);
        addTorque(yyy * power, yRotate * Vector3.down);

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
