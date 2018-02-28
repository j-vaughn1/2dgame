using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public enum TurnEnum { Dog, Cat, None }
    private TurnEnum currentTurn;
    private TurnEnum nextPlayerTurn;

    public PlayerMover catMover;
    public PlayerMover dogMover;

    public bool catGoesFirst = true;

    public float turnTime = 30; // In seconds
    public float TurnEnumTime = 30; // In seconds

    private void StartTurn(TurnEnum newTurn) {
        currentTurn = newTurn;
        TurnEnumTime = turnTime;
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
        if (TurnEnumTime >= 0)
            TurnEnumTime -= Time.deltaTime;

        // End of turn
        if (TurnEnumTime <= 0) {
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
