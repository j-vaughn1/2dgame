using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
    public enum TurnEnum { Dog, Cat, None }
    private TurnEnum currentTurn;
    private TurnEnum nextPlayerTurn;

    public PlayerMover catMover;
    public PlayerMover dogMover;

    public BarUpdater turnBar;
    public Text turnText;

    public bool catGoesFirst = true;

    public float maxTurnTime = 20; // In seconds
    public float currentTurnTime = 10; // In seconds
    public float idleTime = 3; //In seconds, idle time between each player turn.

    private void StartTurn(TurnEnum newTurn) {
        currentTurn = newTurn;

        if (currentTurn != TurnEnum.None)
            currentTurnTime = maxTurnTime;
        else
            currentTurnTime = idleTime;

        catMover.enabled = (currentTurn == TurnEnum.Cat);
        dogMover.enabled = (currentTurn == TurnEnum.Dog);
        if (currentTurn == TurnEnum.Cat)
        {
            turnText.text = "Cat Turn";
            nextPlayerTurn = TurnEnum.Dog;
        }
        if (currentTurn == TurnEnum.Dog)
        {
            turnText.text = "Dog Turn";
            nextPlayerTurn = TurnEnum.Cat;

            //Throw the bone
            FindObjectOfType<AI>().Throw();
        }
        if (currentTurn == TurnEnum.None)
            turnText.text = "Wait!!";
        
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

        if (currentTurn != TurnEnum.None)
            turnBar.maxValue = maxTurnTime;
        else
            turnBar.maxValue = idleTime;

        turnBar.currentValue = currentTurnTime;
       
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
