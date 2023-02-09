using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;
    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectVolume";
    [SerializeField] private AudioClipRefsSO audioClipRefsSo;
    private float _volume = 1f;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, 1f);
    }
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerOnOnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounterOnOnAnyCut;
        Player.Instance.OnPickedSomething += InstanceOnOnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounterOnOnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounterOnOnAnyObjectTrashed;
    }

    private void TrashCounterOnOnAnyObjectTrashed(object sender, EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSo.trash, trashCounter.transform.position);
    }

    private void BaseCounterOnOnAnyObjectPlacedHere(object sender, EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSo.objectDrop, baseCounter.transform.position);
    }

    private void InstanceOnOnPickedSomething(object sender, EventArgs e) {
        PlaySound(audioClipRefsSo.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounterOnOnAnyCut(object sender, EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSo.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManagerOnOnRecipeFailed(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSo.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManagerOnOnRecipeSuccess(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSo.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 pos, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, pos, volumeMultiplier);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 pos, float volumeMultiplier = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], pos, volumeMultiplier * _volume);
    }

    public void PlayFoostepsSound(Vector3 pos, float vol) {
        PlaySound(audioClipRefsSo.footstep, pos, vol);
    }

    public void ChangeVolume() {
        _volume += .1f;
        if (_volume > 1f) {
            _volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME,_volume);
        PlayerPrefs.Save();
    }
    
    public float GetVolume() {
        return _volume;
    }

}