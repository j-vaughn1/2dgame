using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public enum TurnEnum { Dog, Cat, None }
    private TurnEnum currentTurn;
    private TurnEnum nextPlayerTurn;

    public PlayerMover catMover;
    public PlayerMover dogMover;

    public BarUpdater turnBar;

    public bool catGoesFirst = true;

    public float maxTurnTime = 30; // In seconds
    public float currentTurnTime = 30; // In seconds

    private void StartTurn(TurnEnum newTurn) {
        currentTurn = newTurn;
        currentTurnTime = maxTurnTime;
        catMover.enabled = (currentTurn == TurnEnum.Cat);
        dogMover.enabled = (currentTurn == TurnEnum.Dog);
        if (currentTurn == TurnEnum.Cat)
            nextPlayerTurn = TurnEnum.Dog;
        if (currentTurn == TurnEnum.Dog)
            nextPlayerTurn = TurnEnum.Cat;
    }
    public void StartNextPlayerTurn() {
        StartTurn(nextPlayerTurn);
    }

    public TurnEnum getTurn() {
        return currentTurn;
    }

    /****************************/

    // Use this for initialization
    void Start () {
        if (catGoesFirst)
            StartTurn(TurnEnum.Cat);
        else
            StartTurn(TurnEnum.Dog);
    }
	
	// Update is called once per frame
	void Update () {
        if (currentTurnTime >= 0)
            currentTurnTime -= Time.deltaTime;

        turnBar.maxValue = maxTurnTime;
        if (currentTurn != TurnEnum.None) {
            turnBar.currentValue = currentTurnTime;
        } else {
            turnBar.currentValue = 0;
        }

        // End of turn
        if (currentTurnTime <= 0) {
            // Stop player's turn
            if (currentTurn != TurnEnum.None) {
                StartTurn(TurnEnum.None);
            // Start the next turn
            } else {
                StartNextPlayerTurn();
            }
        }
	}
}
