using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneControl : MonoBehaviour
{


    //************************

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Dog" || col.tag == "Cat")  //if the cat or dog walk into the bone, destroy it
        {
            Destroy(GameObject.FindGameObjectWithTag("Bone"));

        }

    }
}