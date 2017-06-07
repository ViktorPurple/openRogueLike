using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    private Camera myCam;

	// Use this for initialization
	void Start () {

        myCam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        myCam.orthographicSize = (Screen.height / 32f) / 10f;

        if (target)
        {
            
            transform.position = Vector3.Lerp(transform.position, target.position, 0.08f) + new Vector3(0, 0, -10);
        }
	
	}
}
