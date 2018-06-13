/*
 * Force character to throw a GameObject every few seconds.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_Aiming : MonoBehaviour {
    public Ammmmo ammo;
    public float throwDelay = 2;

    private float timeSinceThrow = 0;
    private Aiming aimScr;

	// Use this for initialization
	void OnEnable() {
        timeSinceThrow = 0;
        aimScr = GetComponent<Aiming>();
    }
	
	// Update is called once per frame
	void Update () {
		if (!aimScr.isActiveAndEnabled || aimScr.ammo == null) {
            timeSinceThrow += Time.deltaTime;
            if (timeSinceThrow >= throwDelay) {
                aimScr.enabled = true;
                aimScr.PickupAmmo(ammo);
                timeSinceThrow = 0;
            }
        }
	}
}
