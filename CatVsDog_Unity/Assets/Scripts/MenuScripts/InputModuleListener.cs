/* This class listens to the EventSystem's input module.
 * When the user tries to do actions in a menu, this script runs some Action functions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // For UnityEvent
using UnityEngine.EventSystems; // For StandaloneInputModule

public class InputModuleListener : MonoBehaviour {
    public GameObject firstSelectedGameObject = null; // If null, SelectFirstGameObject will use EventSystem's first selected...
    public AudioSource menuNavigateSound = null;
    public AudioSource menuConfirmSound = null;
    public AudioSource menuCancelSound = null;

    public bool canCancel = false; // If true, will be able to play cancel sound
    public GameObject[] cancelOptions = { }; // List of options that should play the cancel sound

    private StandaloneInputModule inputModule;
    private GameObject prevSelection = null;

    [HideInInspector]
    public bool onLastUpdate = false; // Is set by Menu when input should be disabled. Input is disabled this way so it has 1 last frame to play sounds before being disabled.

    [HideInInspector]
    public UnityEvent onMovedFromNullEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent onCancelEvent = new UnityEvent();

    // Intended for use with onMovedFromNullEvent
    public void SelectFirstGameObject() {
        EventSystem evSys = EventSystem.current;
        if (firstSelectedGameObject)
            evSys.SetSelectedGameObject(firstSelectedGameObject);
        else // Default: Use Event system's firstSelected...
            evSys.SetSelectedGameObject(evSys.firstSelectedGameObject);
    }
    // Wrapper for EventSystem.SetSelectedGameObject()
    public void SelectGameObject(GameObject gameObj) {
        EventSystem.current.SetSelectedGameObject(gameObj);
    }

    public bool IsCancelOption(GameObject obj) {
        foreach (GameObject option in cancelOptions) {
            if (option == obj)
                return true;
        }
        return false;
    }

    /**********************************/

    // Function runs when object is spawned, even when script is disabled (but not if GameObject is disabled!!!)
    void Awake() {
        inputModule = FindObjectOfType<StandaloneInputModule>();
    }

    void OnEnable() {
        prevSelection = EventSystem.current.currentSelectedGameObject;
    }

    // Update is called once per frame
    void Update() {
        // If option isn't selected, but want to select one... Invoke onMovedFromNullEvent
        EventSystem evSys = EventSystem.current;
        if (evSys.currentSelectedGameObject == null) {
            if (Input.GetButtonDown(inputModule.horizontalAxis) || Input.GetButtonDown(inputModule.verticalAxis))
                onMovedFromNullEvent.Invoke();
        }

        // Play sound when selection confirmed
        if (prevSelection != null && Input.GetButtonDown(inputModule.submitButton)) {
            // Cancel if a "cancel" option
            if (IsCancelOption(prevSelection)) {
                if (menuCancelSound != null)
                    menuCancelSound.Play();
            // Otherwise, play confirmation sound
            } else if (menuConfirmSound != null) {
                menuConfirmSound.Play();
            }
        }
        // When cancel is pressed, invoke action
        else if (canCancel && Input.GetButtonDown(inputModule.cancelButton)) {
            onCancelEvent.Invoke();
            // Play sound when selection cancelled
            if (menuCancelSound != null)
                menuCancelSound.Play();
        }
        // Play sound when selection changes
        else if (menuNavigateSound != null && prevSelection != evSys.currentSelectedGameObject)
            menuNavigateSound.Play();
        prevSelection = evSys.currentSelectedGameObject;

        if (onLastUpdate) {
            onLastUpdate = false;
            this.enabled = false;
        }
    }
}
