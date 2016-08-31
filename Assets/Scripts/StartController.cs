using UnityEngine;
using System.Collections;

public class StartController : MonoBehaviour {

    public GameObject mainCamera;
    public GameObject player;
    private CameraController cc;
    private Rigidbody prb;

    void Start()
    {
        cc = mainCamera.GetComponent<CameraController>();
        prb = player.GetComponent<Rigidbody>();

        StartGame();
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.R.ToString())))
        {
            ResetPosition();
        }
    }

    // ----

    private void StartGame()
    {
        ResetPosition();
        StartCoroutine(LookAround());
    }

    private IEnumerator LookAround()
    {
        prb.useGravity = false;
        for(int i=0; i<360; i++)
        {
            cc.Longitude(-1);

            yield return null;
        }
        prb.useGravity = true;
    }

    private void ResetPosition()
    {
        prb.Sleep();   // TODO これは…
        player.transform.position = transform.position + Vector3.up * 2;
        player.transform.rotation = Quaternion.identity;
        cc.FollowPlayer();
    }

}
