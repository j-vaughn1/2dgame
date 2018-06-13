/* Represents a value between 0 - 1 via a few icons.
 * The more icons listed, the more precise the measurements.
 * 
 * When the user selects this object, they can use the horizontal axis
 * to increase / decrease the value.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems; // For StandaloneInputModule

public class IconSlider : MonoBehaviour {
    public float value {
        get { return this._value; }
        set {
            this.SetValue((int) Mathf.Ceil(value * iconButtons.Length));
        }
    }
    public AudioSource clickSound = null;
    public GameObject zeroImgObject = null;
    public Button zeroButton;
    public Button[] iconButtons;

    private int numIconsSelected = -1;
    private float _value = 1;
    private StandaloneInputModule inputModule;

    const float INPUT_DELAY = 0.2f;

    private float timeSinceLastInput = INPUT_DELAY +1;
    private float lastInput;

    public class ValueChangeEvent : UnityEvent<float> { }
    [HideInInspector]
    public ValueChangeEvent onValueChangeEvent = new ValueChangeEvent();

    /****************************************/

    void KeepSelection() {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    void OnClick(Button iconButton) {
        int index = System.Array.IndexOf(iconButtons, iconButton);
        SetValue(index +1);
        clickSound.Play();
        KeepSelection();
    }

    public void SetValue(int newNumIconsSelected) {
        newNumIconsSelected = Mathf.Min(Mathf.Max(newNumIconsSelected, 0), iconButtons.Length);
        this._value = ((float) newNumIconsSelected) / iconButtons.Length;
        this.numIconsSelected = newNumIconsSelected;
        foreach (Button icon in iconButtons) {
            if (newNumIconsSelected > 0) {
                icon.GetComponent<Animator>().SetBool("Show", true); // "Increasing" animation
                --newNumIconsSelected;
            } else {
                icon.GetComponent<Animator>().SetBool("Show", false); // "Decreasing" animation
            }
        }

        if (zeroImgObject != null)
            zeroImgObject.SetActive(numIconsSelected == 0);
        onValueChangeEvent.Invoke(this._value);
    }

    // To bind to another button
    void ToggleZero() {
        KeepSelection();
        if (numIconsSelected == 0)
            SetValue(iconButtons.Length);
        else
            SetValue(0);
        clickSound.Play();
    }

    // Keep focus on this slider, even if click child button
    // Reference: https://answers.unity.com/questions/1226851/addlistener-to-onpointerdown-of-button-instead-of.html
    void BindButtonSelectEvent(Button bttn) {
        EventTrigger evTrigger = bttn.gameObject.AddComponent<EventTrigger>();
        // Add event for mouse down
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((evData) => KeepSelection());
        evTrigger.triggers.Add(pointerDown);
    }

    /****************************************/

    // Use this for initialization
    void Start () {
        inputModule = FindObjectOfType<StandaloneInputModule>();
        if (numIconsSelected == -1)
            numIconsSelected = iconButtons.Length;
        zeroButton.onClick.AddListener(ToggleZero);
        BindButtonSelectEvent(zeroButton);
        foreach (Button icon in iconButtons) {
            icon.onClick.AddListener(() => this.OnClick(icon));
            BindButtonSelectEvent(icon);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Manage input cooldown
        if (!Input.GetButton(inputModule.horizontalAxis)) // If let go of button, reset cooldown
            timeSinceLastInput = INPUT_DELAY + 1;
        else if (timeSinceLastInput < INPUT_DELAY + 1)
            timeSinceLastInput += Time.unscaledDeltaTime;

        if (EventSystem.current.currentSelectedGameObject == this.gameObject) {
            int prevIconsSelected = numIconsSelected;
            // If horizontal axis used, change percentage
            if (Input.GetAxisRaw(inputModule.horizontalAxis) > 0 &&
                (lastInput <= 0 || timeSinceLastInput >= INPUT_DELAY) // Add delay when holding "Right"
            ) {
                SetValue(numIconsSelected + 1);
                timeSinceLastInput = 0;
                lastInput = 1;
            } else if (Input.GetAxisRaw(inputModule.horizontalAxis) < 0 &&
                (lastInput >= 0 || timeSinceLastInput >= INPUT_DELAY) // Add delay when holding "Left"
            ) {
                SetValue(numIconsSelected - 1);
                timeSinceLastInput = 0;
                lastInput = -1;
            }
            // If any of ^^^ cases ran, play sound. (Notice: Doesn't play sound when trying to change past max value)
            if (prevIconsSelected != numIconsSelected)
                clickSound.Play();
        }
    }
}
