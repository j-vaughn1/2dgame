using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public GameObject boneAmmo;
    public GameObject explosionBone;



    void OnExplode()
    {
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        Instantiate(explosionBone, transform.position, randomRotation); // Particle effect post explosion
        Destroy(boneAmmo, 1);
        Destroy(explosionBone, 2);
    }

    void OnCollisionEnter2D(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            OnExplode();
        }

        if(col.gameObject.tag == "Wall")
        {
            OnExplode();
        }
    
    }

}
