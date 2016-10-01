using UnityEngine;
using System.Collections;

public class StartController : MonoBehaviour {

	public GameObject playerCamera;
    public GameObject player;
	public bool isLookAround = true;

	private PlayerCameraController cc;
    private Rigidbody prb;

    void Start()
	{
		// public v
		if (playerCamera == null)
			playerCamera = GameObject.Find ("PlayerCamera");
		if (player == null)
			player = GameObject.Find ("Player");

		// private v
		cc = playerCamera.GetComponent<PlayerCameraController>();
        prb = player.GetComponent<Rigidbody>();

		// init
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
		if(isLookAround)
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
