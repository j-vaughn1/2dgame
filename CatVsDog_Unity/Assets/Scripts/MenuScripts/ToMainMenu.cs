using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMainMenu : Menu {

	public void ToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"); //Goto Game scene. Note: Scene name is defined in File > Build settings
    }
}
