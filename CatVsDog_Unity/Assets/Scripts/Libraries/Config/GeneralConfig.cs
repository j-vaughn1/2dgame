/* General Options apply to the entire game... So this is a singleton.
 * Holds general options for the game.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralConfig : Configuration {
    // Public so it can be printed form other classes. (DEBUG)
    public static readonly string MISC_CONFIG_PATH = CONFIG_FOLDER + "general.json";
    private static GeneralConfig configInst = LoadConfig<GeneralConfig>(MISC_CONFIG_PATH);
    private static bool firstInstMade = false; // Used to ensure only one GeneralConfig is made (even before configInst is set)

    // Variables that are saved in the config...
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public static GeneralConfig GetInstance() {
        return configInst;
    }

    /****************************************/

    override protected string GetFilepath() {
        return MISC_CONFIG_PATH; // Only have one config of this type...
    }

    public GeneralConfig() {
        // The first instance will be set as the Singleton...
        Debug.Assert(!firstInstMade, "Cannot create multiple instances of General Config!");
        firstInstMade = true;
    }
}
