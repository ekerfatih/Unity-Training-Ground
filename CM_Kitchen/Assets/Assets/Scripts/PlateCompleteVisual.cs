using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {
    
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSo;
        public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSoGameObjectList;
    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectList) {
                kitchenObjectSoGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectList) {
            if(kitchenObjectSoGameObject.kitchenObjectSo == e.KitchenObjectSo) {
                kitchenObjectSoGameObject.gameObject.SetActive(true);
            }
        }
    }
}