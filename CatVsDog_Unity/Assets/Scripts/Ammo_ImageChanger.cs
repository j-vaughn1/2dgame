using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(  Ammo  ))]
[RequireComponent(typeof(  SpriteRenderer  ))]
public class Ammo_ImageChanger : MonoBehaviour {
    private Ammo ammoScr;
    private SpriteRenderer renderer;

    public Sprite catBoneImg;
    public Sprite dogBoneImg;

    public bool forceCatImg = false;
    public bool forceDogImg = false;

    // Use this for initialization
    void Start () {
        ammoScr = GetComponent<Ammo>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (forceCatImg) {
            renderer.sprite = catBoneImg;
            return;
        }
        if (forceDogImg) {
            renderer.sprite = dogBoneImg;
            return;
        }

        if (ammoScr.throwingObj) {
            if (ammoScr.throwingObj.GetComponent<CatManager>()) {
                renderer.sprite = catBoneImg;
            } else if (ammoScr.throwingObj.GetComponent<CatManager>()) {
                renderer.sprite = dogBoneImg;
            }
        }
    }
}
