using System;
using UnityEngine;
public class StoveCounterSound : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start() {
        stoveCounter.OnStateChanged += StoveCounterOnOnStateChanged;
    }

    private void StoveCounterOnOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool playsound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if(playsound) {
            _audioSource.Play();
        }
        else {
            _audioSource.Pause();
        }
    }
}