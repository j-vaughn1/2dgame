using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class CatManager : MonoBehaviour {
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

    public void FixedUpdate() {
        switch (catMovement.getState()) {
        case Movement.WalkDirection.Left:
            anim.SetInteger("C_State", 1);
            Flip(-1);
            break;
        case Movement.WalkDirection.Right:
            anim.SetInteger("C_State", 1);
            Flip(1);
            break;
        case Movement.WalkDirection.None:
            anim.SetInteger("C_State", 0);
            break;
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
