using UnityEngine;
using UnityEngine.UI; // For Slider

public class DBG_ConfigTest : MonoBehaviour {
    public IconSlider musicSlider;
    public IconSlider sfxSlider;

    private GeneralConfig config;

    public void OnSave() {
        print("Saving config to: "+ GeneralConfig.MISC_CONFIG_PATH);
        config.musicVolume = musicSlider.value;
        config.sfxVolume = sfxSlider.value;
        config.SaveToFile();
    }

	// Use this for initialization
	void Start () {
        config = GeneralConfig.GetInstance();
        musicSlider.value = config.musicVolume;
        sfxSlider.value = config.sfxVolume;
    }
}
