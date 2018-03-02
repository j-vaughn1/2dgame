using System.Collections;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public GameObject boneAmmo;
    public GameObject explosionBone;



    void OnExplode()
    {
        int power = 5;
        int radius = 2;

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
        Destroy(gameObject);
            /*
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            Instantiate(explosionBone, transform.position, randomRotation); // Particle effect post explosion
            Destroy(boneAmmo, 1);
            Destroy(explosionBone, 2);
            */
        }

    

        void Collision2D(Collision col)
    {
            if (col.gameObject.tag == "Floor")
            {
                OnExplode();
            }

            if (col.gameObject.tag == "Wall")
            {
                OnExplode();
            }

        }
    
}

