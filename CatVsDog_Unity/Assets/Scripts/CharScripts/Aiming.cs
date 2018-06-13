/*
 * Both characters call this code to move the RigidBody around.
 * TurnManager will "disable" this script to prevent players / AIs
 *     from throwing objects on the other player's turn.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Aiming : MonoBehaviour {
    // min & max power must be >= 0!
    public float minPower = 5; // This is velocity... so <units> per second
    public float maxPower = 10;
    public float powerChangeRate = 5; // <Power units> per second

    public float minAngle = 0;
    public float maxAngle = 180;
    public float angleChangeRate = 90; // Degrees per second

    // When ammo is set, this starts the entire throw process
    public Transform ammoGrabRelTo = null; // GameObject to find ammoGrabPos relative to...
    public Vector3 ammoGrabPos; // Where ammo should be place, relative to the character
    public Ammmmo ammo {
        // Other scripts cannot set ammo directly. They need to try picking up, dropping, or throwing the Ammo...
        get { return _ammo; }
    } // Set this to start the throwing process
    private Ammmmo _ammo = null;

    public float currPower { get { return _currPower; } }
    public float currAngle { get { return _currAngle; } }
    public bool isAngleLocked { get { return _isAngleLocked; } }
    private float _currPower;
    private bool _isPowerIncreasing = true;
    private float _targetPower = -1; // If less than zero, target isn't set
    private float _currAngle = 90;
    private float _targetAngle;
    private bool _isAngleLocked;

    // To flip the sprite when changing aim direction
    Movement moveScr;

    // Aiming UI
    public GameObject aimingUIPrefab; // Disabled until ammo is picked up
    private GameObject aimingUI; // Disabled until ammo is picked up
    private Transform angleUI;
    private BarUpdater powerBar;
    private GameObject powerBarObject; // Disabled until aiming angle is locked

    private TurnManager turnManager = null; // Will end turn once bone is thrown

    // Events to invoke
    [HideInInspector]
    public UnityEvent onAngleReached;
    // These events are for the character animations.
    public AnimationAction<Ammmmo> ammoPickupAction {
        get {
            if (aimingUI == null) // If haven't started, Initialize() object early
                Initialize();
            return _ammoPickupAction;
        }
    }
    private AnimationAction<Ammmmo> _ammoPickupAction;
    public AnimationAction<Ammmmo> ammoThrowAction {
        get {
            if (aimingUI == null) // If haven't started, Initialize() object early
                Initialize();
            return _ammoThrowAction;
        }
    }
    private AnimationAction<Ammmmo> _ammoThrowAction;
    public AnimationAction<Ammmmo> ammoDropAction {
        get {
            if (aimingUI == null) // If haven't started, Initialize() object early
                Initialize();
            return _ammoDropAction;
        }
    }
    private AnimationAction<Ammmmo> _ammoDropAction;

    /*********************************************/

    public void PickupAmmo(Ammmmo ammo) {
        // Should not set new ammo while holding a different one
        Debug.Assert(_ammo == null, "Character animations do not support instantly picking up another Ammo. Should have dropped previous Ammo first.", this);

        if (this.isActiveAndEnabled && !ammoPickupAction.IsActionRunning() // Can pickup ammo...
            && _ammo == null // Not already holding ammo
            && ammo != null) { // and target ammo exists...
            ammoPickupAction.StartAction(ammo);
            if (ammoDropAction.IsActionRunning())
                Debug.LogWarning("Somehow trying to pickup Ammo while dropping another one!");
        }
    }
    public void DropAmmo() {
        if (_ammo != null && !ammoDropAction.IsActionRunning() && !ammoThrowAction.IsActionRunning()) {
            ammoDropAction.StartAction(ammo);
            if (ammoPickupAction.IsActionRunning())
                Debug.LogWarning("Somehow trying to drop Ammo while picking another one up!");
        }
    }

    /* Both characters call this to set the angle to throw at.
     * When done, invokes the onAngleReached event.
     */
    public void MoveToAimingAngle(float newTargetAngle) {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (_isAngleLocked || ammo == null)
            return;
        newTargetAngle = Mathf.Clamp(newTargetAngle, minAngle, maxAngle);

        _targetAngle = newTargetAngle;
        if (_currAngle == newTargetAngle) // If already at this angle, just say it's done
            onAngleReached.Invoke();
    }

    /* Is called by both characters to save the current angle.
     * This also starts the power bar.
     */
    public void LockAimingAngle() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (_isAngleLocked || ammo == null)
            return;
        _isAngleLocked = true;
        powerBarObject.SetActive(true);
        powerBar.maxValue = maxPower - minPower;
        powerBar.currentValue = 0;
    }

    /* Both characters call this after the power bar is shown.
     * It throws the ammo at currPower.
     */
    public void Throw() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (!_isAngleLocked)
            return;
        ammoThrowAction.StartAction(ammo);
    }

    /* Intended for the AI.
     * Called right after SetAimingAngle()...
     * This waits for power bar to reach the given angle.
     *      Once angle is reached, Throw() is automatically called.
     */
    public void ThrowAtPower(float newTargetPower) {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (!_isAngleLocked)
            return;
        // DEBUG: AI doesn't actually throw using these power limits... so change min & max power to fit the AI throwing...
        _targetPower = newTargetPower; // Also, don't clamp power
        float prevPowerChangePerc = (maxPower - minPower) / powerChangeRate; // % change per second
        minPower = Mathf.Clamp(_targetPower - 5, 0, 5);
        maxPower = _targetPower + 5;
        powerChangeRate = (maxPower - minPower) * prevPowerChangePerc; // So power bar changes at same speed
        //        _targetPower = Mathf.Clamp(newTargetPower, minPower, maxPower);
    }

    /*********************************************/

    /* Once the pickup animation is ready, have character actually hold the ammo...
     */
    private void OnPickupActionDone(Ammmmo ammo) {
        ResetThrow();
        // Pickup the new ammo
        _ammo = ammo;
        // Lock ammo in place
        Collider2D ammoCollider = ammo.GetComponent<Collider2D>();
        Rigidbody2D ammoRigidbody = ammoCollider.attachedRigidbody;
        ammoCollider.enabled = false;
        ammoRigidbody.simulated = false;
        this.UpdateAmmoPosition();
        ammo.OnPickUp();

        // Show Aiming UI
        aimingUI.SetActive(true);
    }

    /* Runs for both drop & throw actions.
     */
    private void ReleaseAmmo(Ammmmo ammo) {
        // Release lock and let ammo drop
        Collider2D ammoCollider = ammo.GetComponent<Collider2D>();
        Rigidbody2D ammoRigidbody = ammoCollider.attachedRigidbody;
        ammoCollider.enabled = true;
        ammoRigidbody.simulated = true;
        _ammo = null;
    }
    /* When throw animation is ready, let go of the ammo.
     */
    private void OnThrowActionDone(Ammmmo ammo) {
        ammo.PrimeAmmo(this.gameObject); // Convert ammo to "Throwing" state
        // Throw the ammo
        Rigidbody2D ammoRigidbody = ammo.GetComponent<Rigidbody2D>();
        ammoRigidbody.velocity = new Vector2(Mathf.Cos(_currAngle * Mathf.Deg2Rad), Mathf.Sin(_currAngle * Mathf.Deg2Rad)) * _currPower;
        ReleaseAmmo(ammo);
        ammo.OnThrown(); // Tell ammo it has been thrown
        if (turnManager != null && turnManager.gameObject.activeSelf && turnManager.enabled)
            turnManager.EndPlayersTurn();
//        this.enabled = false; // Disable throwing
        ResetThrow();
    }
    /* When drop animation is ready, let go of the ammo.
     */
    private void OnDropActionDone(Ammmmo ammo) {
        print("Dropped");
        ReleaseAmmo(ammo);
        ammo.OnDrop();
        ResetThrow();
    }
    
    /* Unsets all throwing variables for next throw... */
    private void ResetThrow() {
        if (aimingUI == null) // If haven't started, Initialize() object early
            Initialize();
        _isAngleLocked = false;
        _targetAngle = _currAngle;
        _targetPower = -1; // Unset target power

        _currPower = minPower;
        _isPowerIncreasing = true;

        // Disable UI
        powerBar.maxValue = maxPower - minPower;
        powerBar.currentValue = 0;
        powerBarObject.SetActive(false);
        aimingUI.SetActive(false);
    }

    private void UpdateAmmoPosition() {
        if (ammoGrabRelTo == null)
            ammoGrabRelTo = this.transform;
        ammo.transform.position = ammoGrabRelTo.TransformPoint(ammoGrabPos);
    }

    /*********************************************/

    /* Called once when object is created. */
    void Initialize() {
        if (aimingUI == null) {
            aimingUI = Instantiate(aimingUIPrefab);
            aimingUI.name = name +"_"+ aimingUIPrefab.name;
            angleUI = aimingUI.transform.Find("AngleMarker");
            powerBarObject = aimingUI.transform.Find("PowerBar").gameObject;
            powerBar = powerBarObject.transform.Find("Power").GetComponent<BarUpdater>();
            aimingUI.SetActive(false);
        }

        turnManager = FindObjectOfType<TurnManager>();
        moveScr = GetComponent<Movement>();
        _ammoPickupAction = new AnimationAction<Ammmmo>(OnPickupActionDone);
        _ammoThrowAction = new AnimationAction<Ammmmo>(OnThrowActionDone);
        _ammoDropAction = new AnimationAction<Ammmmo>(OnDropActionDone);
    }

    void Start() {
        if (aimingUI == null) // If haven't started, Initialize() object
            Initialize();
    }

    /* TurnManager enables this script at the start of the player's turn. */
    void OnEnable() {
        ResetThrow();
    }

    /* TurnManager disables this script at the end of the player's turn. */
    void OnDisable() {
        DropAmmo(); // If have a bone, drop it
    }
	
	/* Update is called once per frame. */
	void Update() {
        if (Time.timeScale <= 0) // If game paused, stop
            return;
        if (ammo == null) // If haven't picked up an object, stop
            return;
        // Change throwing angle
        if (!_isAngleLocked) {
            if (_targetAngle != _currAngle) { // Move angle closer to target
                _currAngle = Mathf.MoveTowards(_currAngle, _targetAngle, angleChangeRate * Time.deltaTime);
                if (_currAngle == _targetAngle) // Just reached angle, so invoke event
                    onAngleReached.Invoke();
            }

            if (moveScr != null) {
                if (_currAngle >= 90)
                    moveScr.FaceLeft();
                else
                    moveScr.FaceRight();
            }
        }
        // Show current throwing angle
        angleUI.eulerAngles = new Vector3(0, 0, _currAngle);
        aimingUI.transform.position = transform.position;
        UpdateAmmoPosition(); // Move ammo so it's still on the character

        // Change power
        if (_isAngleLocked) {
            if (_isPowerIncreasing) {
                _currPower += powerChangeRate * Time.deltaTime;
                if (_currPower >= maxPower) {
                    _currPower = maxPower;
                    _isPowerIncreasing = false;
                }
            } else {
                // If _targetPower set, throw at that power...
                if (_targetPower >= 0 && _currPower <= _targetPower) {
                    Throw();
                    return;
                }
                // Otherwise, decrease the power
                _currPower -= powerChangeRate * Time.deltaTime;
                if (_currPower <= minPower) {
                    _currPower = minPower;
                    _isPowerIncreasing = true;
                }
            }

            // Show power bar
            powerBar.maxValue = maxPower - minPower;
            powerBar.currentValue = _currPower - minPower;
        }
	}
}
