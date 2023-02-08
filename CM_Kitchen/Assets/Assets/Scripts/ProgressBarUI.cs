using System;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarUI : MonoBehaviour {


    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress _hasProgress;
    private void Start() {
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(_hasProgress == null) Debug.LogError("GameObject "+hasProgressGameObject + " does not have a component that implements IHasProgress!");
        _hasProgress.OnProgressChanged += HasProgressOnOnProgressChanged;
        Hide();
    }

    private void HasProgressOnOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        if(e.progressNormalized == 0 || e.progressNormalized == 1) {
            Hide();
        }
        else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}