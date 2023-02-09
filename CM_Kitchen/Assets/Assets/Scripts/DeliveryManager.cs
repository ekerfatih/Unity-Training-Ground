using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour {
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance;
    [SerializeField] private RecipeListSO recipeListSo;
    private List<RecipeSO> _waitingRecipeSoList;
    private float _spawnRecipeTimer;
    private const float SpawnRecipeTimerMax = 4f;
    private const int WaitingRecipesMax = 4;
    private void Awake() {
        
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
            
        _waitingRecipeSoList = new List<RecipeSO>();
    }

    private void Update() {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0) {
            _spawnRecipeTimer = SpawnRecipeTimerMax;
            if (_waitingRecipeSoList.Count < WaitingRecipesMax) {

                RecipeSO waitingRecipeSO = recipeListSo.RecipeSOList[Random.Range(0, recipeListSo.RecipeSOList.Count)];
                _waitingRecipeSoList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < _waitingRecipeSoList.Count; i++) {
            RecipeSO waitingRecipeSo = _waitingRecipeSoList[i];
            if(waitingRecipeSo.KitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectSoList().Count) {
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSo in waitingRecipeSo.KitchenObjectSoList) {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList()) {
                        if(plateKitchenObjectSo == recipeKitchenObjectSo) {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound) {
                        plateContentMatchesRecipe = false;
                    }
                }
                if(plateContentMatchesRecipe) {
                    Debug.Log("true");
                    _waitingRecipeSoList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this,EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this,EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return _waitingRecipeSoList;
    }
}