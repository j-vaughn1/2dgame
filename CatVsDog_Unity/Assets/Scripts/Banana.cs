﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Ammmmo {
    public GameObject splat;
    public int damage = 150; //fix damage dealt when hit




    protected override void OnCollideSurface(Collider2D col) {  //bone was thrown, but it missed the other character. 
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        Destroy(Instantiate(splat, transform.position + new Vector3(0, 0, -.1f), randomRotation), 1);  //uses a splatter prefab

        base.OnCollideSurface(col); // Let the base class run their code (Destroy this object)
    }
    protected override void OnCollideCharacter(Collider2D col) {
        DamagePlayer(col, damage);

        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        Destroy(Instantiate(splat, transform.position + new Vector3(0, 0, -.1f), randomRotation), 1);
        Object.Destroy(this.gameObject);    //when max size is reached, object is destroyed

        base.OnCollideCharacter(col); // Let the base class run their code (Destroy this object)
    }

}
