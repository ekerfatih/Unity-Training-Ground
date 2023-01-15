using System;
using System.Collections;
using UnityEngine;

public class AIMovePattern : MonoBehaviour {
    
    //Declare random angle and hit it with correct transform point
    private AIStates _aiStates;



    private void UpdateAiState(AIStates state) {

        _aiStates = state;
        switch (state) {
            case AIStates.WaitingTurn: break;
            case AIStates.CatchBall: break;
        }
    }

    private IEnumerator Idle() {
        yield break;
    }
    private IEnumerator Turn() {
        yield break;
    }
}

public enum AIStates {
    WaitingTurn,
    CatchBall,
}