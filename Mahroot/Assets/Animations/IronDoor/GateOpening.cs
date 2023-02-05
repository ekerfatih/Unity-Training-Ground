using System;
using UnityEngine;

public class GateOpening : MonoBehaviour {
                            
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void ActivateDoor(bool state) {
        _animator.SetBool("open",state);
    }
}