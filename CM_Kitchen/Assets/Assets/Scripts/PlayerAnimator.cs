using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private Animator _animator;
    private Player _player;
    private const string IS_WALKING = "IsWalking";
    private void Awake() {
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
    }
    private void Update() {
        _animator.SetBool(IS_WALKING,_player.IsWalking());
    }
}