using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orecontrol : MonoBehaviour {
    private Transform _player;
    public GameObject shadow;


    private void Start() {
        _player = FindObjectOfType<PlayerController>().transform;
    }
    private void Update() {
        if (Vector3.Distance(_player.transform.position, transform.position) < 11f) {
            shadow.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E)) {
                Application.Quit();
                Debug.Log("asdd");
            }
        }
        else {
            shadow.SetActive(false);
        }
    }
}
