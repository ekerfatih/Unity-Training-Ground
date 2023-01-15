using System;
using UnityEngine;

public class PlayerMoveBorder : MonoBehaviour {

    public static PlayerMoveBorder Instance;
    
    
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public bool showBorder;
    

}