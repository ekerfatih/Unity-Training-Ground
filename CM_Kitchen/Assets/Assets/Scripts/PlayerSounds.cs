using UnityEngine;
public class PlayerSounds : MonoBehaviour {

    private Player _player;
    private float footStepTimer;
    private float footStepTimerMax = .1f;
    private void Awake() {
        _player = GetComponent<Player>();
    }
    private void Update() {
        footStepTimer -= Time.deltaTime;
        if (footStepTimer < 0f) {
            footStepTimer = footStepTimerMax;
            if (_player.IsWalking()) {
                float volume = 1f;
                SoundManager.Instance.PlayFoostepsSound(transform.position, volume);
            }
        }
    }

}