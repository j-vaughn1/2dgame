using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro; // For Turn text
using deVoid.Utils; // For ASignal

public class TurnManager : MonoBehaviour {
    public enum TurnEnum { Dog, Cat, None } // "None" turn does not end on its own.
    private TurnEnum currentTurn = TurnEnum.None;
    private TurnEnum nextPlayerTurn;

    public BarUpdater turnBar;
    public TMP_Text turnTimeText;
    public Animator turnIndicator;

    public bool catGoesFirst = true;
    
    public float maxTurnTime = 10; // In seconds
    public float idleTime = 3; //In seconds, idle time between each player turn.
    private const float TURN_INDICATOR_LENGTH = 3; // How long the turn transition takes
    private float currentTurnTime = -1; // In seconds (Don't start on someone's turn)

    //List of prefabs to spawn
    public GameObject[] ammoPrefabs;
    public int ammoSpawnAmount = 2;
    //Avoid spawning ammo next to characters
    public Transform[] avoidSpawnNear = { };
    public float avoidSpawnRange = 1;

    public class TurnStartSignal : ASignal<TurnEnum> { }
    public class TurnEndSignal : ASignal<TurnEnum> { }

    private List<System.Object> pauseEvents = new List<System.Object>(); // A list of GameObjects that are doing stuff in-between player turns. The next turn won't start until this array is empty.

    //**************************************

    /* The time (in seconds) left for the current turn. */
    public float GetRemainingTurnTime() {
        return currentTurnTime;
    }

    /* Call this to end the player's turn early.
     */
    public void EndPlayersTurn() {
        if (currentTurn == TurnEnum.Cat || currentTurn == TurnEnum.Dog)
            TransitionToTurn(TurnEnum.None);
    }

    /* Tell the TurnManager that some event is happening in-between turns.
     * The next turn won't begin until the event is done.
     * For example:
     *     When a bone is thrown, players will want to see what it hits before the next turn starts.
     */
    public void AddPauseEvent(System.Object obj) {
        if (pauseEvents.Contains(obj))
            Debug.LogWarning("Tried to add a Pause Event multiple times:"+ obj);
        else
            pauseEvents.Add(obj);
    }
    /* Tell the TurnManager that the event is done.
     * Once all pause events are done, the next turn is started.
     */
    public void RemovePauseEvent(System.Object obj) {
        pauseEvents.Remove(obj);
    }

    /* Ends the turn, and then calls turnEndFunc()
     * Once that function returns, allows the code to continue.
     */
    public void ForceTurnEnd(System.Action turnEndFunc) {
        TransitionToTurn(TurnEnum.None);
        AddPauseEvent(this);
        turnEndFunc();
        RemovePauseEvent(this);
    }
    /* Force the turn to end, and then go to the main menu.
     */
    public void SetGameOver() {
        TransitionToTurn(TurnEnum.None);
        AddPauseEvent(this);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"); //Goto Game scene. Note: Scene name is defined in File > Build settings
        RemovePauseEvent(this);
    }

    //*********************************

    public TurnEnum GetTurn() {
        return currentTurn;
    }

    /****************************/

    // Use this for initialization
    void Start () {
        nextPlayerTurn = (catGoesFirst) ? TurnEnum.Cat : TurnEnum.Dog;
        TransitionToTurn(TurnEnum.None); // Disable both characters' scripts. Once the turn timer is done, this player will move.
        currentTurnTime = 1; // DEBUG: Speed up turn timer... since no Pause Events have been implemented yet
    }
	
	// Update is called once per frame
	void Update () {
        if (currentTurnTime >= 0) {
            // Decrease turn time
            currentTurnTime -= Time.deltaTime;
            if (currentTurn == TurnEnum.None && pauseEvents.Count > 0)
                currentTurnTime = idleTime;

            // Update Turn timer bar
            turnBar.maxValue = (currentTurn == TurnEnum.None) ? idleTime : maxTurnTime;
            turnBar.currentValue = currentTurnTime;

            // End of turn
            if (currentTurnTime <= 0) {
                switch (currentTurn) {
                case TurnEnum.Cat:
                case TurnEnum.Dog:
                    TransitionToTurn(TurnEnum.None);
                    break;
                case TurnEnum.None:
                    TransitionToTurn(nextPlayerTurn);
                    break;
                }
            }
        }

        // Show turn time
        if (currentTurn != TurnEnum.None) {
            turnTimeText.SetText(""+ Mathf.Max(Mathf.Ceil(currentTurnTime), 0));
        } else { // Hide turn time in-between character turns
            turnTimeText.SetText("");
        }
	}

    //*****************************

    /* Begin the turn transition... */
    private void TransitionToTurn(TurnEnum newTurn) {
        currentTurnTime = -1;
        Signals.Get<TurnEndSignal>().Dispatch(currentTurn);

        // Start the next turn...
        if (newTurn != TurnEnum.None) { // On a character's turn...
            nextPlayerTurn = newTurn;
            DestroyObjects(GameObject.FindGameObjectsWithTag("Ammo-Static")); // DEBUG: Destroy all leftover ammo at the end of the turn
            Invoke("OnPlayerTurnIndicatorFinished", TURN_INDICATOR_LENGTH); // Wait for player's turn indicator to finish before starting their turn...
            SpawnAmmo();
        }

        switch (newTurn) {
        case TurnEnum.Cat:
            turnIndicator.SetBool("Cat Turn_Start", true);
            break;
        case TurnEnum.Dog:
            turnIndicator.SetBool("Dog Turn_Start", true);
            break;
        case TurnEnum.None: // The "None" turn doesn't have a transition, so start it right away
            currentTurn = newTurn;
            currentTurnTime = idleTime;
            Signals.Get<TurnStartSignal>().Dispatch(currentTurn);
            break;
        }
    }
    /* After the turn indicator animation happens, let the character move. */
    private void OnPlayerTurnIndicatorFinished() {
        currentTurn = nextPlayerTurn;
        currentTurnTime = maxTurnTime;
        Signals.Get<TurnStartSignal>().Dispatch(currentTurn);
        nextPlayerTurn = (currentTurn == TurnEnum.Cat) ? TurnEnum.Dog : TurnEnum.Cat; // Let the other player go next
    }

    //*****************************

    /* Used to spawn more ammo, since the ammo explodes */
    void SpawnAmmo() {
        for (int i = 0; i < ammoSpawnAmount; ++i) {
            float yPos = .5f;                        //spawn the bone at a random location
            float xPos;
            // Keep trying to pick spawn positions until a valid one is picked
            bool invalidSpawnPos = true; // Need to run this at least once
            do {
                xPos = Random.Range(-18f, 18f);
                invalidSpawnPos = false;
                foreach (Transform avoidObj in avoidSpawnNear) {
                    if (Mathf.Abs(xPos - avoidObj.position.x) < avoidSpawnRange)
                        invalidSpawnPos = true;
                }
            } while (invalidSpawnPos);

            GameObject ammo = ammoPrefabs[  Random.Range(0, ammoPrefabs.Length)  ];
            Instantiate(ammo, new Vector3(xPos, yPos), transform.rotation);
        }
    }

    /* Used to destroy bones. */
    void DestroyObjects(GameObject[] x) {
        foreach(GameObject obj in x) {
//            print("Destroying " + obj);
            Destroy(obj);
        }
    }
}
