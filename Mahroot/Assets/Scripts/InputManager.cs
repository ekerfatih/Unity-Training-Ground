using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    private event EventHandler OnInteractAction;
    private PlayerControls _playerControls;
    public static InputManager Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable() {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
        //_playerControls.Player.Interact.performed += InteractOnperformed;
    }

    private void InteractOnperformed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    private void OnDisable() {
        _playerControls.Disable();
    }

    public Vector2 GetPlayerMovement() {
        return _playerControls.Player.Move.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta() {
        return _playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumped() {
        return _playerControls.Player.Jump.triggered;
    }
    

}