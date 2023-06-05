using System;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {

    public float force = 10f;
    public float forceOffset = 0.1f;
    private void Update() {
        if(Input.GetMouseButton(0)) {
            HandleInput();
        }
    }

    private void HandleInput() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit)) {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer) {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
            }
        }
    }
}