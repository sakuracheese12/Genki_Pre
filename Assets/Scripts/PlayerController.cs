using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerController : MonoBehaviour {

    public GameObject mainCamera;
    public AudioClip seJump;
    public AudioClip seCollision;
    // value
    public float torque = 5;
    public float maxAV = 15;
    public float jumpPower = 10;
    public float contactOffset = 0.01f;
    public float cooltime = 1;

    private Rigidbody rb;
    private List<Vector3> jumpNormals = new List<Vector3>();
    private bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = maxAV;
        GetComponent<Collider>().contactOffset = contactOffset;
    }

    void Update ()
    {
        Quaternion yRotate = mainCamera.GetComponent<CameraController>().getYRotate();

        // add torque
        int right = !Input.GetKey(KeyCode.D) ? Input.GetKey(KeyCode.A) ? -1 : 0 : 1;
        int forward = !Input.GetKey(KeyCode.W) ? Input.GetKey(KeyCode.S) ? -1 : 0 : 1;
        int yyy = !Input.GetKey(KeyCode.E)? Input.GetKey(KeyCode.Q)? -1 : 0 : 1;

        AddTorque(right * torque, yRotate * Vector3.back);
        AddTorque(forward * torque, yRotate * Vector3.right);
        AddTorque(yyy * torque, yRotate * Vector3.down);

        // jump
        AddJump();
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        List<Vector3> normals = collisionInfo.contacts
            .Select(cp => cp.normal)
            .ToList();
        Vector3 normalSum = -normals.Aggregate((a, b) => a + b).normalized;

        // before jump
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            jumpNormals.AddRange(normals);
        }

        // play se
        PlayClipAtPoint(seCollision, normalSum);

        // debug
        DebugCollision(collisionInfo, Color.red);
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        List<Vector3> normals = collisionInfo.contacts
            .Select(cp => cp.normal)
            .ToList();

        // before jump
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            jumpNormals.AddRange(normals);
        }

        // debug
        DebugCollision(collisionInfo);
    }

    // ---- private ----

    private void AddTorque(float torque, Vector3 axis)
    {
        if (torque == 0) return;
        rb.AddTorque(axis.normalized * torque, ForceMode.Force);
    }

    private void AddJump()
    {
        if (jumpNormals.Count == 0) return;

        Vector3 normalSum = -jumpNormals.Aggregate((a, b) => a + b).normalized;

        rb.AddForce(normalSum * jumpPower, ForceMode.Impulse);
        PlayClipAtPoint(seJump, normalSum);
        
        jumpNormals.Clear();

        StartCoroutine(Cooling());
    }

    private IEnumerator Cooling()
    {
        canJump = false;
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(cooltime);

        canJump = true;
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void PlayClipAtPoint(AudioClip clip, Vector3 localPosition)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position + localPosition);
    }

    // ---- debug ----

    private void DebugCollision(Collision collisionInfo)
    {
        DebugCollision(collisionInfo, Color.white);
    }
    private void DebugCollision(Collision collisionInfo, Color color)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 10, color);
        }
    }

}
