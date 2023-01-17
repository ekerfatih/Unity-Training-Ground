using System;
using Unity.VisualScripting;
using UnityEngine;

public class mover : MonoBehaviour {

    public Transform blade;
    public Vector3 offset;
    public float speed;
    public LayerMask ground;
    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit,ground)) {
            blade.transform.position = Vector3.Lerp(blade.transform.position, hit.point + offset, Time.deltaTime * speed);
        }
        var lookAt = hit.point -blade.transform.position ;
        var rot =Quaternion.LookRotation(lookAt);
        rot.x = 0;
        rot.z = 0;
        blade.transform.rotation = rot;
    }



}