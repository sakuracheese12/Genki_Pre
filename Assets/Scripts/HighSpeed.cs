using UnityEngine;
using System.Collections;

public class HighSpeed : MonoBehaviour {

    public Transform plt;
    
	// Update is called once per frame
	void Update () {
        transform.position = plt.position;
        transform.rotation = plt.rotation;
    }
}
