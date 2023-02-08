using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    private const string IS_WALKING = "IsWalking";

    private Animator _animator;
    private Player _player;
    private void Awake() {
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
    }
    private void Update() {
        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}