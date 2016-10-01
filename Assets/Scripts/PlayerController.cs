using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerController : MonoBehaviour {

	public GameObject playerCamera;
	public AudioClip seJump;
	public AudioClip seCollision;
    // value
    public float torque = 5;
	public float maxAV = 15;
	public float jumpPower = 10;
	public float boostPower = 1;
    public float cooltime = 1;
	public int jumpFlame = 5;

    private Rigidbody rb;
	private List<Vector3> jumpNormals = new List<Vector3>();
	private bool isJump = false;
	private int isJumpCount = 0;
	private bool canJump = true;

    void Start()
    {
		// public v
		if (playerCamera == null)
			playerCamera = GameObject.Find ("PlayerCamera");

		// private v
        rb = GetComponent<Rigidbody>();

		// init
        rb.maxAngularVelocity = maxAV;
    }

    void Update ()
    {
        Quaternion yRotate = playerCamera.GetComponent<PlayerCameraController>().GetYRotate();

        // add torque
        int right = !Input.GetKey(KeyCode.D) ? Input.GetKey(KeyCode.A) ? -1 : 0 : 1;
        int forward = !Input.GetKey(KeyCode.W) ? Input.GetKey(KeyCode.S) ? -1 : 0 : 1;
        int yyy = !Input.GetKey(KeyCode.E)? Input.GetKey(KeyCode.Q)? -1 : 0 : 1;

        AddTorque(right * torque, yRotate * Vector3.back);
        AddTorque(forward * torque, yRotate * Vector3.right);
        AddTorque(yyy * torque, yRotate * Vector3.down);

        // jump
        AddJump();

		// isJumpCount
		if (isJumpCount == 0) {
			isJump = false;
		} else {
			isJumpCount--;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			isJump = true;
			isJumpCount = jumpFlame;
		}
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        List<Vector3> normals = collisionInfo.contacts
            .Select(cp => cp.normal)
            .ToList();
        Vector3 normalSum = -normals.Aggregate((a, b) => a + b).normalized;

        // before jump
		if (canJump && isJump)
        {
            jumpNormals.AddRange(normals);
        }

        // play se
        PlayClipAtPoint(seCollision, normalSum);

        // debug
        //DebugCollision(collisionInfo, Color.red);
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        List<Vector3> normals = collisionInfo.contacts
            .Select(cp => cp.normal)
            .ToList();

        // before jump
		if (canJump && isJump)
        {
            jumpNormals.AddRange(normals);
        }

        // debug
        //DebugCollision(collisionInfo);
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

        //PhysicMaterial pm = col.material;
		//TODO consider friction for boost
        Vector3 normalSum = -jumpNormals.Aggregate((a, b) => a + b).normalized;
		jumpNormals.Clear();
		Vector3 before = Quaternion.AngleAxis(90, normalSum) * rb.angularVelocity;
		Vector3 boostVecter = GetProjectedVector(before, normalSum);

		rb.AddForce(normalSum * jumpPower + boostVecter * boostPower, ForceMode.Impulse);

        PlayClipAtPoint(seJump, normalSum);

        StartCoroutine(Cooling());
    }

	private Vector3 GetProjectedVector(Vector3 before, Vector3 normal) 
	{
		// get projected vector
		float angle = 0.0f;
		Vector3 norm3 = Vector3.zero;
		Quaternion.FromToRotation (normal, before).ToAngleAxis (out angle, out norm3);
		Vector3 projected = Quaternion.AngleAxis (90 - angle, norm3) * before;

		// projection
		return Vector3.Dot(before, projected) / projected.magnitude / projected.magnitude * projected;
	}

    private IEnumerator Cooling()
    {
        canJump = false;

        yield return new WaitForSeconds(cooltime);

        canJump = true;
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
