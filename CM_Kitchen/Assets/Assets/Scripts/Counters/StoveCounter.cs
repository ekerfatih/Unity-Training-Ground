using System;
using UnityEngine;

public class StoveCounter : BaseCounter ,IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSoArray;
    private State _currentState;
    private FryingRecipeSO _fryingRecipeSo;
    private BurningRecipeSO _burningRecipeSo;
    private float _fryingTimer;
    private float _burningTimer;
    private void Start() {
        _currentState = State.Idle;
    }
    private void Update() {
        if (HasKitchenObject()) {
            switch (_currentState) {

                case State.Idle: break;
                case State.Frying:

                    _fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = _fryingTimer / _fryingRecipeSo.fryingTimerMax
                    });

                    if (_fryingTimer > _fryingRecipeSo.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSo.output, this);
                        _currentState = State.Fried;
                        _burningTimer = 0;
                        _burningRecipeSo = GetBurningRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this,new OnStateChangedEventArgs {
                            state = _currentState
                        });
                    }
                    break;
                case State.Fried: 
                    _burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = _burningTimer / _burningRecipeSo.burningTimerMax
                    });

                    if (_burningTimer > _burningRecipeSo.burningTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSo.output, this);
                        _currentState = State.Burned;
                        OnStateChanged?.Invoke(this,new OnStateChangedEventArgs {
                            state = _currentState
                        });
                        OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                
                case State.Burned: break;
            }
        }
    }
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _fryingRecipeSo = GetFryingRecipeSoWithInput(GetKitchenObject().GetKitchenObjectSO());
                    _currentState = State.Frying;
                    _fryingTimer = 0;
                    OnStateChanged?.Invoke(this,new OnStateChangedEventArgs {
                        state = _currentState
                    });
                    
                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = _fryingTimer / _fryingRecipeSo.fryingTimerMax
                    });
                }
            }
        }
        else {
            if (player.HasKitchenObject()) {

            }
            else {
                GetKitchenObject().SetKitchenObjectParent(player);
                _currentState = State.Idle;
                OnStateChanged?.Invoke(this,new OnStateChangedEventArgs {
                    state = _currentState
                });
                
                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSo) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSoWithInput(inputKitchenObjectSo);

        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSo) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSo) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSoWithInput(KitchenObjectSO inputKitchenObjectSo) {
        foreach (BurningRecipeSO burningRecipeSo in burningRecipeSoArray) {
            if (burningRecipeSo.input == inputKitchenObjectSo) {
                return burningRecipeSo;
            }
        }
        return null;
    }
    
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }
}