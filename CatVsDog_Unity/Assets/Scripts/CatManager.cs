﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class CatManager : MonoBehaviour {
    Animator anim;
    Movement catMovement;
    bool facingRight;

    public int maxCatHealth = 1000;
    private int currentCatHealth;
    public BarUpdater catBar;
    private bool isDead = false;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        catMovement = GetComponent<Movement>();
        facingRight = true;
        currentCatHealth = maxCatHealth;
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

    public void Update()
    {
        catBar.maxValue = maxCatHealth;
        catBar.currentValue = currentCatHealth;

        if(currentCatHealth <= 0 && !isDead)
        {
            currentCatHealth = 0;
            isDead = true;
            anim.SetInteger("C_State", 3); //play dead animation
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

    public void TakeDamage(int damage)
    {
        if(currentCatHealth > 0)
        {
            currentCatHealth -= damage;
            anim.SetInteger("C_State", 2); //play "Hurt" animation
        }
    }
}