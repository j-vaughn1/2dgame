/*
 * Changes the AudioMixer's volumes to reflect the general config.
 * All sounds should use the AudioMixer, so their voumes are updated as well.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSetter : MonoBehaviour {
    public AudioMixer mixer; // This object 
    GeneralConfig config;

    private float muteAttenuation = -100; // If percent is at 0, it's completely muted
    // The following two are used for Lerp
    private float minAttenuation = -20;
    private float normAttenuation = 0;

    public void UpdateSoundsVolume() {
        mixer.SetFloat("MusicVolume", ConvertPercentToAttenuation(config.musicVolume));
        mixer.SetFloat("SfxVolume", ConvertPercentToAttenuation(config.sfxVolume));
        // Make warnings for every Audio not in a group...
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>()) {
            if (audio.outputAudioMixerGroup == null)
                Debug.LogWarning("The audio " + audio + " is not in a mixer. It's volume will never be changed.");
        }
    }

    /*********************************/

    float ConvertPercentToAttenuation(float perc) {
        if (perc <= 0.00001) // Have some leeway for floats
            return muteAttenuation;
        return Mathf.Lerp(minAttenuation, normAttenuation, perc);
    }

	// Use this for initialization
	void Start () {
        config = GeneralConfig.GetInstance();
        UpdateSoundsVolume();
    }
}
