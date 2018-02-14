using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {
    Movement moveScr;

	// Use this for initialization
	void Start () {
        moveScr = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") < 0)
            moveScr.WalkLeft();
        if (Input.GetAxis("Horizontal") > 0)
            moveScr.WalkRight();
        if (Input.GetButton("Jump"))
            moveScr.Jump();
    }
}
