using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class cameracontrol : MonoBehaviour {
    [SerializeField] private Transform camera;
    private Transform _player;
    void Start() {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    
    void Update()
    {
        camera.LookAt(_player.position + Vector3.up);
    }
}
