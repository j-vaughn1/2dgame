using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    Animator anim;
    Movement catMovement;
    bool facingRight;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        catMovement = GetComponent<Movement>();
        facingRight = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // handling Cat moves left + animation
        if (Input.GetAxis("Horizontal") < 0)
        {
            catMovement.WalkLeft();
            anim.SetInteger("C_State", 1);
            Flip(Input.GetAxis("Horizontal"));

        }
        // handling Cat moves right + animation
        else if (Input.GetAxis("Horizontal") > 0)
        {
            catMovement.WalkRight();
            anim.SetInteger("C_State", 1);
            Flip(Input.GetAxis("Horizontal"));
        }
        // handling Cat in Idle mode
        else
        {
            anim.SetInteger("C_State", 0);
        }
    }

    private void Flip(float hor)
    {
        if(hor > 0 && !facingRight || hor < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 scale = transform.localScale;

            scale.x *= -1;

            transform.localScale = scale;
        }

    }
}
