using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class CatManager : MonoBehaviour {
    Animator anim;
    Movement catMovement;
    bool facingRight;
    public enum state { Idle, Walk, Pickup, Throw, JumpUp, JumpDown, Hurt, Dead};

    public int maxCatHealth = 1000;
    private int currentCatHealth;
    public BarUpdater catBar;
    private bool isDead = false;

    private TurnManager turnManager;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        catMovement = GetComponent<Movement>();
        facingRight = true;
        currentCatHealth = maxCatHealth;
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void FixedUpdate() {
//        Debug.Log("State: " + anim.parameters[0]);
        switch (catMovement.getState()) {
        case Movement.WalkDirection.Left:
            anim.SetInteger("State", (int)state.Walk);
            Flip(-1);
            break;
        case Movement.WalkDirection.Right:
            anim.SetInteger("State", (int)state.Walk);
            Flip(1);
            break;
        case Movement.WalkDirection.None:
            anim.SetInteger("State", (int)state.Idle);
            break;
        }
    }

    public void Update()
    {
        catBar.suffixStr = " / " + maxCatHealth;
        catBar.maxValue = maxCatHealth;
        catBar.currentValue = currentCatHealth;

        if(currentCatHealth <= 0 && !isDead)
        {
            currentCatHealth = 0;
            isDead = true;
            anim.SetInteger("State", (int)state.Dead); //play dead animation
            turnManager.SetGameOver();
        }
    }

    public void Flip(float hor)
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
            anim.SetInteger("State", (int)state.Hurt); //play "Hurt" animation
        }
    }
}
