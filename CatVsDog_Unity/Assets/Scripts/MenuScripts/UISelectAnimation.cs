/* Resize this GUI object when it is selected.
 */

using UnityEngine;
using UnityEngine.EventSystems; // For EventSystem

[RequireComponent(typeof(Animator))]
public class UISelectAnimation : MonoBehaviour {
    public string selectedTrigger = "Highlighted";
    public string deselectTrigger = "Normal";

    private bool prevSelected = false;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        prevSelected = EventSystem.current.currentSelectedGameObject == this.gameObject;
        if (prevSelected) {
            anim.SetTrigger(selectedTrigger);
            anim.ResetTrigger(deselectTrigger);
        } else {
            anim.ResetTrigger(selectedTrigger);
            anim.SetTrigger(deselectTrigger);
        }
    }

    // Update is called once per frame
    void Update() {
        bool nowSelected = EventSystem.current.currentSelectedGameObject == this.gameObject;
        if (!prevSelected && nowSelected) {
            anim.SetTrigger(selectedTrigger);
            anim.ResetTrigger(deselectTrigger);
        }
        else if (prevSelected && !nowSelected) {
            anim.ResetTrigger(selectedTrigger);
            anim.SetTrigger(deselectTrigger);
        }
        prevSelected = nowSelected;
    }
}
