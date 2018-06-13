/* Pause Menu that can be used during gameplay.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu {
    public AudioSource musicToMute;
    public AudioSource pauseSound;
    public float pausePitch = 1f;
    public float unpausePitch = 0.5f;

    public Menu instructionsMenu;
    public Menu configMenu;
    private TurnManager turnManager;

    public void OnInstructionsClicked() {
        OpenSubmenu(instructionsMenu);
    }
    public void OnConfigClicked() {
        OpenSubmenu(configMenu);
    }

    public void OnExitClicked() {
        turnManager.ForceTurnEnd(ExitGame);
    }

    /*******************************************************/

    override public void OpenMenu() {
        base.OpenMenu();
        Time.timeScale = 0;
        if (musicToMute.isPlaying)
            musicToMute.Pause();
        pauseSound.pitch = pausePitch;
        pauseSound.Play();
    }

    override public void CloseMenu() {
        base.CloseMenu();
        Time.timeScale = 1;
        if (!musicToMute.isPlaying)
            musicToMute.Play();
        pauseSound.pitch = unpausePitch;
        pauseSound.Play();
    }

    void ExitGame() {
        Time.timeScale = 1;
        SetMenuActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"); //Goto Game scene. Note: Scene name is defined in File > Build settings
    }

    /***************************************************/

    // Use this for initialization
    override protected void Start() {
        turnManager = FindObjectOfType<TurnManager>();
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Pause") && !menuObjToDisable.activeSelf) {
            OpenMenu();
        }
    }
}
