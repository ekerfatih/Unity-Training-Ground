using System;
using UnityEngine;
public class KitchenGameManager : MonoBehaviour {

    public static KitchenGameManager Instance;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePauseToggle;



    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _state;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 10f;
    private bool _isGamePaused = false;

    private void Awake() {
        Instance = this;
        _state = State.WaitingToStart;
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnOnPauseAction;
    }

    private void GameInput_OnOnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    public void TogglePauseGame() {
        _isGamePaused = !_isGamePaused;
        Time.timeScale = _isGamePaused ? 0 : 1;
        OnGamePauseToggle?.Invoke(this,EventArgs.Empty);
    }

    private void Update() {
        switch (_state) {

            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer < 0f) {
                    _state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0f) {
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    _state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0f) {
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver: break;
        }
        Debug.Log(_state);
    }

    public bool IsGamePlaying() {
        return _state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive() {
        return _state == State.CountdownToStart;
    }
    public bool IsGameOver() {
        return _state == State.GameOver;
    }
    public float GetCountdownToStartTimer() {
        return _countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized() {
        return 1 -(_gamePlayingTimer / _gamePlayingTimerMax);
    }

}