using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public GameObject explosion;             //uses prefab of explosion
    public float explosion_delay = 10f;
    public float current_size = 0f;
    public float explosion_max_size = 5f;
    public float explosion_rate = 1f;
    public float explosion_speed = 1f;
    bool exploded = false;
    CircleCollider2D explosion_size;

    private CatManager cat;
    private DogManager dog;
    private TurnManager turnManager;
    public GameObject throwingObj = null;

    private int damage = 100; //fix damage dealt when hit


    //**************************

    void Start() {
        explosion_size = gameObject.GetComponent<CircleCollider2D>();
        cat = FindObjectOfType<CatManager>();
        dog = FindObjectOfType<DogManager>();
        turnManager = FindObjectOfType<TurnManager>();
    }

    //**************************

    void Update()                                    //a timer for the explosion
    {
        explosion_delay -= Time.deltaTime;
        if (explosion_delay < 0 && !exploded) {
            exploded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        // Don't hurt yourself
        if (col.gameObject == throwingObj)
            return;
        if (col.gameObject.tag == "Cat" && !exploded) // If the coconut collides with a "Cat"
        {
            cat.TakeDamage(damage);
            exploded = true;
        }
        if (col.gameObject.tag == "Dog" && !exploded) // If the coconut collides with a "Dog"
        {
            dog.TakeDamage(damage);
            exploded = true;
        }
        if((col.gameObject.tag == "Wall" || col.gameObject.tag == "Floor") && !exploded)
        {
            exploded = true;
        }

    }

    //*****************************

    void FixedUpdate()                                     //physics of the explosion is updated here
    {
        if (exploded == true) {
            if (current_size < explosion_max_size)         //explosion increases in size until max size is reached
            {
                current_size += explosion_rate;
            } else {
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                Destroy(Instantiate(explosion, transform.position + new Vector3(0,0,-.1f), randomRotation), 1);
                Object.Destroy(this.gameObject);    //when max size is reached, object is destroyed
                turnManager.EndPlayersTurn();
            }
            explosion_size.radius = current_size;
        }
    }
}