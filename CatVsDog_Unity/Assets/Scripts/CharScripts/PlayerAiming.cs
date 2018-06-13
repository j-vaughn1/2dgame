using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour {
    Aiming aimScr;

    void OnCollisionEnter2D(Collision2D coll) {
        if (!this.isActiveAndEnabled) // Don't throw bones when disabled
            return;

        if (coll.gameObject.tag == "Ammo-Static" && aimScr.ammo == null) {
            print("Picking up:"+coll.gameObject);
            aimScr.PickupAmmo(coll.gameObject.GetComponent<Ammmmo>());
        }
    }

    /*********************************************/

    // Use this for initialization
    void Start () {
        aimScr = GetComponent<Aiming>();
    }
	
	// Update is called once per frame
	void Update () {
        // Aiming...
        if (!aimScr.isAngleLocked) {
            if (Input.GetButtonDown("Throw")) // Lock throw angle
                aimScr.LockAimingAngle();
            // Change angle
            else if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0)
                aimScr.MoveToAimingAngle(aimScr.maxAngle);
            else if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") < 0)
                aimScr.MoveToAimingAngle(aimScr.minAngle);
            // Not changing angle
            else
                aimScr.MoveToAimingAngle(aimScr.currAngle);

            // Throw ammo
        } else {
            if (Input.GetButtonDown("Throw"))
                aimScr.Throw();
        }
	}
}
