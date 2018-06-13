using UnityEngine;
using System.Collections;


public class AI_Movement : MonoBehaviour
{
    private Transform boneTransform;
    private Movement pMovement;
    private float walkingDistance;

    private Vector2 prevPos;
    private float stuckTime = 0;

    private Transform targetAmmo = null;
    private Movement.WalkDirection walkDirection = Movement.WalkDirection.None;

    void FindAmmo() {
        if (targetAmmo)
            return;
        
        Vector3 enemyPos = GetComponent<AI_Aiming>().target.position;
        // Find the ammo furthest from the Cat
        float ammoDistance = 0;
        foreach (GameObject ammo in GameObject.FindGameObjectsWithTag("Ammo-Static")) {
            float distance = Vector2.Distance(ammo.transform.position, enemyPos); // Distance in 2D
            if (distance > ammoDistance) {
                targetAmmo = ammo.transform;
                ammoDistance = distance;
            }
        }

        // Initially walk towards the ammo
        if (targetAmmo) {
            walkingDistance = targetAmmo.position.x - transform.position.x;
            if (walkingDistance < 0)
                walkDirection = Movement.WalkDirection.Left;
            else
                walkDirection = Movement.WalkDirection.Right;
        }
    }

    public void Walk()
    {
        pMovement = GetComponent<Movement>();
        if (targetAmmo != null) {
            walkingDistance = targetAmmo.position.x - transform.position.x;

            // If walked too far, go back
            if (walkingDistance > 3 && walkDirection == Movement.WalkDirection.Left)
                walkDirection = Movement.WalkDirection.Right;
            else if (walkingDistance < -3 && walkDirection == Movement.WalkDirection.Right)
                walkDirection = Movement.WalkDirection.Left;
            
            // Just keep walking towards Ammo
            if (walkDirection == Movement.WalkDirection.Left)
                pMovement.WalkLeft();
            else if (walkDirection == Movement.WalkDirection.Right)
                pMovement.WalkRight();

            // If stuck, jump
            if (walkingDistance != 0 && transform.position.x == prevPos.x) {
                stuckTime += Time.deltaTime;
                if (stuckTime > .2f) {
                    pMovement.Jump();
                    stuckTime = 0;
                }
            } else {
                stuckTime = 0;
            }
        }
    }

    void OnEnable() {
        prevPos = transform.position;
        stuckTime = 0;
    }

    void Update()
    {
        FindAmmo();
        Walk();
        prevPos = transform.position;
    }
}
