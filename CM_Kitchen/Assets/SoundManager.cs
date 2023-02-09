using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;
    
    [SerializeField] private AudioClipRefsSO audioClipRefsSo;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
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
        PlaySound(audioClipRefsSo.trash,trashCounter.transform.position);
    }

    private void BaseCounterOnOnAnyObjectPlacedHere(object sender, EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSo.objectDrop,baseCounter.transform.position);
    }

    private void InstanceOnOnPickedSomething(object sender, EventArgs e) {
        PlaySound(audioClipRefsSo.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounterOnOnAnyCut(object sender, EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSo.chop,cuttingCounter.transform.position);
    }

    private void DeliveryManagerOnOnRecipeFailed(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSo.deliveryFail,deliveryCounter.transform.position);
    }

    private void DeliveryManagerOnOnRecipeSuccess(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSo.deliverySuccess,deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip,Vector3 pos,float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip,pos,volume);
    }
    private void PlaySound(AudioClip[] audioClipArray,Vector3 pos,float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)],pos,volume);
    }

    public void PlayFoostepsSound(Vector3 pos,float vol) {
        PlaySound(audioClipRefsSo.footstep,pos,vol);
    }
    
}