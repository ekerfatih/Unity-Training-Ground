using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapsuleScc : MonoBehaviour {
    private Transform _player;
    [SerializeField] private GameObject visible;
    [SerializeField] private GameObject ghost;
    private void Awake() {
        _player = FindObjectOfType<PlayerController>().transform;
    }
    private void Update() {
        if(Vector3.Distance(_player.position , transform.position )< 5f) {
            ghost.SetActive(true);
            visible.SetActive(true);
        }
        else if(visible.activeSelf && Vector3.Distance(_player.position , transform.position )> 5f){
            visible.SetActive(false);
            ghost.SetActive(false);
        }
        
        if(visible.activeSelf) {
            if(Input.GetKeyDown(KeyCode.E)) {
                  var position  = GameObject.FindWithTag("teleport").transform.position;
                  _player.transform.position = position;
            }
        }
        
    }
}
