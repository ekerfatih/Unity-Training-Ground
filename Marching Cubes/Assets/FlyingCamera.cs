using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCamera : MonoBehaviour {

    public Camera cam;
    public float speed;
    public WorldGenerator WorldGenerator;
    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + (cam.transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal")), speed * Time.deltaTime);
        transform.Rotate(new Vector3(0,Input.GetAxis("Mouse X"), 0));
        cam.transform.Rotate(new Vector3( -Input.GetAxis("Mouse Y"),0, 0));

        if (Input.GetMouseButton(0)) {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Terrain") {
                    WorldGenerator.GetChunkFromVector3(hit.transform.position).PlaceTerrain(hit.point);
                }
            }
        }
        if (Input.GetMouseButton(1)) {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Terrain") {
                    WorldGenerator.GetChunkFromVector3(hit.transform.position).RemoveTerrain(hit.point);
                }
            }
        }
    }
}