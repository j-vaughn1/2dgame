using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneControl : MonoBehaviour
{
    public float bone_death = 15f;    //bone will destroy after this time
    public float bone_time;
    bool bone_kill = false;

    //*******************
    void enable()
    {
        bone_time = Time.time;
    }

    //***********************
    void Update()
    {
        if (Time.time - bone_time > bone_death)     //timer for destroying bones
        {
            bone_kill = true;
            bone_time = Time.time; //reset current time
        }
    }

    //************************

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Dog" || col.tag == "Cat")  //if the cat or dog walk into the bone, destroy it
        {
            bone_kill = true;
        }

    }

    //*************************
    void FixedUpdate()
    {
        if (bone_kill)
        {
            Object.Destroy(this.gameObject);
            bone_kill = false;
        }
    }

}