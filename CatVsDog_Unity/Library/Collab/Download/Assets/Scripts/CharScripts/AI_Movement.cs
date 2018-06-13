using UnityEngine;
using System.Collections;


public class AI_Movement : MonoBehaviour
{
    private Transform boneTransform;
    private Movement pMovement;
    private float walkingDistance;

    private Vector2 prevPos;
    private float stuckTime = 0;

    public void Walk()
    {
        pMovement = GetComponent<Movement>();
        // Find the closest ammo
        Transform closestAmmo = null;
        float ammoDistance = Mathf.Infinity;
        foreach (GameObject ammo in GameObject.FindGameObjectsWithTag("Ammo-Static")) {
            float distance = Vector2.Distance(ammo.transform.position, this.transform.position); // Distance in 2D
            if (distance < ammoDistance) {
                closestAmmo = ammo.transform;
                ammoDistance = distance;
            }
        }

        walkingDistance = closestAmmo.transform.position.x - transform.position.x;
        
        if (walkingDistance < 0)
        {
            pMovement.WalkLeft();
        }
        else if (walkingDistance > 0)
        {
            pMovement.WalkRight();
        }

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
        prevPos = transform.position;
    }

    void OnEnable() {
        prevPos = transform.position;
        stuckTime = 0;
    }

    void Update()
    {
        Walk();
    }
}
