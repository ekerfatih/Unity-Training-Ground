using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIMovePattern : MonoBehaviour {

    //Declare random angle and hit it with correct transform point
    private AIStates currentState;
    private AIStates AIState {
        get { return currentState; }
        set {
            StopAllCoroutines();
            currentState = value;
            switch (currentState) {
                case AIStates.WaitingTurn:
                    //StartCoroutine(Idle());
                    break;

                case AIStates.CatchBall:
                    StartCoroutine(Turn());
                    break;

            }
        }
    }

    private IEnumerator Start() {
        while (true) {
            Debug.Log(Camera.main.WorldToScreenPoint(transform.position).y);
            float screenPosition = math.remap(0, Camera.main.pixelHeight, -1, 1, Camera.main.WorldToScreenPoint(transform.position).y);
            print(screenPosition);
            if (screenPosition == 0) screenPosition = Random.Range(-1,1);

            transform.DOMove(transform.position + transform.forward * screenPosition, 1f);
            AIState = AIStates.WaitingTurn;
            yield return new WaitForSeconds(.5f);
        }
    }
    private void Update() {

    }
    private IEnumerator Turn() {
        yield break;
    }
}

public enum AIStates {
    WaitingTurn,
    CatchBall,
}