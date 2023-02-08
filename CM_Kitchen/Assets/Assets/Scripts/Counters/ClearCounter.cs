using System;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else {
                
            }
        }
        else {
            if (player.HasKitchenObject()) {

            }
            else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}