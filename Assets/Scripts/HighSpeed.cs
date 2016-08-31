using UnityEngine;
using System.Collections;

public class HighSpeed : MonoBehaviour {

    public GameObject player;
    public float power = 2;

    private Rigidbody prb;
    private Rigidbody hrb;

    void Start()
    {
        prb = player.GetComponent<Rigidbody>();
        hrb = GetComponent<Rigidbody>();

        hrb.maxAngularVelocity = prb.maxAngularVelocity * power;
    }

    void Update()
    {
        hrb.angularVelocity = prb.angularVelocity * power;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position;

    }

}
