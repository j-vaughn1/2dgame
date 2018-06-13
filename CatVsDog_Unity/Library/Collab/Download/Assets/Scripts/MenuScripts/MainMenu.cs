using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public Image fadeOutImg;
    public AudioSource bgMusic;

    public float fadeOutTime = 2;
    private float currentFadeTime;
    private float musicStartVolume = -1;

    public void Play()
    {
        print("Play!");
        currentFadeTime = Mathf.Max(currentFadeTime, 0); // Set currentFade = 0 if not started. Otherwise, don't do anything
    }

    public void Options() 
    {
        //Goto Option Scence
    }

    public void Quit()
    {
        Application.Quit();
    }

    /*******************************************************/

    void Start() {
        currentFadeTime = -1;
        fadeOutImg.gameObject.SetActive(false);
    }

    void Update() {
        if (currentFadeTime >= fadeOutTime) { // Fade-out finished
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame"); //Goto Game scene. Note: Scene name is defined in File > Build settings
        } else if (currentFadeTime >= 0) { // Start fading out
            if (musicStartVolume == -1)
                musicStartVolume = bgMusic.volume;

            fadeOutImg.gameObject.SetActive(true);
            fadeOutImg.color = new Color(fadeOutImg.color.r, fadeOutImg.color.g, fadeOutImg.color.b,
                Mathf.Clamp01(currentFadeTime / fadeOutTime));
            bgMusic.volume = Mathf.Clamp01(musicStartVolume * (1 - (currentFadeTime / fadeOutTime))); // Fade out music also
            currentFadeTime += Time.deltaTime;
        }
    }
}
