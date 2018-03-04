using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    //variables used for spawning new bones
    public float max_bones = 3f;        //The max amount of bones in the game at a time
    public float curr_bones = 1f;       //Current number of bones in game, game starts with one          
    public float spawn_delay = 5f;      //delay between spawns
    public float curr_time = Time.deltaTime;
    public GameObject Bones;




    void Start () {
        if (((Time.deltaTime - curr_time) > spawn_delay) && curr_bones <= max_bones)     //must be more than 5 seconds since last spawn, and there must be fewer than 4 bones on scene
        {
            InvokeRepeating("Spawn", 1f, 3f);
            ++curr_bones;                //represent that a new bone is created
            curr_time = Time.deltaTime;  //reset current time
        }
    }
	

	void Update () {

      
    }


    //*****************************

    void Spawn()                                                //Used to spawn more bones, since the bones explode
    {

        float yPos = Random.Range(.5f, 5f);                        //spawn the bone at a random location
        float xPos = Random.Range(.5f, 10f);
        var curPos = new Vector3(xPos, yPos);
        Instantiate(Bones, curPos, transform.rotation);


    }
}
