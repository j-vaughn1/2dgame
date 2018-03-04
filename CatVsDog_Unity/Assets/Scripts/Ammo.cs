using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public GameObject explosion;             //uses prefab of explosion
    public float explosion_delay = 1f;
    public float current_size = 0f;
    public float explosion_max_size = 5f;
    public float explosion_rate = 1f;
    public float explosion_speed = 1f;
    bool exploded = false;
    CircleCollider2D explosion_size;



    //**************************

    void Start() {
        explosion_size = gameObject.GetComponent<CircleCollider2D>();


    }

    //**************************

    void Update()                        //a timer for the explosion
    {

        explosion_delay -= Time.deltaTime;
        if (explosion_delay < 0) {
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
                Instantiate(explosion, transform.position, randomRotation); // Particle effect post explosion
                Object.Destroy(this.transform.parent.gameObject);    //when max size is reached, object is destroyed

            }
            explosion_size.radius = current_size;
        }
    }


    //*****************************

    void onTriggerEnter2D(Collision col)                               //bone explosion physics
    {
        if (exploded == true) {

            if (col.gameObject.GetComponent<Rigidbody2D>() != null)     //explosion force
            {
                Vector2 target = col.gameObject.transform.position;
                Vector2 bomb = gameObject.transform.position;

                Vector2 direction = 5f * (target - bomb);
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

                rb.AddForce(new Vector2(direction.x, direction.y * 2f));  //split the force between x and y components so either can be adjusted

            }

        }


    }



}