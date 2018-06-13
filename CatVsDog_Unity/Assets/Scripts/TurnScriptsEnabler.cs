/*
 * At the start of the given turn, enables this list of scripts.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils; // For ASignal

public class TurnScriptsEnabler : MonoBehaviour {
    public MonoBehaviour[] scripts;
    public TurnManager.TurnEnum enableTurn;

    public void Awake() {
        Signals.Get <TurnManager.TurnStartSignal>().AddListener(OnTurnStart);
        TurnManager turnManager = FindObjectOfType<TurnManager>();
        if (turnManager != null && turnManager.gameObject.activeSelf && turnManager.enabled) { // To handle DEBUG: TurnManager might be disabled
            foreach (MonoBehaviour scr in scripts)
                scr.enabled = (enableTurn == turnManager.GetTurn());
        } else {
            if (turnManager == null)
                Debug.LogWarning(this +" cannot run; TurnManager not found");
            else
                Debug.LogWarning(this + " cannot run; TurnManager is disabled");
        }
    }
    public void OnDestroy() {
        Signals.Get<TurnManager.TurnStartSignal>().RemoveListener(OnTurnStart);
    }

    // This should be bound to the TurnStart event in TurnManager.
    public void OnTurnStart(TurnManager.TurnEnum newTurn) {
        foreach (MonoBehaviour scr in scripts)
            scr.enabled = (enableTurn == newTurn);
    }
}
