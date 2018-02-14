using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    private Rigidbody2D rigidBody;
    private bool onFloor;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        onFloor = false;
    }

    void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.tag == "Floor")
            onFloor = true;
        print("hit:" + coll.gameObject.tag);
    }
    void OnCollisionExit2D(Collision2D coll) {
        if (coll.gameObject.tag == "Floor")
            onFloor = false;
        print("left:"+ coll.gameObject.tag);
    }
	
	// Update is called once per frame
	public void WalkLeft() {
        rigidBody.velocity = new Vector2(-5, rigidBody.velocity.y);
    }
    public void WalkRight() {
        rigidBody.velocity = new Vector2(5, rigidBody.velocity.y);
    }
    public void Jump() {
        if (onFloor)
            rigidBody.velocity += new Vector2(0, 5);
    }
}
