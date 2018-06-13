using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class DogManager : MonoBehaviour
{
    Animator anim;
    Movement dogMovement;
    bool facingRight;
    private bool isDead = false;

    public int maxDogHealth = 1000;
    private int currentDogHealth;
    public BarUpdater dogBar;

    private TurnManager turnManager;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        dogMovement = GetComponent<Movement>();
        currentDogHealth = maxDogHealth;
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void FixedUpdate() {
        switch (dogMovement.getState()) {
        case Movement.WalkDirection.Left:
            anim.SetInteger("D_State", 1);
            Flip(-1);
            break;
        case Movement.WalkDirection.Right:
            anim.SetInteger("D_State", 1);
            Flip(1);
            break;
        case Movement.WalkDirection.None:
            anim.SetInteger("D_State", 0);
            break;
        }
    }

    public void Update() {
        dogBar.suffixStr = " / " + maxDogHealth;
        dogBar.maxValue = maxDogHealth;
        dogBar.currentValue = currentDogHealth;
    }

    public void Flip(float hor) {
        if (hor > 0 && !facingRight || hor < 0 && facingRight) {
            facingRight = !facingRight;

            Vector3 scale = transform.localScale;

            scale.x *= -1;

            transform.localScale = scale;
        }

    }


    public void TakeDamage(int damage) {
        if (currentDogHealth > 0) {
            currentDogHealth -= damage;
            anim.SetInteger("D_State", 2); //play "Hurt" animation
        }
        if (currentDogHealth <= 0 && !isDead)
        {
            currentDogHealth = 0;
            isDead = true;
            anim.SetInteger("D_State", 3); //play dead animation
            turnManager.SetGameOver();
        }
    }
}
