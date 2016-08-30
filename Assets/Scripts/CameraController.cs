using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float speed = 3;

    private Vector3 offset;
    private Quaternion yRotate;

    void Start ()
    {
        offset = transform.position - player.transform.position;

        Vector3 projectedForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1));
        yRotate = Quaternion.FromToRotation(Vector3.forward, projectedForward);
	}

    void Update ()
    {
        int right = !Input.GetKey(KeyCode.RightArrow) ? Input.GetKey(KeyCode.LeftArrow) ? -1 : 0 : 1;
        int forward = !Input.GetKey(KeyCode.UpArrow) ? Input.GetKey(KeyCode.DownArrow) ? -1 : 0 : 1;

        if (right != 0) longitude(right * speed);
        if (forward != 0) latitude(forward * speed);
    }
	
	void LateUpdate ()
    {
        transform.position = player.transform.position + offset;
    }

    // ----- public -----

    public Quaternion getYRotate() { return yRotate; }
    
    // ---- private ----

    // 経度 +right -left
    private void longitude(float angle)
    {
        Vector3 axis = Vector3.down;
        transform.RotateAround(player.transform.position, axis, angle);

        Quaternion rotate = Quaternion.AngleAxis(angle, axis);
        offset = rotate * offset;
        yRotate = rotate * yRotate;
    }

    // 緯度 +up -down
    private void latitude(float angle)
    {
        Vector3 axis = getYRotate() * Vector3.right;
        transform.RotateAround(player.transform.position, axis, angle);

        Quaternion rotate = Quaternion.AngleAxis(angle, axis);
        offset = rotate * offset;
    }

}
