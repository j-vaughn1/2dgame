/*
 * Both characters call this code to move the RigidBody around.
 * TurnManager will "disable" this script to prevent players / AIs
 *     from moving on the other character's turn.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {
    public enum WalkDirection { None, Left, Right};
    private Rigidbody2D rigidBody;
    private bool tryJump;
    private WalkDirection walkDir;
    private bool walkDirSetThisFrame = false;
// UPDATE TEST
    public float WalkSpeed = 7f;
    public float Friction = 9f;
    public float JumpSpeed = 25f;

    public Vector2 knockBackRightSpeed = new Vector2(5,10);

    private Aiming aimScr;
    // List of contacts to tell which surface the character is touching...
    private GameObject floorObj = null;
    private GameObject wallLeftObj = null;
    private GameObject wallRightObj = null;
    private ContactPoint2D[] contacts = new ContactPoint2D[5]; // Check 5 contact points per collision

    public bool onFloor { get { return floorObj != null; } }
    public bool hitLeftWall { get { return wallLeftObj != null; } }
    public bool hitRightWall { get { return wallRightObj != null; } }

    private bool applyingKnockback = false;

    /**********************************************************/

    // Hacky method of changing which direction character is facing...
    private void Flip(float f) {
        GetComponent<CharacterManager>().Flip(f);
    }
    public void FaceLeft() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        Flip(-1);
    }
    public void FaceRight() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        Flip(1);
    }

    // Hacky method of detecting facing direction...
    public WalkDirection GetFacingDirection() {
        if (transform.localScale.x < 0)
            return WalkDirection.Left;
        return WalkDirection.Right;
    }

    /**********************************************************/

    // These are called by other scripts
    public void WalkLeft() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (aimScr && aimScr.ammo != null) // Stop if aiming projectile
            return;
        if (hitLeftWall)
            return;
        walkDir = WalkDirection.Left;
        walkDirSetThisFrame = true;
        rigidBody.velocity = new Vector2(-WalkSpeed, rigidBody.velocity.y); // Want to apply the walk speed right away
    }
    public void WalkRight() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (aimScr && aimScr.ammo != null) // Stop if aiming projectile
            return;
        if (hitRightWall)
            return;
        walkDir = WalkDirection.Right;
        walkDirSetThisFrame = true;
        rigidBody.velocity = new Vector2(WalkSpeed, rigidBody.velocity.y); // Want to apply the walk speed right away
    }
    public void Jump() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (aimScr && aimScr.ammo != null) // Stop if aiming projectile
            return;
        tryJump = true;
    }

    // When hit... move character back
    public void Knockback(WalkDirection knockbackDir) {
        if (knockbackDir == WalkDirection.None)
            knockbackDir = GetFacingDirection();

        if (knockbackDir == WalkDirection.Left) {
            rigidBody.velocity = new Vector2(-knockBackRightSpeed.x, knockBackRightSpeed.y);
        } else {
            rigidBody.velocity = knockBackRightSpeed;
        }
        applyingKnockback = true;
    }

    public WalkDirection getState() {
        return walkDir;
    }

    /*****************************************************/

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        aimScr = GetComponent<Aiming>();
        walkDir = WalkDirection.None;
        tryJump = false;
    }

    void OnCollisionStay2D(Collision2D coll) {
        if (rigidBody.velocity.y < 0.0001) {
            int numContacts = coll.GetContacts(contacts);
            for (int i = 0; i < numContacts; ++i) {
                // We don't break after first contact point because one game object could be a L piece: a floor & a wall at the same time
                if (contacts[i].normal.y > 0.9) {
                    floorObj = coll.gameObject;
                    applyingKnockback = false;
                } else if (contacts[i].normal.x > 0.9) {
                    wallLeftObj = coll.gameObject;
                } else if (contacts[i].normal.x < -0.9) {
                    wallRightObj = coll.gameObject;
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D coll) {
        if (coll.gameObject == floorObj)
            floorObj = null;
        if (coll.gameObject == wallLeftObj)
            wallLeftObj = null;
        if (coll.gameObject == wallRightObj)
            wallRightObj = null;
    }

    public void FixedUpdate() {
        if (!walkDirSetThisFrame)
            walkDir = WalkDirection.None;
        if (!applyingKnockback) { // Only allow movement if character isn't being knocked back
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
            if (onFloor && tryJump)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpSpeed);
        }
        tryJump = false;
        walkDirSetThisFrame = false;
    }
}
