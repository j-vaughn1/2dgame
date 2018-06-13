/* Read / Write JSON files for the configuration.
 * The serialized variables are saved into the JSON file.
 * 
 * It allows for multiple configurations of the same type.
 * Use case: Different players binding the same controls.
 * 
 *----------------------------
 * 
 * Reference: https://unity3d.com/learn/tutorials/topics/scripting/loading-game-data-json
 * 
 * System.IO.File: https://answers.unity.com/questions/381902/systemio-documentation.html
 * Creating folder: https://stackoverflow.com/a/9065716
 * readonly: http://www.arungudelli.com/tutorial/c-sharp/10-differences-between-constant-vs-readonly-static-readonly-fields/
 * Generic function in non-generic class: https://stackoverflow.com/a/423620
 * Constraints on Generics: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new-constraint
 */

using UnityEngine; // For JsonUtility
using System.IO; // For File class

public abstract class Configuration {
    protected static readonly string CONFIG_FOLDER = Application.persistentDataPath + "/config/";

    /* Implementations need to say where they're saving the configuration.
     * This is a member variable because can have multiple Configurations of the same type...
     * like keybindings (for different players / devices?).
     */
    abstract protected string GetFilepath();

    /**********************************/

    /* Load a configration of the given subclass from filepath.
     * If none given, returns a default configuration of that type.
     */
    protected static ConfigType LoadConfig<ConfigType>(string filepath) where ConfigType : Configuration, new() {
        ConfigType result = new ConfigType();

        if (File.Exists(result.GetFilepath())) { // Read config from a file
            string jsonString = File.ReadAllText(result.GetFilepath());
            JsonUtility.FromJsonOverwrite(jsonString, result); // Also use default values for config options not specified in JSON
        }
        return result;
    }

    /* Save the configuration to a file.
     */
	virtual public void SaveToFile() {
        string jsonString = JsonUtility.ToJson(this);
        Directory.CreateDirectory(CONFIG_FOLDER); // If folder doesn't exist, create it
        File.WriteAllText(this.GetFilepath(), jsonString);
    }
}
