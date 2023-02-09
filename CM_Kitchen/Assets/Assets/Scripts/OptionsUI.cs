using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    public static OptionsUI Instance;
    
    [SerializeField] private Button soundEffectButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    private void Awake() {
        Instance = this;
        soundEffectButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void Start() {
        KitchenGameManager.Instance.OnGamePauseToggle += InstanceOnOnGamePauseToggle;
        UpdateVisual();
        Hide();
    }

    private void InstanceOnOnGamePauseToggle(object sender, EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundEffectsText.text = "Sound Effects : " + (SoundManager.Instance.GetVolume() * 10f).ToString("#");
        musicText.text = "Music : " + (MusicManager.Instance.GetVolume() * 10f).ToString("#");
    }

    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
    
}
