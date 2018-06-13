/* This class represents something that can happen... that needs to wait on an animation.
 * 
 * Example Use Case:
 * When character is throwing Ammo, they want to start an animation before the object is thrown.
 * At some point in the animation, it will have an event that tells this Action to continue...
 * 
 * and the Ammo is actually thrown.
 * 
 * References:
 * Coroutines - https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
 * 
 */

using UnityEngine;
using UnityEngine.Events;

public class AnimationAction<T> where T : class {
    private System.Action<T> onActionDoneFunc;
    private bool actionActive = false;
    private T actionInfo = null;

    [System.Serializable]
    public class ActionStartEvent : UnityEvent<T> { }

    [HideInInspector]
    public ActionStartEvent onActionStart = new ActionStartEvent();

    public AnimationAction(System.Action<T> onActionDoneFunc) {
        this.onActionDoneFunc = onActionDoneFunc;
    }

    /* Begin the action.
     */
    public void StartAction(T actionInfo) {
        actionActive = true;
        this.actionInfo = actionInfo;
        onActionStart.Invoke(actionInfo);
    }

    /* Tell the original script that the Action has been completed.
     * (Call onActionDoneFunc)
     */
    public void FinishAction() {
        if (actionActive) {
            onActionDoneFunc(actionInfo);
            actionActive = false;
            actionInfo = null;
        }
    }

    /* Stop the action.
     */
    public void CancelAction() {
        actionActive = false;
        actionInfo = null;
    }

    /* Returns true if the action is running.
     */
    public bool IsActionRunning() {
        return actionActive;
    }
}
