using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public GameObject boneAmmo;


    void Start () {
       
    }

    void OnExplode()
    {

        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        Instantiate(boneAmmo, transform.position, randomRotation); // Particle effect post explosion
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Dog")
        {
            //decrease health

            OnExplode();
        }
        else if(col.tag == "Cat")
        {

        }
        else if(col.tag == "floor")
        {
            OnExplode();
        }
        else { OnExplode(); }
    
    }

}
