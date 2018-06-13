using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammmmo : MonoBehaviour {
    public GameObject throwingObj = null; // Is set by Aiming script when the Ammo is thrown
    protected TurnManager turnManager; // Child scripts might use this to add pause events

    public AudioSource effect1;//hits wall/floor or is dropped
    public AudioSource effect2;//hits character

    // Child classes should call this to damage the players.
    // I put the code in this base class because all Ammo will need to damage the player...
    // which means they all need to check the tag of the collider, and then apply damage to the correct object.
    // This normally means that every class needs these lines of code.
    //
    // By putting the code in the base class, all Ammo can use this code...
    // Without copying & pasting this functionality.
    //
    // In other words: This reduces the amount of redundant code in the project.
    // This makes it MUCH easier to change how the players are damaged in the future.
    protected void DamagePlayer(Collider2D coll, int damage) {
        coll.GetComponent<CharacterManager>().TakeDamage(damage);
        if (throwingObj.transform.position.x < coll.transform.position.x)
            coll.GetComponent<Movement>().Knockback(Movement.WalkDirection.Right);
        else
            coll.GetComponent<Movement>().Knockback(Movement.WalkDirection.Left);
    }

    // Note: Ammo is locked into place for player to throw via the Aiming script.

    /* 
     * When this is called, the Ammo is changed into a "Thrown" state.
     * To add functionality, scripts should override OnThrown()
     * 
     * If Ammo calls this method, it should manually call OnThrown() as well.
     * Note: Call OnThrown() after the Ammo's velocity is set...
     */
    protected void PrimeAmmo(GameObject throwingObj) {
        Collider2D ammoCollider = GetComponent<Collider2D>();
        Rigidbody2D ammoRigidbody = ammoCollider.attachedRigidbody;
        ammoCollider.isTrigger = true; // To stop ball from teleporting through player, make the Ammo a Trigger collider
        ammo.throwingObj = throwingObj;
    }

    //***********************************************
    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }


    //***********************************************
    public virtual void OnPickUp()  // (This is called by Aiming script when Ammo is picked up)
    {
        print("\tBASE___Picked up");
        //what should this do? it can't be set to dynamic until it is thrown. It could change color or highlight itself?
        // This isn't needed for an ammo, but it could add special effects when picked up (like the exaples you listed above)
    }


    //***********************************************
    public virtual void OnDrop()  //set ammo to static (Also called by Aiming script when Ammo is dropped)
    {
        print("\tBASE___Dropped");
        // If this method is overridden by child classes, they should call this base method after they are done
        transform.gameObject.tag = "Ammo-Static";
        
    }

    protected virtual void OnCollideSurface(Collider2D col)  //bone was thrown, but it missed the other character. (Called by this script, when trigger collision detected)
    {
        print("\tBASE___Hit Surface");
        // If this method is overridden by child classes, they should call this base method after they are done
        transform.gameObject.tag = "Ammo-Static";
        effect2.Play();
        Object.Destroy(this.gameObject);
    }

    //***********************************************
    protected virtual void OnCollideCharacter(Collider2D col) { //Cause damage and destroy the ammo object (Called by this script, when trigger collision detected)
        // If this method is overridden by child classes, they should call this base method after they are done
        print("\tBASE___Hit CHAR");
        effect2.Play();
        Object.Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == throwingObj) // Don't injure the character that threw this ammo
            return;
        if (gameObject.tag == "Ammo-Dynamic")    //make sure that the ammo is dynamic, if it isn't the object is just picked up
        {
            if (col.gameObject.tag == "Dog" || col.gameObject.tag == "Cat")
            {
                OnCollideCharacter(col);
            }
            else if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Floor")
            {
                OnCollideSurface(col);
            }

        }
        // Aiming script handles picking up the Ammo (and calling OnPickup()  )
    }


    //***********************************************
    public virtual void OnThrown() //when the bone is thrown, set it to dynamic (Called by Aiming script when thrown)
    {
        print("\tBASE___Thrown");
        // If this method is overridden by child classes, they should call this base method after they are done
        transform.gameObject.tag = "Ammo-Dynamic";
    }
}
