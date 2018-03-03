using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBone : MonoBehaviour {

    private CatManager cat;
    private int damage = 100; //fix damage dealt when hit

    // Use this for initialization
    void Start () {
        cat = FindObjectOfType<CatManager>();
        Destroy(gameObject, 10); //Destroy dogbone object after 10 sec (when bone not hit the cat)
        
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Cat") // If the coconut collides with a "Bird" 
        {
            cat.TakeDamage(damage);

            Destroy(gameObject);
        }
    }



}
