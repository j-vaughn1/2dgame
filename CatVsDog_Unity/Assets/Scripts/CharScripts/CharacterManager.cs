using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class CharacterManager : MonoBehaviour {
    Animator anim;
    Movement charMovement;
    bool facingRight;
    public enum state { Idle, Walk, Pickup, Throw, JumpUp, JumpDown, Hurt, Dead};

    public int maxHealth = 1000;
    private int currentHealth;
    public BarUpdater charBar;
    private bool isDead = false;

    private TurnManager turnManager;

    // Use this for initialization
    void OnEnable ()
    {
        anim = GetComponent<Animator>();
        charMovement = GetComponent<Movement>();
        facingRight = transform.localScale.x > 0;
        currentHealth = maxHealth;
        turnManager = FindObjectOfType<TurnManager>();

        Aiming aimScr = GetComponent<Aiming>();
        aimScr.ammoPickupAction.onActionStart.AddListener(
            (ammo) =>
            {
                print("Picked up ammo");
                anim.SetInteger("State", (int)state.Pickup);
                aimScr.ammoPickupAction.FinishAction();
            });

        aimScr.ammoThrowAction.onActionStart.AddListener(
          (ammo) =>
          {
              print("Throw ammo");
              anim.SetInteger("State", (int)state.Throw);
              aimScr.ammoThrowAction.FinishAction();
          });

        aimScr.ammoDropAction.onActionStart.AddListener(
          (ammo) => {
              print("Drop ammo");
              anim.SetInteger("State", (int)state.Idle);
              aimScr.ammoDropAction.FinishAction();
          });
    }

    public void FixedUpdate() {
//        Debug.Log("State: " + anim.parameters[0]);
        switch (charMovement.getState()) {
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
        charBar.suffixStr = " / " + maxHealth;
        charBar.maxValue = maxHealth;
        charBar.currentValue = currentHealth;

        if(currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            isDead = true;
            anim.SetInteger("State", (int)state.Dead); //play dead animation
            turnManager.SetGameOver();
        }
    }

    public void Flip(float hor)
    {
        Vector3 scale = transform.localScale;
        if (hor > 0 && !facingRight)
        {
            facingRight = true;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        } else if (hor < 0 && facingRight)
        {
            facingRight = false;
            scale.x = Mathf.Abs(scale.x) * -1;
            transform.localScale = scale;
        }
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damage;
            anim.SetInteger("State", (int)state.Hurt); //play "Hurt" animation
        }
    }
}
