using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Transform _cam;
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private const float PlayerSpeed = 2.0f;
    private const float JumpHeight = 1.0f;
    private const float GravityValue = -9.81f;
    private bool _isPlayerAwayFromTrain;
    private bool _isPlayerAwayFromDoor;
    private TrainDoorOpeningAnimation _trainDoorOpeningAnimation;
    private GateOpening _gateOpening;
    private void Start() {
        _controller = gameObject.GetComponent<CharacterController>();
        _cam = Camera.main.transform;
        Cursor.visible = false;
            _trainDoorOpeningAnimation = FindObjectOfType<TrainDoorOpeningAnimation>();
            _gateOpening = FindObjectOfType<GateOpening>();
    }
    
    
    void Update() {
        Movement();
        
        
        if(!_isPlayerAwayFromTrain && transform.position.x > 10) {
            _trainDoorOpeningAnimation.IsDoorOpen(false);
            _isPlayerAwayFromTrain = true;
        }
        
        if(!_isPlayerAwayFromDoor && transform.position.x > 30) {
            _gateOpening.ActivateDoor(false);
            _isPlayerAwayFromDoor = true;
        }
        
        
    }
    private void Movement() {

        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0) {
            _playerVelocity.y = 0f;
        }
        float isRunning = Input.GetKey(KeyCode.LeftShift) ? 4 : 2;


        Vector2 movement = InputManager.Instance.GetPlayerMovement();
        if (movement.y == -1) isRunning = 2;
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = _cam.forward * move.z * isRunning + _cam.transform.right * move.x * 2;
        move.y = 0;
        _controller.Move(move * (Time.deltaTime * PlayerSpeed));

        if (move != Vector3.zero) {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (InputManager.Instance.PlayerJumped() && _groundedPlayer) {
            _playerVelocity.y += Mathf.Sqrt(JumpHeight * -3.0f * GravityValue);
        }

        _playerVelocity.y += GravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
}