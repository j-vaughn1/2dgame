using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneLauncher : MonoBehaviour {
    private bool foundBone;
    private PlayerMover playerScr;
    public Transform aimingReticle;
    public GameObject bone;

    public float minReticleDistance = 3;
    public float maxReticleDistance = 6;

    public float minSpeed = 5;
    public float maxSpeed = 10;
    public float maxPowerTime = 3;
    public float minAngle = 0;
    public float maxAngle = 90;
    public float angleChangeSpeed = 90; // Degrees per second

    public float currPowerTime { get { return _currPowerTime; } }
    public float currAngle { get { return _currAngle; } }
    private float _currPowerTime;
    private float _currAngle;

    private CatManager cat;
    private DogManager dog;

    // Use this for initialization
    void OnEnable() {
        foundBone = false;
        _currPowerTime = 0;
        _currAngle = 90;
        playerScr = GetComponent<PlayerMover>();

        // So script works for either cat or dog...
        cat = GetComponent<CatManager>();
        dog = GetComponent<DogManager>();
    }
    void OnDisable() {
        aimingReticle.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (!this.enabled) // Don't throw bones when disabled
            return;
        
        if (coll.gameObject.tag == "Bone-static" && !foundBone) {
            Destroy(coll.gameObject);
            foundBone = true;
            playerScr.enabled = false;
            aimingReticle.localPosition = Vector3.zero;
            aimingReticle.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
		if (foundBone) {
            // Set throwing angle with left & right
            _currAngle -= Input.GetAxis("Horizontal") * angleChangeSpeed * Time.deltaTime;
            // Also use up & down
            if (Input.GetAxis("Vertical") > 0.0001) { // Aim higher
                if (_currAngle > 90) // Decrease angle
                    _currAngle = Mathf.Max(_currAngle - Input.GetAxis("Vertical") * angleChangeSpeed * Time.deltaTime, 90.001f);
                else // Increase angle
                    _currAngle = Mathf.Min(_currAngle + Input.GetAxis("Vertical") * angleChangeSpeed * Time.deltaTime, 89.999f);
            } else if (Input.GetAxis("Vertical") < -0.0001) { // Aim lower
                if (_currAngle > 90) // Keep increasing angle
                    _currAngle -= Input.GetAxis("Vertical") * angleChangeSpeed * Time.deltaTime;
                else // Decrease angle
                    _currAngle += Input.GetAxis("Vertical") * angleChangeSpeed * Time.deltaTime;
            }
            _currAngle = Mathf.Clamp(_currAngle, minAngle, maxAngle);

            Vector2 currAimVector = new Vector2(Mathf.Cos(_currAngle * Mathf.Deg2Rad), Mathf.Sin(_currAngle * Mathf.Deg2Rad));

            if (_currAngle <= 90) {
                Flip(1); // Face right
            } else {
                Flip(-1); // Face left
            }

            if (Input.GetButton("Jump")) { // Holding down throw button
                _currPowerTime = Mathf.Min(_currPowerTime + Time.deltaTime, maxPowerTime);

            } else if (Input.GetButtonUp("Jump")) { // Just released throw button
                float throwSpeed = (maxSpeed - minSpeed) * _currPowerTime / maxPowerTime + minSpeed;

                // Spawn bone
                GameObject boneInstance = Instantiate(bone, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
                boneInstance.GetComponent<Ammo>().throwingObj = this.gameObject;

                // Throw the bone
                Rigidbody2D rigid;
                rigid = boneInstance.GetComponent<Rigidbody2D>();
                rigid.velocity = currAimVector * throwSpeed;

                // Stop this script
                aimingReticle.gameObject.SetActive(false);
                this.enabled = false;
                foundBone = false;
            }

            // Update aiming reticle
            float reticleDist = (maxReticleDistance - minReticleDistance) * _currPowerTime / maxPowerTime + minReticleDistance;
            aimingReticle.position = currAimVector * reticleDist;
            aimingReticle.position += transform.position;
        }
	}

    /**************************/

    void Flip(float horz) {
        if (cat)
            cat.Flip(horz);
        if (dog)
            dog.Flip(horz);
    }
}
