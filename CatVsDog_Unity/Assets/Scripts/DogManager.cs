using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogManager : MonoBehaviour
{
    Animator anim;
    Movement dogMovement;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        dogMovement = GetComponent<Movement>();

        
    }

    // Update is called once per frame
    void Update()
    {
        // handling Cat moves left + animation
        if (Input.GetAxis("Horizontal") < 0)
        {
            dogMovement.WalkLeft();
            anim.SetInteger("State", 1);
        }
        // handling Cat moves right + animation
        else if (Input.GetAxis("Horizontal") > 0)
        {
            dogMovement.WalkRight();
            anim.SetInteger("State", 1);
        }
        // handling Cat jumps + animation
        else if (Input.GetButton("Jump"))
        {
            dogMovement.Jump();
            anim.SetInteger("State", 2);
        }
        else
        {
            anim.SetInteger("State", 0);
        }
    }
}
