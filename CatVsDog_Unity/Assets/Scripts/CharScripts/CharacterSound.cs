using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSound : MonoBehaviour {

    public AudioClip cry;

    public AudioSource crySource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Cry(){
        crySource.PlayOneShot(cry);
    }
}
