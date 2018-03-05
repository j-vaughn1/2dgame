using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    //variables used for spawning new bones     
    public float spawn_delay = 5f;      //delay between spawns
    private float last_spawn_time;
    public GameObject Bones;




    void Enable() {
        last_spawn_time = Time.time;               //set time
    }
	

	void Update () { // 1/60 second... each frame
        if (Time.time - last_spawn_time > spawn_delay)     //must be more than 5 seconds since last spawn, and there must be fewer than 4 bones on scene
        {
            Spawn();
            last_spawn_time = Time.time; //reset current time
        }
    }


    //*****************************

    void Spawn()                                                //Used to spawn more bones, since the bones explode
    {

        float yPos = .5f;                        //spawn the bone at a random location
        float xPos = Random.Range(-8f, 8f);
        var curPos = new Vector3(xPos, yPos);
        Instantiate(Bones, curPos, transform.rotation);


    }
}
