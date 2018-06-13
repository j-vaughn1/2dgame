using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBG_ClickSpawn : MonoBehaviour {
    public GameObject primaryAmmo;
    public GameObject secondaryAmmo;

    Vector3 GetMouseWorldPosition() {
        Camera mainCam = Camera.main;
        Vector2 mousePos = Input.mousePosition;
        Vector3 camPos = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCam.nearClipPlane));
        return new Vector3(camPos.x, camPos.y, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            Instantiate(primaryAmmo, GetMouseWorldPosition(), Quaternion.identity);
        else if (Input.GetMouseButtonDown(1))
            Instantiate(secondaryAmmo, GetMouseWorldPosition(), Quaternion.identity);
    }
}
