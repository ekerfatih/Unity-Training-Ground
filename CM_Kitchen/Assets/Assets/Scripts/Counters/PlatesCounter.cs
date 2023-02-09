using System;
using UnityEngine;
public class PlatesCounter : BaseCounter {

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    
    
    [SerializeField] private KitchenObjectSO plateKitchenObjectSo;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int _plateSpawnAmount;
    private int _plateSpawnAmountMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;
            
            if(_plateSpawnAmount < _plateSpawnAmountMax) {
                _plateSpawnAmount++;
                OnPlateSpawned?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if(!player.HasKitchenObject()) {
            if(_plateSpawnAmount > 0) {
                _plateSpawnAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSo, player);
                OnPlateRemoved?.Invoke(this,EventArgs.Empty);
            }
        }
    }

}