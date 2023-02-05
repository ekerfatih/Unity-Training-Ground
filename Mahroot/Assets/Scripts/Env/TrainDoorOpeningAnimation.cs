using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDoorOpeningAnimation : MonoBehaviour {
    private Animator _animator;
    public AudioClip doorOpeningSound;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void IsDoorOpen(bool state) {
        _animator.SetBool("open", state);
    }
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            IsDoorOpen(true);
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            IsDoorOpen(false);
        }
    }
}
