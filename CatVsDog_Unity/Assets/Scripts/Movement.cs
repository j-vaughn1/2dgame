using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {
    private enum WalkDirection { None, Left, Right};
    private Rigidbody2D rigidBody;
    private bool onFloor;
    private bool tryJump;
    private WalkDirection walkDir;

    public float WalkSpeed = 5f;
    public float Friction = 9f;
    public float JumpSpeed = 30f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        walkDir = WalkDirection.None;
        onFloor = false;
        tryJump = false;
    }

    void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.tag == "Floor" && rigidBody.velocity.y < 0.0001)
            onFloor = true;
    }
    void OnCollisionExit2D(Collision2D coll) {
        if (coll.gameObject.tag == "Floor")
            onFloor = false;
    }

    public void FixedUpdate() {
        switch (walkDir) {
        case WalkDirection.Left:
            rigidBody.velocity = new Vector2(-WalkSpeed, rigidBody.velocity.y);
            break;
        case WalkDirection.Right:
            rigidBody.velocity = new Vector2(WalkSpeed, rigidBody.velocity.y);
            break;
        case WalkDirection.None:
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            break;
        }
        walkDir = WalkDirection.None;
        if (onFloor && tryJump)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpSpeed);
        tryJump = false;
    }
	
	// These are called by other scripts
	public void WalkLeft() {
        walkDir = WalkDirection.Left;
        rigidBody.velocity = new Vector2(-WalkSpeed, rigidBody.velocity.y); // Want to apply the walk speed right away
    }
    public void WalkRight() {
        walkDir = WalkDirection.Right;
        rigidBody.velocity = new Vector2(WalkSpeed, rigidBody.velocity.y); // Want to apply the walk speed right away
    }
    public void Jump() {
        tryJump = true;
    }
}
