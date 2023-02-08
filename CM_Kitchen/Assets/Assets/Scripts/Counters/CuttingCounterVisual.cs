using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {

    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator _animator;
    private static readonly int Cut = Animator.StringToHash("Cut");
    private void Awake() {
        _animator = GetComponent<Animator>();
    }
    private void Start() {
        cuttingCounter.OnCut += CuttingCounterOnOnCut;
        
    }
    private void CuttingCounterOnOnCut(object sender, EventArgs e) {
        _animator.SetTrigger(Cut);
    }

  

}