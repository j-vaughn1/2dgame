using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Aiming : MonoBehaviour {
    public Transform target;
    public float timeHit = 2.9f; // in second
    public float accuracyDiviation = 2f;

    private Aiming aimScr;
    private Vector3 throwPoint { // Position the bone is thrown from, as an absolute position
        get {  return transform.position + aimScr.ammoGrabPos; }
    }
    private float throwAngle;
    private float throwVelocity;

    public void OnAngleReached() {
        aimScr.LockAimingAngle();
        aimScr.ThrowAtPower(throwVelocity); // Notice: "Power" == velocity
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (!this.isActiveAndEnabled) // Don't throw bones when disabled
            return;

        if (coll.gameObject.tag == "Ammo-Static" && aimScr.ammo == null) {
            aimScr.PickupAmmo(coll.gameObject.GetComponent<Ammmmo>());

            // Calculate angle to throw bone at
            float xdistance;
            xdistance = target.position.x - throwPoint.x;
            //Make the throwing less accurate
            xdistance = Random.Range(xdistance - accuracyDiviation, xdistance + accuracyDiviation);

            float ydistance;
            ydistance = target.position.y - throwPoint.y;
            float throwAngleRadians; // in radian
            throwAngleRadians = Mathf.Atan((ydistance + 4.905f * (Mathf.Pow(timeHit, 2))) / xdistance);
            throwAngle = throwAngleRadians * Mathf.Rad2Deg; // aimScr uses degrees, so convert angles
            throwVelocity = xdistance / (Mathf.Cos(throwAngleRadians) * timeHit);
//            print("ABS: Throwing at angle: " + throwAngle + " & power: " + throwVelocity);

            // Keep throwVelocity positive
            if (throwVelocity < 0) {
                throwAngle += 180;
                throwVelocity *= -1;
            }
            // Keep throwAngle between 0 & 360 degrees
            while (throwAngle < 0)
                throwAngle += 360;
            while (throwAngle >= 360)
                throwAngle -= 360;
//            print("Corrected: Throwing at angle: "+ throwAngle +" & power: "+ throwVelocity);
            Invoke("StartThrow", 1.2f);
        }
    }

    /*******************************************************/

    // Use this for initialization
    void Start() {
        aimScr = GetComponent<Aiming>();
        aimScr.onAngleReached.AddListener(OnAngleReached);
    }

    private void StartThrow() {
        aimScr.MoveToAimingAngle(throwAngle);
    }
}
