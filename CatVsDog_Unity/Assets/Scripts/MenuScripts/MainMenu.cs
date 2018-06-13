using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu {
    public Image fadeOutImg;
    public AudioSource bgMusic;
    public Menu creditsMenu;
    public Menu instructionsMenu;
    public Menu configMenu;

    public float fadeOutTime = 2;
    private float currentFadeTime;
    private float musicStartVolume = -1;

    public void Play() {
        print("Play!");
        CloseMenu();
        fadeOutImg.gameObject.SetActive(true);
        currentFadeTime = Mathf.Max(currentFadeTime, 0); // Set currentFade = 0 if not started. Otherwise, don't do anything
    }

    public void Instructions() {
        OpenSubmenu(instructionsMenu);
    }
    public void Credits() {
        OpenSubmenu(creditsMenu);
    }
    public void Options() {
        OpenSubmenu(configMenu);
    }

    public void Quit() {
        CloseMenu();
        Application.Quit();
    }

    /*******************************************************/

    override protected void Awake() {
        base.Awake();
        openAtInit = true;
        currentFadeTime = -1;
        fadeOutImg.gameObject.SetActive(false);
    }

    void Update() {
        if (currentFadeTime >= fadeOutTime) { // Fade-out finished
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame"); //Goto Game scene. Note: Scene name is defined in File > Build settings
        } else if (currentFadeTime >= 0) { // Start fading out
            if (musicStartVolume == -1)
                musicStartVolume = bgMusic.volume;

            fadeOutImg.color = new Color(fadeOutImg.color.r, fadeOutImg.color.g, fadeOutImg.color.b,
                Mathf.Clamp01(currentFadeTime / fadeOutTime));
            bgMusic.volume = Mathf.Clamp01(musicStartVolume * (1 - (currentFadeTime / fadeOutTime))); // Fade out music also
            currentFadeTime += Time.deltaTime;
        }
    }
}
