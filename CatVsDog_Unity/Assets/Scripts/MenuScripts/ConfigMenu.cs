using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMenu : Menu {
    public GameObject menuToClone;
    public string firstSelectedName = "Music IconSlider";
    public string[] cancelOptionNames = { "CancelButton" };
    public string musicSliderName = "Music IconSlider";
    public string sfxSliderName = "Sfx IconSlider";

    private GeneralConfig config;
    private VolumeSetter volumeUpdater;
    private float prevSfx = -1f;
    private float prevMusic = -1f;

    private Transform menuClone = null;
    private IconSlider sfxSlider = null;
    private IconSlider musicSlider = null;

    void OnMusicChanged(float newValue) {
        config.musicVolume = musicSlider.value;
        volumeUpdater.UpdateSoundsVolume();
    }
    void OnSfxChanged(float newValue) {
        config.sfxVolume = sfxSlider.value;
        volumeUpdater.UpdateSoundsVolume();
    }

    public void OnSave() {
        config.musicVolume = musicSlider.value;
        config.sfxVolume = sfxSlider.value;
        config.SaveToFile();
        volumeUpdater.UpdateSoundsVolume();

        prevMusic = config.musicVolume;
        prevSfx = config.sfxVolume;
        CloseMenu();
    }

    void UndoChanges() {
        if (prevMusic >= 0) { // Only apply sound changes if stored value for music
            config.musicVolume = prevMusic;
            config.sfxVolume = prevSfx;
        }
    }

    override public void OpenMenu() {
        this.menuObjToDisable = Instantiate(menuToClone, menuToClone.transform.parent);
        menuClone = this.menuObjToDisable.transform;
        prevMusic = config.musicVolume;
        prevSfx = config.sfxVolume;
        InputModuleListener inputModule = GetComponent<InputModuleListener>();

        // Change menu's references to point to cloned menu
        inputModule.firstSelectedGameObject = menuClone.Find(firstSelectedName).gameObject;
        inputModule.cancelOptions = System.Array.ConvertAll<string, GameObject>(cancelOptionNames,
            (optionName) => menuClone.Find(optionName).gameObject
        );
        musicSlider = menuClone.Find(musicSliderName).GetComponent<IconSlider>();
        sfxSlider = menuClone.Find(sfxSliderName).GetComponent<IconSlider>();

        base.OpenMenu();
        // Set cloned sliders to match the config's volume
        musicSlider.value = config.musicVolume;
        sfxSlider.value = config.sfxVolume;
        musicSlider.onValueChangeEvent.AddListener(OnMusicChanged);
        sfxSlider.onValueChangeEvent.AddListener(OnSfxChanged);
    }
    override public void CloseMenu() {
        base.CloseMenu();
        if (menuClone) {
            UndoChanges();
            Destroy(menuClone.gameObject);
            InputModuleListener inputModule = GetComponent<InputModuleListener>();
            // Remove all references to deleted objects
            inputModule.firstSelectedGameObject = null;
            inputModule.cancelOptions = System.Array.ConvertAll<string, GameObject>(cancelOptionNames,
                (optionName) => null
            );
            musicSlider = null;
            sfxSlider = null;
            menuClone = null;
        }
    }

    /*****************************************/

    // Use this for initialization
    override protected void Start () {
		config = GeneralConfig.GetInstance();
        volumeUpdater = FindObjectOfType<VolumeSetter>();
    }

    void OnDisable() {
        UndoChanges();
    }
    void OnDestroy() {
        UndoChanges();
    }
}
