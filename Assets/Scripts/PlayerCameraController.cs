using UnityEngine;
using System.Collections;

public class PlayerCameraController : MonoBehaviour {

    public GameObject player;
    public float speed = 3;

    private Vector3 offset;
    private Quaternion yRotate;

    void Start ()
    {
		// public v
		if (player == null)
			player = GameObject.Find ("Player");

		// private v
        offset = transform.position - player.transform.position;

        Vector3 projectedForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1));
        yRotate = Quaternion.FromToRotation(Vector3.forward, projectedForward);

		// init
		Latitude(30);
	}

    void Update ()
    {
        int right = !Input.GetKey(KeyCode.RightArrow) ? Input.GetKey(KeyCode.LeftArrow) ? -1 : 0 : 1;
        int forward = !Input.GetKey(KeyCode.UpArrow) ? Input.GetKey(KeyCode.DownArrow) ? -1 : 0 : 1;

        if (right != 0) Longitude(right * speed);
        if (forward != 0) Latitude(forward * speed);
    }
	
	void LateUpdate ()
    {
        FollowPlayer();
    }

    // ----- public -----

    public void FollowPlayer()
    {
        transform.position = player.transform.position + offset;
    }

    public Quaternion GetYRotate() { return yRotate; }

    // 経度 +right -left
    public void Longitude(float angle)
    {
        Vector3 axis = Vector3.down;
        transform.RotateAround(player.transform.position, axis, angle);

        Quaternion rotate = Quaternion.AngleAxis(angle, axis);
        offset = rotate * offset;
        yRotate = rotate * yRotate;
    }

    // 緯度 +up -down
    public void Latitude(float angle)
    {
        Vector3 axis = GetYRotate() * Vector3.right;
        transform.RotateAround(player.transform.position, axis, angle);

        Quaternion rotate = Quaternion.AngleAxis(angle, axis);
        offset = rotate * offset;
    }

}
