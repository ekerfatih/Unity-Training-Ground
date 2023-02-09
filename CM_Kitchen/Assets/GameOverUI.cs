using System;
using TMPro;
using UnityEngine;
public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    
    private void Start() {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManagerOnOnStateChanged;
        Hide();
    }

    private void KitchenGameManagerOnOnStateChanged(object sender, EventArgs e) {
        if (KitchenGameManager.Instance.IsGameOver()) {
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            Show();
        }
        else {
            Hide();
        }
    }
    

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}